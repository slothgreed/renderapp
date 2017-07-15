using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using KI.Foundation.Utility;

namespace KI.Gfx.Analyzer
{
    public enum VertexParam
    {
        MinCurvature,
        MaxCurvature,
        MeanCurvature,
        GaussCurvature,
        Voronoi
    }

    public class Vertex
    {
        /// <summary>
        /// temporaryEdgeforopposite
        /// </summary>
        public List<Edge> m_AroundEdge = new List<Edge>();
        public Vector3 Position
        {
            get;
            set;
        }



        /// <summary>
        /// HalfEdgeでもつm_VertexのIndex番号
        /// </summary>
        public int Index { get; set; }
        public Vector3 MaxVector { get; set; }
        public Vector3 MinVector { get; set; }
        private Dictionary<VertexParam, object> Parameter;


        public void AddParameter(VertexParam param, float value)
        {
            Parameter.Add(param, value);
        }
        public object GetParameter(VertexParam param)
        {
            return Parameter[param];
        }
        /// <summary>
        /// テンポラリ計算用フラグ
        /// </summary>
        public object CalcFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlag { get; set; }

        public bool ErrorVertex
        {
            get
            {
                return AroundEdge.Any(p => p.DeleteFlag) || DeleteFlag;
            }
        }

        public Vertex(Vector3 pos,int index = -1)
        {
            Parameter = new Dictionary<VertexParam, object>();
            Position = pos;
            Index = index;
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

        public IEnumerable<Edge> AroundEdge
        {
            get
            {
                //opposite計算後は削除されている。
                if (m_AroundEdge != null)
                {
                    foreach (var edge in m_AroundEdge)
                    {
                        yield return edge;
                    }
                }
            }
        }
        public IEnumerable<Mesh> AroundMesh
        {
            get
            {
                foreach (var edge in AroundEdge)
                {
                    yield return edge.Mesh;
                }
            }
        }
        public IEnumerable<Vertex> AroundVertex
        {
            get
            {
                foreach (var edge in AroundEdge)
                {
                    yield return edge.End;
                }
            }
        }

        public void RemoveAroundEdge(Edge edge)
        {
            m_AroundEdge.Remove(edge);
        }

        private Vector3 _normal = Vector3.Zero;
        public Vector3 Normal
        {
            get
            {
                if (_normal == Vector3.Zero)
                {
                    if(AroundMesh.Count() != 0)
                    {
                        Vector3 sum = Vector3.Zero;
                        int count = AroundMesh.Count();
                        foreach (var mesh in AroundMesh)
                        {
                            sum += mesh.Normal;
                        }
                        sum.X /= count;
                        sum.Y /= count;
                        sum.Z /= count;
                        _normal = sum.Normalized();
                    }
                }
                return _normal;
            }
        }
        public void Dispose()
        {
            DeleteFlag = true;
            m_AroundEdge = null;
        }

        #region [operator]
        public static Vector3 operator +(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position + v2.Position);
        }
        public static Vector3 operator -(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position - v2.Position);
        }
        public static Vector3 operator *(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position * v2.Position);
        }
        public static bool operator ==(Vertex v1, Vertex v2)
        {
            if (object.ReferenceEquals(v1, v2))
            {
                return true;
            }
            if ((object)v1 == null || (object)v2 == null)
            {
                return false;
            }

            if (Math.Abs(v1.Position.X - v2.Position.X) > KICalc.THRESHOLD05)
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
        public static bool operator !=(Vertex v1, Vertex v2)
        {
            return !(v1 == v2);
        }

        #endregion
    }
}
