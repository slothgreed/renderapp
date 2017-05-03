using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using KI.Foundation.Utility;
namespace KI.Gfx.Analyzer
{
    public class Mesh
    {
        /// <summary>
        /// エッジ
        /// </summary>
        private List<Edge> m_Edge = new List<Edge>();

        public int Index
        {
            get;
            private set;
        }

        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlg { get; set; }

        private Vector3 _normal = Vector3.Zero;
        public Vector3 Normal
        {
            get
            {
                if(_normal == Vector3.Zero)
                {
                    _normal = KICalc.Normal(
                        m_Edge[1].Start.Position - m_Edge[0].Start.Position,
                        m_Edge[2].Start.Position - m_Edge[0].Start.Position
                    );

                }
                return _normal; 
            }
        }

        public Mesh(int index)
        {
            Index = index;
        }
        public Mesh(Edge edge1, Edge edge2, Edge edge3, int index)
        {
            SetEdge(edge1, edge2, edge3);
            Index = index;
        }

        public void SetEdge(Edge edge1, Edge edge2, Edge edge3)
        {
            m_Edge.Add(edge1);
            m_Edge.Add(edge2);
            m_Edge.Add(edge3);
        }
        public IEnumerable<Edge> AroundEdge
        {
            get
            {
                return m_Edge;
            }
        }
        public IEnumerable<Vertex> AroundVertex
        {
            get
            {
                foreach (var edge in m_Edge)
                {
                    yield return edge.Start;
                }
            }
        }
        public void Dispose()
        {
            DeleteFlg = true;
            m_Edge.Clear();
            m_Edge = null;
        }
        public bool ErrorMesh()
        {
            return DeleteFlg;
        }
    }
}
