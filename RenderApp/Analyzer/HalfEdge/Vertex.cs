using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.Analyzer
{
    public class Vertex
    {
        private List<Edge> m_AroundEdge = new List<Edge>();
        public Vector3 Position
        {
            get;
            private set;
        }
        public int Number = -1;
        public float GaussCurvature;
        public float MeanCurvature;
        public float MinCurvature;
        public float MaxCurvature;
        public float VoronoiRagion;
        public bool calcFrag = false;
        public Vector3 MaxVector { get; set; }
        public Vector3 MinVector { get; set; }

        #region [operator]
        public static Vector3 operator +(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position + v2.Position);
        }
        public static Vector3 operator -(Vertex v1,Vertex v2)
        {
            return new Vector3(v1.Position - v2.Position);
        }
        public static Vector3 operator *(Vertex v1,Vertex v2)
        {
            return new Vector3(v1.Position * v2.Position);
        }
        #endregion


        public Vertex(Vector3 pos,int number)
        {
            Position = pos;
            Number = number;
        }
        /// <summary>
        /// エッジのインデックスのセッタ
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            for (int i = 0; i < m_AroundEdge.Count; i++)
            {
                if (m_AroundEdge[i] == edge)
                {
                    return;
                }
            }
            m_AroundEdge.Add(edge);
        }
        /// <summary>
        /// エッジ
        /// </summary>
        /// <returns></returns>
        public List<Edge> GetAroundEdgeList()
        {
            return m_AroundEdge;
        }
        public IEnumerable<Edge> GetAroundEdge()
        {
            foreach(var edge in m_AroundEdge)
            {
                yield return edge;
            }
        }
        public IEnumerable<Mesh> GetAroundMesh()
        {
            foreach(var edge in GetAroundEdge())
            {
                yield return edge.Mesh;
            }
        }
        /// <summary>
        /// 頂点座標
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPosition()
        {
            return Position;
        }

        private Vector3 _normal;
        public Vector3 Normal
        {
            get
            {
                if (_normal == null)
                {
                    Vector3 sum = Vector3.Zero;
                    int count = 0;
                    foreach (var edge in GetAroundEdge())
                    {
                        sum += edge.Mesh.Normal;
                    }
                    sum.X /= count;
                    sum.Y /= count;
                    sum.Z /= count;
                    _normal = sum.Normalized();
                }
                return _normal;

            }
        }
    }
}
