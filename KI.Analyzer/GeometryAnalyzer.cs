using System.Collections.Generic;
using KI.Analyzer.Algorithm;
using KI.Foundation.Core;

namespace KI.Analyzer
{
    public enum AnalyzeType
    {
        VertexMeanNormal,
        VertexCurvature,
        VertexSaliency,
    }

    public class GeometryAnalyzer : KIFactoryBase<IAnalyzer>
    {
        public Dictionary<string, IAnalyzer> Storage = new Dictionary<string, IAnalyzer>();

        private HalfEdgeDS HalfEdge
        {
            get;
            set;    
        }

        public GeometryAnalyzer(HalfEdgeDS half)
        {
            HalfEdge = half;
        }

        public void CalculateParameter(AnalyzeType type)
        {
            IAnalyzer analyze = null;
            switch (type)
            {
                case AnalyzeType.VertexMeanNormal:
                    analyze = new VertexNormalAlgorithm(HalfEdge);
                    break;
                case AnalyzeType.VertexCurvature:
                    analyze = new VertexCurvatureAlgorithm(HalfEdge);
                    break;
                case AnalyzeType.VertexSaliency:
                    analyze = new VertexSaliencyAlgorithm(HalfEdge);
                    break;
                default:
                    break;
            }

            AddItem(analyze);
        }
    }
}
