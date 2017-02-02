using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using KI.Foundation.Utility;

namespace KI.Gfx.Analyzer
{
    public class Vertex
    {
        //あとの管理がめんどくさくなるので、vertexは1つだけエッジを持つようにする。
        public Edge m_Edge { get; set; }
        /// <summary>
        /// temporaryEdgeforopposite
        /// </summary>
        private List<Edge> m_AroundEdge = new List<Edge>();
        public Vector3 Position
        {
            get;
            private set;
        }
        /// <summary>
        /// HalfEdgeでもつm_VertexのIndex番号
        /// </summary>
        public int Index { get; set; }
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
        public static bool operator ==(Vertex v1,Vertex v2)
        {
            if(object.ReferenceEquals(v1,v2))
            {
                return true;
            }
            if ((object)v1 == null || (object)v2 == null)
            {
                return false;
            }

            if(Math.Abs(v1.Position.X - v2.Position.X) > KICalc.THRESHOLD05)
            {
                return false;
            }
            if (Math.Abs(v1.Position.Y - v2.Position.Y) > KICalc.THRESHOLD05)
            {
                return false;
            }
            if (Math.Abs(v1.Position.Z - v2.Position.Z) > KICalc.THRESHOLD05)
            {
                return false;
            }
            return true;
        }
        public static bool operator !=(Vertex v1,Vertex v2)
        {
            return !(v1 == v2);
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
            m_Edge = edge;
            if(!m_AroundEdge.Contains(edge))
            {
                m_AroundEdge.Add(edge);
            }
        }
        /// <summary>
        /// 頂点の持つエッジの削除
        /// </summary>
        /// <param name="edge"></param>
        public void ClearEdge()
        {
            m_AroundEdge.Clear();
            m_AroundEdge = null;
        }

        public IEnumerable<Edge> GetAroundEdge()
        {
            //opposite計算後は削除されている。
            if(m_AroundEdge != null)
            {
                foreach (var edge in m_AroundEdge)
                {
                    yield return edge;
                }
            }else
            {
                yield return m_Edge;
                Edge loop = m_Edge.Opposite.Next;
                while (loop != m_Edge)
                {
                    yield return loop;
                    loop = loop.Opposite.Next;
                }
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
            foreach (var edge in GetAroundEdge())
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
            m_Edge = null;
        }
        public bool ErrorVertex()
        {
            return DeleteFlg;
        }
    }
}
