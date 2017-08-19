using System.Collections.Generic;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    public class VertexNormalAlgorithm
    {
        public List<Vector3> Normal = new List<Vector3>();

        public VertexNormalAlgorithm(HalfEdgeDS half)
        {
            Calculate(half);
        }

        private void Calculate(HalfEdgeDS halfedge)
        {
            Normal.Clear();
            foreach (var vertex in halfedge.Vertexs)
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
