using KI.Foundation.Core;
using KI.Analyzer.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private HalfEdge halfEdge
        {
            get;
            set;    
        }

        public GeometryAnalyzer(HalfEdge half)
        {
            halfEdge = half;
        }
        public void CalculateParameter(AnalyzeType type)
        {
            IAnalyzer analyze = null;
            switch (type)
            {
                case AnalyzeType.VertexMeanNormal:
                    analyze = new VertexNormalAlgorithm(halfEdge);
                    break;
                case AnalyzeType.VertexCurvature:
                    analyze = new VertexCurvatureAlgorithm(halfEdge);
                    break;
                case AnalyzeType.VertexSaliency:
                    analyze = new VertexSaliencyAlgorithm(halfEdge);
                    break;
                default:
                    break;
            }
            AddItem(analyze);
        }
    }
}
