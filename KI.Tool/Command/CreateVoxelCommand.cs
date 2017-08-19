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
        /// ボクセル空間
        /// </summary>
        public VoxelSpace voxel = null;
        
        /// <summary>
        /// 分割数
        /// </summary>
        private int partition = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        /// <param name="part">分割数</param>
        public CreateVoxelCommand(RenderObject asset, int part)
        {
            renderObject = asset as RenderObject;
            partition = part;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult CanExecute(string commandArg)
        {
            return CanCreateGeometry(renderObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Execute(string commandArg)
        {
            Vector3 min;
            Vector3 max;
            List<Vector3> voxelPosition = new List<Vector3>();
            List<Vector3> voxelNormal = new List<Vector3>();
            KICalc.MinMax(renderObject.Geometry.Position, out min, out max);
            voxel = new VoxelSpace(renderObject.Geometry.Position, renderObject.Geometry.Index, partition, min, max);
            GetVoxelObject(out voxelPosition, out voxelNormal);

            RenderObject voxelObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel :" + renderObject.Name);
            Geometry info = new Geometry("Voxel :" + renderObject.Name, voxelPosition, voxelNormal, KICalc.RandomColor(), null, null, GeometryType.Quad);
            voxelObject.SetGeometryInfo(info);
            voxelObject.Transformation(renderObject.ModelMatrix);
            Global.RenderSystem.ActiveScene.AddObject(voxelObject);

            //RenderObject innerObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel Inner : " + renderObject.Name);

            //var voxels = voxel.GetVoxel(VoxelState.Inner);
            //var colors = new List<Vector3>();
            //foreach (var v in voxels)
            //{
            //    colors.Add(KICalc.GetPseudoColor(v.Distance, 0, 100));
            //}

            //Geometry innerInfo = new Geometry("Voxel Inner : " + renderObject.Name, voxel.GetPoint(VoxelState.Inner), null, colors, null, null, GeometryType.Point);
            //innerObject.SetGeometryInfo(innerInfo);
            //innerObject.Transformation(renderObject.ModelMatrix);
            //Global.RenderSystem.ActiveScene.AddObject(innerObject);

            return CommandResult.Success;
        }

        /// <summary>
        /// ボクセル形状の取得
        /// </summary>
        /// <param name="position">位置情報</param>
        /// <param name="normal">法線</param>
        public void GetVoxelObject(out List<Vector3> position, out List<Vector3> normal)
        {
            position = new List<Vector3>();
            normal = new List<Vector3>();
            foreach (var voxel in voxel.Voxels)
            {
                if (voxel.State == VoxelState.Border)
                {
                    SetVoxel(position, normal, voxel.X, voxel.Y, voxel.Z);
                }
            }
        }

        /// <summary>
        /// voxelをpositionにセット
        /// </summary>
        /// <param name="positions">頂点リスト</param>
        /// <param name="normals">法線リスト</param>
        /// <param name="xIndex">x要素番号</param>
        /// <param name="yIndex">y要素番号</param>
        /// <param name="zIndex">z要素番号</param>
        private void SetVoxel(List<Vector3> positions, List<Vector3> normals, int xIndex, int yIndex, int zIndex)
        {
            Vector3 minVoxel = voxel.GetVoxelPosition(xIndex, yIndex, zIndex);
            Vector3 maxVoxel = minVoxel + new Vector3(voxel.Interval);
            //minVoxel += new Vector3(m_Length * 0.1f);
            //maxVoxel -= new Vector3(m_Length * 0.1f);
            Vector3 v0 = new Vector3(minVoxel.X, minVoxel.Y, minVoxel.Z);
            Vector3 v1 = new Vector3(maxVoxel.X, minVoxel.Y, minVoxel.Z);
            Vector3 v2 = new Vector3(maxVoxel.X, maxVoxel.Y, minVoxel.Z);
            Vector3 v3 = new Vector3(minVoxel.X, maxVoxel.Y, minVoxel.Z);
            Vector3 v4 = new Vector3(minVoxel.X, minVoxel.Y, maxVoxel.Z);
            Vector3 v5 = new Vector3(maxVoxel.X, minVoxel.Y, maxVoxel.Z);
            Vector3 v6 = new Vector3(maxVoxel.X, maxVoxel.Y, maxVoxel.Z);
            Vector3 v7 = new Vector3(minVoxel.X, maxVoxel.Y, maxVoxel.Z);
            //手前
            SetCube(positions, normals, v0, v3, v2, v1);
            //右
            SetCube(positions, normals, v1, v2, v6, v5);
            //左
            SetCube(positions, normals, v0, v4, v7, v3);
            //奥
            SetCube(positions, normals, v4, v5, v6, v7);
            //上
            SetCube(positions, normals, v2, v3, v7, v6);
            //下
            SetCube(positions, normals, v1, v5, v4, v0);
        }

        /// <summary>
        /// 立方体の追加
        /// </summary>
        /// <param name="positions">頂点リスト</param>
        /// <param name="normals">法線リスト</param>
        /// <param name="q0">頂点1</param>
        /// <param name="q1">頂点2</param>
        /// <param name="q2">頂点3</param>
        /// <param name="q3">頂点4</param>
        private void SetCube(List<Vector3> positions, List<Vector3> normals, Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
        {
            positions.Add(q0); positions.Add(q1); positions.Add(q2); positions.Add(q3);
            Vector3 normal = KICalc.Normal(q1 - q0, q2 - q0);
            normals.Add(normal);
            normals.Add(normal);
            normals.Add(normal);
            normals.Add(normal);
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
