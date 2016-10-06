using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.Analyzer
{
    public class Mesh
    {
        /// <summary>
        /// エッジ
        /// </summary>
        private List<Edge> m_Edge = new List<Edge>();
        /// <summary>
        /// 頂点
        /// </summary>
        private List<Vertex> m_Vertex = new List<Vertex>();
        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlg { get; set; }
        
        public Vector3 Normal { get; set; }

        public Mesh()
        {
            
        }
        public void SetEdge(Edge edge1, Edge edge2, Edge edge3)
        {
            m_Edge.Add(edge1);
            m_Edge.Add(edge2);
            m_Edge.Add(edge3);
        }
        public List<Edge> GetAroundEdge()
        {
            return m_Edge;
        }
        public void SetVertex(Vertex ver1, Vertex ver2, Vertex ver3)
        {
            m_Vertex.Add(ver1);
            m_Vertex.Add(ver2);
            m_Vertex.Add(ver3);
        }
        public List<Vertex> GetAroundVertex()
        {
            return m_Vertex;
        }
        public void Dispose()
        {
            DeleteFlg = true;
            m_Vertex.Clear();
            m_Edge.Clear();
            m_Vertex = null;
            m_Edge = null;
        }
    }
}
