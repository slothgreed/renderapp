using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Analyzer.Algorithm.FeatureLine
{
    /// <summary>
    /// 特徴稜線用のグラフ
    /// HalfEdgeに似ているが、ポリゴン用ではないのとAddEdgeなど必要で分けている。
    /// </summary>
    public class FeatureLineGraph
    {
        List<HalfEdge> edges = new List<HalfEdge>();
        List<HalfEdgeVertex> vertexs = new List<HalfEdgeVertex>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vertexNum">頂点数</param>
        public FeatureLineGraph(int vertexNum)
        {
            vertexs = new List<HalfEdgeVertex>(vertexNum);
        }

        /// <summary>
        /// エッジが存在するか
        /// </summary>
        /// <param name="index0">頂点1</param>
        /// <param name="index1">頂点2</param>
        /// <returns>存在するかどうか</returns>
        public int GetEdgeIndex(int index0, int index1)
        {
            foreach (var edge in vertexs[index0].AroundEdge)
            {
                if (edge.End.Index == index1)
                {
                    return edge.Index;
                }
            }

            return -1;
        }

        public void AddVertex(HalfEdgeVertex vertex0, int ov1, int ov2, double val1, double val2)
        {
            if (vertexs[vertex0.Index] == null)
            {
                vertexs[vertex0.Index] = new HalfEdgeVertex(vertex0.Position, vertex0.Index);
            }

            vertexs[vertex0.Index].TmpParameter = new VertexParameter(ov1, ov2, val1, val2);
        }


        /// <summary>
        /// エッジの追加
        /// </summary>
        /// <param name="vertex0">頂点1</param>
        /// <param name="vertex1">頂点2</param>
        /// <returns></returns>
        public int AddEdge(HalfEdgeVertex vertex0, HalfEdgeVertex vertex1)
        {
            HalfEdgeVertex start = vertexs[vertex0.Index];
            HalfEdgeVertex end = vertexs[vertex0.Index];

            HalfEdge edge = new HalfEdge(start, end, edges.Count);
            // 逆向きのエッジも含める
            end.AddEdge(edge);
            edges.Add(edge);

            return 0;
        }

        private class VertexParameter
        {
            double si;
            double sy;
            double kval;

            int ov1;
            int ov2;
            double val1;
            double val2;
            double tconner;

            public VertexParameter(int ov1, int ov2, double val1, double val2)
            {
                this.ov1 = ov1;
                this.ov2 = ov2;
                this.val1 = val1;
                this.val2 = val2;
                this.tconner = 0;
                this.si = 0;
                this.sy = 0;
                this.kval = 0;
            }
        }
    }
}
