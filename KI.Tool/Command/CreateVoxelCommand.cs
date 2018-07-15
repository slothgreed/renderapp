using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Foundation.Command;
using KI.Gfx.Geometry;
using KI.Asset;
using OpenTK;
using KI.Mathmatics;
using KI.Gfx;

namespace KI.Tool.Command
{
    /// <summary>
    /// ボクセルの作成
    /// </summary>
    public class CreateVoxelCommand : CommandBase
    {
        /// <summary>
        /// ボクセル空間
        /// </summary>
        private VoxelSpace voxel = null;

        /// <summary>
        /// ボクセルカラー
        /// </summary>
        private Vector3 voxelColor;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CreateVoxelCommand(VoxelCommandArgs commandArgs)
            :base(commandArgs)
        {
            voxelColor = commandArgs.Color;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute(CommandArgsBase commandArg)
        {
            var voxelCommandArgs = commandArg as VoxelCommandArgs;
            return CommandUtility.CanCreatePolygon(voxelCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult Execute(CommandArgsBase commandArg)
        {
            var voxelCommandArgs = commandArg as VoxelCommandArgs;
            var targetObject = voxelCommandArgs.TargetObject;
            var partition = voxelCommandArgs.Partition;
            var scene = voxelCommandArgs.Scene;

            Vector3 min;
            Vector3 max;
            List<Vector3> voxelPosition = new List<Vector3>();
            List<Vector3> voxelNormal = new List<Vector3>();
            Calculator.MinMax(targetObject.Polygon.Vertexs.Select(p => p.Position), out min, out max);
            voxel = new VoxelSpace(targetObject.Polygon.Vertexs.Select(p => p.Position).ToList(), targetObject.Polygon.Index, partition, min, max);
            var mesh = GetVoxelObject();

            Polygon info = new Polygon("Voxel :" + targetObject.Name, mesh, PolygonType.Quads);
            RenderObject voxelObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel :" + targetObject.Name, info);
            voxelObject.Transformation(targetObject.ModelMatrix);
            scene.AddObject(voxelObject);

            return CommandResult.Success;
        }

        /// <summary>
        /// ボクセル形状の取得
        /// </summary>
        /// <returns>メッシュリスト</returns>
        public List<Mesh> GetVoxelObject()
        {
            var meshs = new List<Mesh>();
            foreach (var voxel in voxel.Voxels)
            {
                if (voxel.State == VoxelState.Border)
                {
                    SetVoxel(meshs, voxel.X, voxel.Y, voxel.Z);
                }
            }

            return meshs;
        }

        /// <summary>
        /// voxelをpositionにセット
        /// </summary>
        /// <param name="meshs">Meshリスト</param>
        /// <param name="xIndex">x要素番号</param>
        /// <param name="yIndex">y要素番号</param>
        /// <param name="zIndex">z要素番号</param>
        private void SetVoxel(List<Mesh> meshs, int xIndex, int yIndex, int zIndex)
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
            SetCube(meshs, v0, v3, v2, v1);
            //右
            SetCube(meshs, v1, v2, v6, v5);
            //左
            SetCube(meshs, v0, v4, v7, v3);
            //奥
            SetCube(meshs, v4, v5, v6, v7);
            //上
            SetCube(meshs, v2, v3, v7, v6);
            //下
            SetCube(meshs, v1, v5, v4, v0);
        }

        /// <summary>
        /// 立方体の追加
        /// </summary>
        /// <param name="meshs">Meshリスト</param>
        /// <param name="q0">頂点1</param>
        /// <param name="q1">頂点2</param>
        /// <param name="q2">頂点3</param>
        /// <param name="q3">頂点4</param>
        private void SetCube(List<Mesh> meshs, Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
        {
            Vector3 normal = Geometry.Normal(q1 - q0, q2 - q0);

            meshs.Add(
                new Mesh(
                    new Vertex(4 * meshs.Count,     q0, normal, voxelColor),
                    new Vertex(4 * meshs.Count + 1, q1, normal, voxelColor),
                    new Vertex(4 * meshs.Count + 2, q2, normal, voxelColor),
                    new Vertex(4 * meshs.Count + 3, q3, normal, voxelColor)));
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult Undo(CommandArgsBase commandArg)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// ボクセルコマンド
    /// </summary>
    public class VoxelCommandArgs : CommandArgsBase
    {
        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        public RenderObject TargetObject { get; private set; }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// 分割数
        /// </summary>
        public int Partition { get; private set; }

        /// <summary>
        /// ボクセルカラー
        /// </summary>
        public Vector3 Color { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="scene">シーン</param>
        public VoxelCommandArgs(RenderObject targetObject, Scene scene, int partition, Vector3 color)
        {
            this.TargetObject = targetObject;
            this.Scene = scene;
            this.Partition = partition;
            this.Color = color;
        }
    }
}
