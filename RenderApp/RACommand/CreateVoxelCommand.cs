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
using RenderApp.Globals;

namespace RenderApp.RACommand
{
    /// <summary>
    /// ボクセルの作成
    /// </summary>
    public class CreateVoxelCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private Geometry geometry = null;

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
            geometry = asset as Geometry;
            partition = part;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public string CanExecute(string commandArg)
        {
            return CanCreateGeometry(geometry);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public string Execute(string commandArg)
        {
            VoxelSpace voxel = new VoxelSpace(geometry.GeometryInfo.Position, geometry.GeometryInfo.Index, partition);
            RenderObject voxelObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel :" + geometry.Name);
            Geometry info = new Geometry(voxel.vPosition, voxel.vNormal, KICalc.RandomColor(), null, null, GeometryType.Quad);
            voxelObject.SetGeometryInfo(info);
            voxelObject.Geometry.Transformation(geometry.ModelMatrix);
            Workspace.SceneManager.ActiveScene.AddObject(voxelObject);

            RenderObject innerObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel Inner : " + geometry.Name);

            var voxels = voxel.GetVoxel(VoxelSpace.VoxelState.Inner);
            var colors = new List<Vector3>();
            foreach (var v in voxels)
            {
                colors.Add(KICalc.GetPseudoColor(v.Value, 0, 100));
            }

            Geometry innerInfo = new Geometry(voxel.GetPoint(VoxelSpace.VoxelState.Inner), null, colors, null, null, GeometryType.Point);
            innerObject.SetGeometryInfo(innerInfo);
            innerObject.Geometry.Transformation(geometry.ModelMatrix);
            Workspace.SceneManager.ActiveScene.AddObject(innerObject);

            return RACommandResource.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
