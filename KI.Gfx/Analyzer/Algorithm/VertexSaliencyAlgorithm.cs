using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KI.Gfx.Analyzer.Algorithm
{
    public class VertexSaliencyAlgorithm : IAnalyzer
    {
        public VertexSaliencyAlgorithm(HalfEdge half)
        {
            Calculate(half);
        }
        public void Calculate(HalfEdge half)
        {

        }
    }
}
