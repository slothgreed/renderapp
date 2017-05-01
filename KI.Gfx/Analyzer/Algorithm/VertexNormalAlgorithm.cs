using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using KI.Foundation.Core;
namespace KI.Gfx.Analyzer.Algorithm
{
    public class VertexNormalAlgorithm : IAnalyzer
    {
        public List<Vector3> Normal = new List<Vector3>();
        public VertexNormalAlgorithm(HalfEdge half)
        {
            Calculate(half);
        }

        private void Calculate(HalfEdge halfedge)
        {
            Normal.Clear();
            foreach (var vertex in halfedge.m_Vertex)
            {
                Vector3 sum = new Vector3();
                int num = 0;
                foreach (var mesh in vertex.AroundMesh)
                {
                    sum += mesh.Normal;
                    num++;
                }
                Normal.Add(sum / num);
            }
        }
    }
}
