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
        List<Edge> m_Edge = new List<Edge>();
        private Vector3 m_Vertex;
        public int Number = -1;
        public float GaussCurvature;
        public float MeanCurvature;
        public float MinCurvature;
        public float MaxCurvature;
        public float VoronoiRagion;
        public bool calcFrag = false;
        public Vector3 Normal { get; set; }
        public Vector3 MaxVector { get; set; }
        public Vector3 MinVector { get; set; }
        
        
        public Vertex(Vector3 pos,int number)
        {
            m_Vertex = pos;
            Number = number;
        }
        /// <summary>
        /// エッジのインデックスのセッタ
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            for (int i = 0; i < m_Edge.Count; i++ )
            {
                if(m_Edge[i] == edge)
                {
                    return;
                }
            }
                m_Edge.Add(edge);
        }
        /// <summary>
        /// エッジ
        /// </summary>
        /// <returns></returns>
        public List<Edge> GetEdge()
        {
            return m_Edge;
        }
        /// <summary>
        /// 頂点座標
        /// </summary>
        /// <returns></returns>
        public Vector3 GetVertex()
        {
            return m_Vertex;
        }
    }
}
