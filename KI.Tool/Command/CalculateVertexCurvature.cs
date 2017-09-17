using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Parameter;
using KI.Foundation.Utility;
using KI.Renderer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Tool.Command
{
    /// <summary>
    /// 曲率の算出コマンド
    /// </summary>
    public class CalculateVertexCurvature : ICommand
    {
        /// <summary>
        /// レンダリング形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">算出オブジェクト</param>
        public CalculateVertexCurvature(KIObject asset)
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
            var vertexInfo = new VertexCurvatureAlgorithm(halfDS);

            var voronoiParam = new ScalarParameter(VertexParam.Voronoi.ToString(), halfDS.HalfEdgeVertexs.Select(p => (float)p.GetParameter(VertexParam.Voronoi)));
            var meanParam = new ScalarParameter(VertexParam.MeanCurvature.ToString(), halfDS.HalfEdgeVertexs.Select(p => (float)p.GetParameter(VertexParam.MeanCurvature)));
            var gaussParam = new ScalarParameter(VertexParam.GaussCurvature.ToString(), halfDS.HalfEdgeVertexs.Select(p => (float)p.GetParameter(VertexParam.GaussCurvature)));
            var minParam = new ScalarParameter(VertexParam.MinCurvature.ToString(), halfDS.HalfEdgeVertexs.Select(p => (float)p.GetParameter(VertexParam.MinCurvature)));
            var maxParam = new ScalarParameter(VertexParam.MaxCurvature.ToString(), halfDS.HalfEdgeVertexs.Select(p => (float)p.GetParameter(VertexParam.MaxCurvature)));
            var minVecParam = new VectorParameter(VertexParam.MinVector.ToString(), halfDS.Vertexs.OfType<HalfEdgeVertex>().Select(p => (Vector3)p.GetParameter(VertexParam.MinVector)));
            var maxVecParam = new VectorParameter(VertexParam.MaxVector.ToString(), halfDS.Vertexs.OfType<HalfEdgeVertex>().Select(p => (Vector3)p.GetParameter(VertexParam.MaxVector)));

            renderObject.Polygon.AddParameter(voronoiParam);
            renderObject.Polygon.AddParameter(meanParam);
            renderObject.Polygon.AddParameter(gaussParam);
            renderObject.Polygon.AddParameter(minParam);
            renderObject.Polygon.AddParameter(maxParam);
            renderObject.Polygon.AddParameter(minVecParam);
            renderObject.Polygon.AddParameter(maxVecParam);


            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
