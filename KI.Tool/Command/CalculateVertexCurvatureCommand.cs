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

            var voronoiParam = new ScalarParameter("Voronoi", halfDS.HalfEdgeVertexs.Select(p => p.Voronoi));
            var meanParam = new ScalarParameter("MeanCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MeanCurvature));
            var gaussParam = new ScalarParameter("GaussCurvature", halfDS.HalfEdgeVertexs.Select(p => p.GaussCurvature));
            var minParam = new ScalarParameter("MinCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MinCurvature));
            var maxParam = new ScalarParameter("MaxCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MaxCurvature));
            var minVecParam = new VectorParameter("MinVector", halfDS.HalfEdgeVertexs.Select(p => p.MinDirection));
            var maxVecParam = new VectorParameter("MaxVector", halfDS.HalfEdgeVertexs.Select(p => p.MaxDirection));

            halfDS.AddParameter(voronoiParam);
            halfDS.AddParameter(meanParam);
            halfDS.AddParameter(gaussParam);
            halfDS.AddParameter(minParam);
            halfDS.AddParameter(maxParam);
            halfDS.AddParameter(minVecParam);
            halfDS.AddParameter(maxVecParam);

            var dirLine = new List<Line>();
            var dirMaxLine = new List<Line>();
            foreach (var position in halfDS.HalfEdgeVertexs)
            {
                Vertex minStart = new Vertex(0, position.Position - position.MinDirection * 0.5f, Vector3.UnitX);
                Vertex minEnd = new Vertex(0, position.Position + position.MinDirection * 0.5f, Vector3.UnitX);

                Vertex maxStart = new Vertex(0, position.Position - position.MaxDirection * 0.5f, Vector3.UnitY);
                Vertex maxEnd = new Vertex(0, position.Position + position.MaxDirection * 0.5f, Vector3.UnitY);

                dirLine.Add(new Line(minStart, minEnd));
                dirMaxLine.Add(new Line(maxStart, maxEnd));
            }

            RenderObject dirLineObject = RenderObjectFactory.Instance.CreateRenderObject("Principal Direction");
            var lines = new Polygon(halfDS.Name + "Direction", dirLine);
            dirLineObject.SetPolygon(lines);
            dirLineObject.ModelMatrix = renderObject.ModelMatrix;
            Global.RenderSystem.ActiveScene.AddObject(dirLineObject);

            RenderObject dirLineObject2 = RenderObjectFactory.Instance.CreateRenderObject("Principal Direction");
            var lines2 = new Polygon(halfDS.Name + "Direction", dirMaxLine);
            dirLineObject2.SetPolygon(lines2);
            dirLineObject2.ModelMatrix = renderObject.ModelMatrix;
            Global.RenderSystem.ActiveScene.AddObject(dirLineObject2);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
