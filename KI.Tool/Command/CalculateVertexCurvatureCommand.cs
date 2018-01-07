using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Parameter;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.Renderer.Material;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">算出オブジェクト</param>
        public CalculateVertexCurvatureCommand(KIObject asset)
        {
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
            var meanParam = new ScalarParameter("MeanCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MeanCurvature).ToArray());
            var gaussParam = new ScalarParameter("GaussCurvature", halfDS.HalfEdgeVertexs.Select(p => p.GaussCurvature).ToArray());
            var minParam = new ScalarParameter("MinCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MinCurvature).ToArray());
            var maxParam = new ScalarParameter("MaxCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MaxCurvature).ToArray());
            var minVecParam = new VectorParameter("MinVector", halfDS.HalfEdgeVertexs.Select(p => p.MinDirection).ToArray());
            var maxVecParam = new VectorParameter("MaxVector", halfDS.HalfEdgeVertexs.Select(p => p.MaxDirection).ToArray());

            halfDS.AddParameter(voronoiParam);
            halfDS.AddParameter(meanParam);
            halfDS.AddParameter(gaussParam);
            halfDS.AddParameter(minParam);
            halfDS.AddParameter(maxParam);
            halfDS.AddParameter(minVecParam);
            halfDS.AddParameter(maxVecParam);

            var dirMinLine = new List<Line>();
            var dirMaxLine = new List<Line>();
            foreach (var position in halfDS.HalfEdgeVertexs)
            {
                Vertex minStart = new Vertex(0, position.Position - position.MinDirection * 0.01f, Vector3.UnitX);
                Vertex minEnd = new Vertex(0, position.Position + position.MinDirection * 0.01f, Vector3.UnitX);

                Vertex maxStart = new Vertex(0, position.Position - position.MaxDirection * 0.01f, Vector3.UnitY);
                Vertex maxEnd = new Vertex(0, position.Position + position.MaxDirection * 0.01f, Vector3.UnitY);

                dirMinLine.Add(new Line(minStart, minEnd));
                dirMaxLine.Add(new Line(maxStart, maxEnd));
            }

            var parentNode = Global.RenderSystem.ActiveScene.FindNode(renderObject);

            VertexParameterMaterial voronoiMaterial = new VertexParameterMaterial(renderObject.Name + " : Voronoi",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                voronoiParam.Values);

            VertexParameterMaterial meanMaterial = new VertexParameterMaterial(renderObject.Name + " : MeanCurvature",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                meanParam.Values);

            VertexParameterMaterial gaussMaterial = new VertexParameterMaterial(renderObject.Name + " : GaussCurvature",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                gaussParam.Values);

            VertexParameterMaterial minMaterial = new VertexParameterMaterial(renderObject.Name + " : MinCurvature",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                minParam.Values);

            VertexParameterMaterial maxMaterial = new VertexParameterMaterial(renderObject.Name + " : MaxCurvature",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                maxParam.Values);

            renderObject.Materials.Add(voronoiMaterial);
            renderObject.Materials.Add(meanMaterial);
            renderObject.Materials.Add(gaussMaterial);
            renderObject.Materials.Add(minMaterial);
            renderObject.Materials.Add(maxMaterial);

            Global.RenderSystem.ActiveScene.AddObject(voronoiMaterial, parentNode);
            Global.RenderSystem.ActiveScene.AddObject(meanMaterial, parentNode);
            Global.RenderSystem.ActiveScene.AddObject(gaussMaterial, parentNode);
            Global.RenderSystem.ActiveScene.AddObject(minMaterial, parentNode);
            Global.RenderSystem.ActiveScene.AddObject(maxMaterial, parentNode);

            var minLines = new Polygon(halfDS.Name + "Direction", dirMinLine);
            var maxLines = new Polygon(halfDS.Name + "Direction", dirMaxLine);
            GeometryMaterial dirMinMaterial = new GeometryMaterial(renderObject.Name + " : MinDirection", minLines, renderObject.Shader);
            GeometryMaterial dirMaxMaterial = new GeometryMaterial(renderObject.Name + " : MaxDirection", maxLines, renderObject.Shader);
            renderObject.Materials.Add(dirMinMaterial);
            renderObject.Materials.Add(dirMaxMaterial);
            Global.RenderSystem.ActiveScene.AddObject(dirMinMaterial, parentNode);
            Global.RenderSystem.ActiveScene.AddObject(dirMaxMaterial, parentNode);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
