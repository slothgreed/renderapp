using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer.Algorithm.FeatureLine;
using KI.Mathmatics;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// 特徴線
    /// </summary>
    public class FeatureLineAlgorithm
    {
        /// <summary>
        /// 凸部分
        /// </summary>
        FeatureLineGraph ridges = new FeatureLineGraph();

        /// <summary>
        /// 凹部分
        /// </summary>
        FeatureLineGraph ravines = new FeatureLineGraph();

        private HalfEdgeDS halfEdgeDS;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="halfEdgeDS">データ構造</param>
        public FeatureLineAlgorithm(HalfEdgeDS structure)
        {
            halfEdgeDS = structure;
        }

        public List<HalfEdge>[] GetConvexLines()
        {
           return ridges.GetFeatureLines();
        }

        public List<HalfEdge>[] GetConcaveLine()
        {
            return ravines.GetFeatureLines();
        }

        /// <summary>
        /// 計算
        /// </summary>
        public void Calculate()
        {
            foreach (var mesh in halfEdgeDS.HalfEdgeMeshs)
            {
                bool convex = false;
                bool concave = false;
                int edge0_1 = 0;
                int edge1_2 = 0;
                int edge2_0 = 0;

                foreach (var vertex in mesh.AroundVertex)
                {
                    if (vertex.MaxCurvature > Math.Abs(vertex.MinCurvature))
                    {
                        convex = true;
                    }

                    if (-Math.Abs(vertex.MaxCurvature) > vertex.MinCurvature)
                    {
                        concave = true;
                    }
                }

                var vertexs = mesh.AroundVertex.ToArray();
                var vertex0 = vertexs[0];
                var vertex1 = vertexs[1];
                var vertex2 = vertexs[2];
                HalfEdgeVertex checkID1 = null;
                HalfEdgeVertex checkID2 = null;
                if (convex)
                {
                    edge0_1 = CalculateConvexParameter(vertex0, vertex1);
                    edge1_2 = CalculateConvexParameter(vertex1, vertex2);
                    edge2_0 = CalculateConvexParameter(vertex2, vertex0);

                    if (edge0_1 == 1 && edge1_2 == 1)
                    {
                        checkID1 = AddConvexPoint(vertex0, vertex1, ridges);
                        checkID2 = AddConvexPoint(vertex1, vertex2, ridges);
                    }
                    else if (edge1_2 == 1 && edge2_0 == 1)
                    {
                        checkID1 = AddConvexPoint(vertex1, vertex2, ridges);
                        checkID2 = AddConvexPoint(vertex2, vertex0, ridges);

                    }
                    else if (edge2_0 == 1 && edge0_1 == 1)
                    {
                        checkID1 = AddConvexPoint(vertex2, vertex0, ridges);
                        checkID2 = AddConvexPoint(vertex0, vertex1, ridges);
                    }

                    if (checkID1 != null && checkID2 != null)
                    {
                        ridges.AddEdge(checkID1, checkID2);
                    }
                }

                checkID1 = null;
                checkID2 = null;
                if (concave)
                {
                    edge0_1 = CalculateConcaveParameter(vertex0, vertex1);
                    edge1_2 = CalculateConcaveParameter(vertex1, vertex2);
                    edge2_0 = CalculateConcaveParameter(vertex2, vertex0);

                    if (edge0_1 == 1 && edge1_2 == 1)
                    {
                        checkID1 = AddConcavePoint(vertex0, vertex1, ravines);
                        checkID2 = AddConcavePoint(vertex1, vertex2, ravines);
                    }
                    else if (edge1_2 == 1 && edge2_0 == 1)
                    {
                        checkID1 = AddConcavePoint(vertex1, vertex2, ravines);
                        checkID2 = AddConcavePoint(vertex2, vertex0, ravines);

                    }
                    else if (edge2_0 == 1 && edge0_1 == 1)
                    {
                        checkID1 = AddConcavePoint(vertex2, vertex0, ravines);
                        checkID2 = AddConcavePoint(vertex0, vertex1, ravines);
                    }

                    if (checkID1 != null && checkID2 != null)
                    {
                        ravines.AddEdge(checkID1, checkID2);
                    }
                }
            }

            ridges.Connect();
            ravines.Connect();
        }

        /// <summary>
        /// 頂点の追加
        /// </summary>
        /// <returns></returns>
        private HalfEdgeVertex AddConvexPoint(HalfEdgeVertex vertex0, HalfEdgeVertex vertex1, FeatureLineGraph ridges)
        {
            HalfEdgeVertex addVertex = null;
            addVertex = ridges.GetVertexByGenerateEdgeIndex(vertex0.Index, vertex1.Index);

            // vertex0, vertex1 からなるエッジがないならば作る
            if (addVertex == null)
            {
                var alpha = Math.Abs(vertex0.MaxDerivaribe);
                var beta = Math.Abs(vertex1.MaxDerivaribe);
                var position = Interaction.Inter(alpha, beta, vertex0.Position, vertex1.Position);
                addVertex = ridges.AddVertex(position, vertex0.Index, vertex1.Index, alpha, beta);
            }

            return addVertex;
        }

        /// <summary>
        /// 頂点の追加
        /// </summary>
        /// <returns></returns>
        private HalfEdgeVertex AddConcavePoint(HalfEdgeVertex vertex0, HalfEdgeVertex vertex1, FeatureLineGraph ravine)
        {
            HalfEdgeVertex checkID;
            HalfEdgeVertex addVertex = null;
            checkID = ravine.GetVertexByGenerateEdgeIndex(vertex0.Index, vertex1.Index);

            // vertex0, vertex1 からなるエッジがないならば作る
            if (checkID == null)
            {
                var alpha = Math.Abs(vertex0.MaxDerivaribe);
                var beta = Math.Abs(vertex1.MaxDerivaribe);
                var position = Interaction.Inter(alpha, beta, vertex0.Position, vertex1.Position);
                addVertex = ravine.AddVertex(position, vertex0.Index, vertex1.Index, alpha, beta);
            }

            return addVertex;
        }

        /// <summary>
        /// パラメータの計算
        /// </summary>
        private int CalculateConvexParameter(HalfEdgeVertex vertex0, HalfEdgeVertex vertex1)
        {
            float ksf0;
            float ksf1;
            Vector3 direction;

            ksf0 = vertex0.MaxDerivaribe;
            if (Vector3.Dot(vertex0.MaxDirection, vertex1.MaxDirection) < 0)
            {
                ksf1 = -vertex1.MaxDerivaribe;
                direction = -vertex1.MaxDirection;
            }
            else
            {
                ksf1 = vertex1.MaxDerivaribe;
                direction = vertex1.MaxDirection;
            }

            if (ksf0 * ksf1 < 0)
            {
                var edge0 = vertex1 - vertex0;
                var edge1 = vertex0 - vertex1;
                if (ksf0 * Vector3.Dot(edge0, vertex0.MaxDirection) > 0 ||
                    ksf1 * Vector3.Dot(edge1, direction) > 0)
                {
                    return 1;
                }
            }

            return 0;
        }

        private int CalculateConcaveParameter(HalfEdgeVertex vertex0, HalfEdgeVertex vertex1)
        {
            float ksf0;
            float ksf1;
            Vector3 direction;

            ksf0 = vertex0.MinDerivaribe;

            // 向きを揃える
            if (Vector3.Dot(vertex0.MinDirection, vertex1.MinDirection) < 0)
            {
                ksf1 = -vertex1.MinDerivaribe;
                direction = -vertex1.MinDirection;
            }
            else
            {
                ksf1 = vertex1.MinDerivaribe;
                direction = vertex1.MinDirection;
            }

            // 同じ向きで曲率の正負が異なるものが欲しい。
            if (ksf0 * ksf1 < 0)
            {
                var edge0 = vertex1 - vertex0;
                var edge1 = vertex0 - vertex1;

                if (ksf0 * Vector3.Dot(edge0, vertex0.MinDirection) < 0 ||
                    ksf1 * Vector3.Dot(edge1, direction) < 0)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
