using System;
using System.Linq;
using KI.Analyzer;
using KI.Analyzer.Algorithm.MarchingCube;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Renderer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        /// <param name="part">分割数</param>
        public CreateMarchingCubeCommand(KIObject asset, int part)
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
            KICalc.MinMax(renderObject.Polygon.Vertexs.Select(p => p.Position), out min, out max);
            min -= Vector3.One * 10;
            max += Vector3.One * 10;
            var voxel = new VoxelSpace(renderObject.Polygon.Vertexs.Select(p => p.Position).ToList(), renderObject.Polygon.Index, partition, min, max);
            var marching = new MarchingCubesAlgorithm(voxel, 0.8f);

            RenderObject marghingObject = RenderObjectFactory.Instance.CreateRenderObject("MarchingCube :" + renderObject.Name);
            var polygon = new Polygon(marghingObject.Name, marching.Meshs, PrimitiveType.Triangles);
            marghingObject.SetPolygon(polygon);
            marghingObject.ModelMatrix = renderObject.ModelMatrix;
            Global.Renderer.ActiveScene.AddObject(marghingObject);

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
