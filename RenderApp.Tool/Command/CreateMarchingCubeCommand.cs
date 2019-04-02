using System;
using System.Linq;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Mathmatics;
using KI.Tool.Command;
using OpenTK;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// Marching Cube の作成
    /// </summary>
    public class CreateMarchingCubeCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private MarchingCubeCommandArgs marchingCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CreateMarchingCubeCommand(MarchingCubeCommandArgs commandArgs)
        {
            marchingCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(marchingCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Execute()
        {
            Vector3 min;
            Vector3 max;
            var targetObject = marchingCommandArgs.TargetObject;
            Calculator.MinMax(targetObject.Polygon.Vertexs.Select(p => p.Position), out min, out max);
            min -= Vector3.One;
            max += Vector3.One;
            var voxel = new VoxelSpace(targetObject.Polygon.Vertexs.Select(p => p.Position).ToList(), targetObject.Polygon.Index, marchingCommandArgs.Partition, min, max);
            var marching = new MarchingCubesAlgorithm(voxel, 0.8f);

            var polygon = new Polygon("MarchingCube :" + targetObject.Name, marching.Meshs, PolygonType.Triangles);
            RenderObject marghingObject = RenderObjectFactory.Instance.CreateRenderObject(polygon.Name, polygon);
            marghingObject.ModelMatrix = targetObject.ModelMatrix;
            marchingCommandArgs.Scene.AddObject(marghingObject);

            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Marching Cube のコマンド
    /// </summary>
    public class MarchingCubeCommandArgs
    {
        /// <summary>
        /// ターゲットオブジェクト
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
        /// コンストラクタ
        /// </summary>
        /// <param name="targetObject">ターゲットオブジェクト</param>
        /// <param name="scene">シーン</param>
        /// <param name="part">分割数</param>
        public MarchingCubeCommandArgs(RenderObject targetObject, Scene scene, int part)
        {
            Scene = scene;
            TargetObject = targetObject;
            Partition = part;
        }

    }
}
