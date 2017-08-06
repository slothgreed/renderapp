using System;
using System.Collections.Generic;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// ボクセルの作成
    /// </summary>
    public class CreateVoxelCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private RenderObject renderObject = null;

        /// <summary>
        /// 分割数
        /// </summary>
        private int partition = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        /// <param name="part">分割数</param>
        public CreateVoxelCommand(KIObject asset, int part)
        {
            renderObject = asset as RenderObject;
            partition = part;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState CanExecute(string commandArg)
        {
            return CanCreateGeometry(renderObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Execute(string commandArg)
        {
            VoxelSpace voxel = new VoxelSpace(renderObject.Geometry.GeometryInfo.Position, renderObject.Geometry.GeometryInfo.Index, partition);
            RenderObject voxelObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel :" + renderObject.Name);
            Geometry info = new Geometry("Voxel :" + renderObject.Name, voxel.vPosition, voxel.vNormal, KICalc.RandomColor(), null, null, GeometryType.Quad);
            voxelObject.SetGeometryInfo(info);
            voxelObject.Transformation(renderObject.ModelMatrix);
            Global.Scene.AddObject(voxelObject);

            RenderObject innerObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel Inner : " + renderObject.Name);

            var voxels = voxel.GetVoxel(VoxelSpace.VoxelState.Inner);
            var colors = new List<Vector3>();
            foreach (var v in voxels)
            {
                colors.Add(KICalc.GetPseudoColor(v.Value, 0, 100));
            }

            Geometry innerInfo = new Geometry("Voxel Inner : " + renderObject.Name, voxel.GetPoint(VoxelSpace.VoxelState.Inner), null, colors, null, null, GeometryType.Point);
            innerObject.SetGeometryInfo(innerInfo);
            innerObject.Transformation(renderObject.ModelMatrix);
            Global.Scene.AddObject(innerObject);

            return CommandState.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
