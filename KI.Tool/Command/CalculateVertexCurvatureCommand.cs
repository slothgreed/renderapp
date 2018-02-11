using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Parameter;
using KI.Renderer;
using KI.Renderer.Attribute;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// 曲率の算出コマンド
    /// </summary>
    public class CalculateVertexCurvatureCommand : ICommand
    {
        /// <summary>
        /// レンダリング形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// シーン
        /// </summary>
        private Scene scene;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">算出オブジェクト</param>
        public CalculateVertexCurvatureCommand(Scene scene, KIObject asset)
        {
            this.scene = scene;
            renderObject = asset as RenderObject;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public CommandResult CanExecute(string commandArg)
        {
            if (renderObject == null)
            {
                return CommandResult.Failed;
            }

            if (renderObject.Polygon is HalfEdgeDS)
            {
                return CommandResult.Success;
            }

            return CommandResult.Failed;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public CommandResult Execute(string commandArg)
        {
            var halfDS = renderObject.Polygon as HalfEdgeDS;
            //var vertexInfo = new VertexCurvatureAlgorithm(halfDS);

            var voronoiParam = new ScalarParameter("Voronoi", halfDS.HalfEdgeVertexs.Select(p => p.Voronoi).ToArray());
            var laplaceParam = new ScalarParameter("Laplace", halfDS.HalfEdgeVertexs.Select(p => p.Laplace).ToArray());
            var meanParam = new ScalarParameter("MeanCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MeanCurvature).ToArray());
            var gaussParam = new ScalarParameter("GaussCurvature", halfDS.HalfEdgeVertexs.Select(p => p.GaussCurvature).ToArray());
            var minParam = new ScalarParameter("MinCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MinCurvature).ToArray());
            var maxParam = new ScalarParameter("MaxCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MaxCurvature).ToArray());
            var laplaceVecParam = new VectorParameter("LaplaceVector", halfDS.HalfEdgeVertexs.Select(p => p.LaplaceVector).ToArray());
            var minVecParam = new VectorParameter("MinVector", halfDS.HalfEdgeVertexs.Select(p => p.MinDirection).ToArray());
            var maxVecParam = new VectorParameter("MaxVector", halfDS.HalfEdgeVertexs.Select(p => p.MaxDirection).ToArray());

            halfDS.AddParameter(voronoiParam);
            halfDS.AddParameter(laplaceParam);
            halfDS.AddParameter(meanParam);
            halfDS.AddParameter(gaussParam);
            halfDS.AddParameter(minParam);
            halfDS.AddParameter(maxParam);
            halfDS.AddParameter(laplaceVecParam);
            halfDS.AddParameter(minVecParam);
            halfDS.AddParameter(maxVecParam);

            var dirMinLine = new List<Vector3>();
            var dirMaxLine = new List<Vector3>();
            var laplaceLine = new List<Vector3>();
            foreach (var position in halfDS.HalfEdgeVertexs)
            {
                var minStart = new Vector3(position.Position);
                var minEnd = new Vector3(position.Position + position.MinDirection);

                var maxStart = new Vector3(position.Position);
                var maxEnd = new Vector3(position.Position + position.MaxDirection);

                dirMinLine.Add(minStart);
                dirMinLine.Add(minEnd);
                dirMaxLine.Add(maxStart);
                dirMaxLine.Add(maxEnd);

                var laplaceStart = new Vector3(position.Position);
                var laplaceEnd = new Vector3(position.Position + position.LaplaceVector);

                laplaceLine.Add(laplaceStart);
                laplaceLine.Add(laplaceEnd);
            }

            var parentNode = Global.Renderer.ActiveScene.FindNode(renderObject);

            VertexParameterAttribute voronoiAttribute = new VertexParameterAttribute(renderObject.Name + " : Voronoi",
                renderObject.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                voronoiParam.Values);

            VertexParameterAttribute laplaceAttribute = new VertexParameterAttribute(renderObject.Name + " : Laplace",
                renderObject.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                laplaceParam.Values);

            VertexParameterAttribute meanAttribute = new VertexParameterAttribute(renderObject.Name + " : MeanCurvature",
                renderObject.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                meanParam.Values);

            VertexParameterAttribute gaussAttribute = new VertexParameterAttribute(renderObject.Name + " : GaussCurvature",
                renderObject.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                gaussParam.Values);

            VertexParameterAttribute minAttribute = new VertexParameterAttribute(renderObject.Name + " : MinCurvature",
                renderObject.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                minParam.Values);

            VertexParameterAttribute maxAttribute = new VertexParameterAttribute(renderObject.Name + " : MaxCurvature",
                renderObject.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                maxParam.Values);

            renderObject.Attributes.Add(voronoiAttribute);
            renderObject.Attributes.Add(laplaceAttribute);
            renderObject.Attributes.Add(meanAttribute);
            renderObject.Attributes.Add(gaussAttribute);
            renderObject.Attributes.Add(minAttribute);
            renderObject.Attributes.Add(maxAttribute);

            scene.AddObject(voronoiAttribute, parentNode);
            scene.AddObject(laplaceAttribute, parentNode);
            scene.AddObject(meanAttribute, parentNode);
            scene.AddObject(gaussAttribute, parentNode);
            scene.AddObject(minAttribute, parentNode);
            scene.AddObject(maxAttribute, parentNode);

            var wireFrameShader = ShaderCreater.Instance.CreateShader(Asset.ShaderType.WireFrame);

            var normals = renderObject.Polygon.Vertexs.Select(p => p.Normal).ToArray();
            var dirMinAttribute = new DirectionAttribute(renderObject.Name + " : MinDirection", wireFrameShader, dirMinLine.ToArray(), new Vector4(1, 0, 0, 1), normals);
            var dirMaxAttribute = new DirectionAttribute(renderObject.Name + " : MaxDirection", wireFrameShader, dirMaxLine.ToArray(), new Vector4(0, 1, 0, 1), normals);
            renderObject.Attributes.Add(dirMinAttribute);
            renderObject.Attributes.Add(dirMaxAttribute);
            scene.AddObject(dirMinAttribute, parentNode);
            scene.AddObject(dirMaxAttribute, parentNode);

            var laplaceVecAttribute = new DirectionAttribute(renderObject.Name + " : MinDirection", wireFrameShader, laplaceLine.ToArray(), new Vector4(1, 0, 0, 1), normals);
            renderObject.Attributes.Add(laplaceVecAttribute);
            scene.AddObject(laplaceVecAttribute, parentNode);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
