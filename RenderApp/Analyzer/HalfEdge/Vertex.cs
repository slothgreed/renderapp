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
        public int Index { get; private set; }
        public float GaussCurvature;
        public float MeanCurvature;
        public float MinCurvature;
        public float MaxCurvature;
        public float VoronoiRagion;
        public bool calcFrag = false;
        public Vector3 MaxVector { get; set; }
        public Vector3 MinVector { get; set; }
        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlg { get; set; }

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
            Index = number;
        }
        /// <summary>
        /// エッジのセッタ
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            if(!m_AroundEdge.Contains(edge))
            {
                m_AroundEdge.Add(edge);
            }
        }
        /// <summary>
        /// 頂点の持つエッジの削除
        /// </summary>
        /// <param name="edge"></param>
        public void DeleteEdge(Edge edge)
        {
            if(m_AroundEdge.Contains(edge))
            {
                m_AroundEdge.Remove(edge);
            }
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
        public IEnumerable<Vertex> GetAroundVertex()
        {
            foreach(var edge in m_AroundEdge)
            {
                yield return edge.End;
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

        private Vector3 _normal = Vector3.Zero;
        public Vector3 Normal
        {
            get
            {
                if (_normal == Vector3.Zero)
                {
                    Vector3 sum = Vector3.Zero;
                    int count = GetAroundMesh().Count();
                    foreach (var mesh in GetAroundMesh())
                    {
                        sum += mesh.Normal;
                    }
                    sum.X /= count;
                    sum.Y /= count;
                    sum.Z /= count;
                    _normal = sum.Normalized();
                }
                return _normal;
            }
        }
        public void Dispose()
        {
            DeleteFlg = true;
            m_AroundEdge.Clear();
            m_AroundEdge = null;
        }
    }
}
