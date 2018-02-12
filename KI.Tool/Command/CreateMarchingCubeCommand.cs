using System;
using System.Linq;
using KI.Analyzer;
using KI.Analyzer.Algorithm.MarchingCube;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Asset;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Mathmatics;

namespace KI.Tool.Command
{
    /// <summary>
    /// Marching Cube の作成
    /// </summary>
    public class CreateMarchingCubeCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// 分割数
        /// </summary>
        private int partition;

        /// <summary>
        /// シーン
        /// </summary>
        private Scene scene;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        /// <param name="part">分割数</param>
        public CreateMarchingCubeCommand(Scene scene, KIObject asset, int part)
        {
            renderObject = asset as RenderObject;
            partition = part;
            this.scene = scene;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult CanExecute(string commandArg)
        {
            if (renderObject == null)
            {
                return CommandResult.Failed;
            }

            if (renderObject.Polygon.Type != PrimitiveType.Triangles)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
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
            Calculator.MinMax(renderObject.Polygon.Vertexs.Select(p => p.Position), out min, out max);
            min -= Vector3.One;
            max += Vector3.One;
            var voxel = new VoxelSpace(renderObject.Polygon.Vertexs.Select(p => p.Position).ToList(), renderObject.Polygon.Index, partition, min, max);
            var marching = new MarchingCubesAlgorithm(voxel, 0.8f);

            var polygon = new Polygon("MarchingCube :" + renderObject.Name, marching.Meshs, PrimitiveType.Triangles);
            RenderObject marghingObject = RenderObjectFactory.Instance.CreateRenderObject(polygon.Name, polygon);
            marghingObject.ModelMatrix = renderObject.ModelMatrix;
            scene.AddObject(marghingObject);

            return CommandResult.Success;
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
