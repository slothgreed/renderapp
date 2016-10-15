using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RenderApp.Utility;
namespace RenderApp.Analyzer
{
    public class Mesh
    {
        /// <summary>
        /// エッジ
        /// </summary>
        private List<Edge> m_Edge = new List<Edge>();
        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlg { get; set; }

        public Vector3 Normal
        {
            get
            {
                return CCalc.Normal(
                    m_Edge[1].Start.GetPosition() - m_Edge[0].Start.GetPosition(),
                    m_Edge[2].Start.GetPosition() - m_Edge[0].Start.GetPosition()
                    );
            }
        }

        public Mesh()
        {
            
        }
        public void SetEdge(Edge edge1, Edge edge2, Edge edge3)
        {
            m_Edge.Add(edge1);
            m_Edge.Add(edge2);
            m_Edge.Add(edge3);
        }
        public IEnumerable<Edge> GetAroundEdge()
        {
            return m_Edge;
        }
        public IEnumerable<Vertex> GetAroundVertex()
        {
            foreach(var edge in m_Edge)
            {
                yield return edge.Start;
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
