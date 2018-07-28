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
        private HalfEdgeDS halfEdgeDS;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="halfEdgeDS">データ構造</param>
        public FeatureLineAlgorithm(HalfEdgeDS structure)
        {
            halfEdgeDS = structure;
        }

        /// <summary>
        /// 計算
        /// </summary>
        public void Calculate()
        {
            var ridges = new Feature();
            var ridgeGraph = new FeatureLineGraph(halfEdgeDS.Vertexs.Count);
            var ravines = new Feature();
            var ravineGraph = new FeatureLineGraph(halfEdgeDS.Vertexs.Count);

            float ksf0;
            float ksf1;
            float ksf2;

            Vector3 bc5;
            Vector3 bc6;
            Vector3 bc7;

            foreach (var mesh in halfEdgeDS.HalfEdgeMeshs)
            {
                bool convex = true;
                bool concave = true;
                int edge0_1 = 0;
                int edge1_2 = 0;
                int edge2_0 = 0;

                foreach (var vertex in mesh.AroundVertex)
                {
                    if (vertex.MaxCurvature < Math.Abs(vertex.MinCurvature))
                    {
                        convex = false;
                    }

                    if (-Math.Abs(vertex.MaxCurvature) < vertex.MinCurvature)
                    {
                        concave = false;
                    }
                }

                var vertexs = mesh.AroundVertex.ToArray();
                var vertex0 = vertexs[0];
                var vertex1 = vertexs[1];
                var vertex2 = vertexs[2];
                int checkID1 = 0;
                int checkID2 = 0;
                if (convex)
                {
                    edge0_1 = CalculateConvexParameter(vertex0.MaxDerivaribe, vertex0, vertex1);
                    edge1_2 = CalculateConvexParameter(vertex1.MaxDerivaribe, vertex1, vertex2);
                    edge2_0 = CalculateConvexParameter(vertex2.MaxDerivaribe, vertex2, vertex0);
                    /*これより前でksf0 が反転している場合がある。元コードのバグ？*/
                    if (edge0_1 == 1 && edge1_2 == 1)
                    {
                        checkID1 = AddConvexPoint(vertex0, vertex1, ridges, ridgeGraph);
                        checkID2 = AddConvexPoint(vertex1, vertex2, ridges, ridgeGraph);
                    }
                    else if (edge0_1 == 1 && edge1_2 == 1)
                    {
                        checkID1 = AddConvexPoint(vertex0, vertex1, ridges, ridgeGraph);
                        checkID2 = AddConvexPoint(vertex1, vertex2, ridges, ridgeGraph);

                    }
                    else if (edge1_2 == 1 && edge2_0 == 1)
                    {
                        checkID1 = AddConvexPoint(vertex1, vertex2, ridges, ridgeGraph);
                        checkID2 = AddConvexPoint(vertex2, vertex0, ridges, ridgeGraph);
                    }

                    ridges.AddEdge(checkID1, checkID2, mesh.Index);
                }

                if (concave)
                {
                    edge0_1 = CalculateConcaveParameter(vertex0.MinDerivaribe, vertex0, vertex1);
                    edge1_2 = CalculateConcaveParameter(vertex1.MinDerivaribe, vertex1, vertex2);
                    edge2_0 = CalculateConcaveParameter(vertex2.MinDerivaribe, vertex2, vertex0);

                    if (edge0_1 == 1 && edge1_2 == 1)
                    {
                        checkID1 = AddConcavePoint(vertex0, vertex1, ravines, ravineGraph);
                        checkID2 = AddConcavePoint(vertex1, vertex2, ravines, ravineGraph);
                    }
                    else if (edge0_1 == 1 && edge1_2 == 1)
                    {
                        checkID1 = AddConcavePoint(vertex0, vertex1, ravines, ravineGraph);
                        checkID2 = AddConcavePoint(vertex1, vertex2, ravines, ravineGraph);

                    }
                    else if (edge1_2 == 1 && edge2_0 == 1)
                    {
                        checkID1 = AddConcavePoint(vertex1, vertex2, ravines, ravineGraph);
                        checkID2 = AddConcavePoint(vertex2, vertex0, ravines, ravineGraph);
                    }
                }
            }
        }

        /// <summary>
        /// 頂点の追加
        /// </summary>
        /// <returns></returns>
        private int AddConvexPoint(HalfEdgeVertex vertex0, HalfEdgeVertex vertex1, Feature ridges, FeatureLineGraph ridgeGraph)
        {
            int checkID = 0;
            // あればそれを
            checkID = ridgeGraph.GetEdgeIndex(vertex0.Index, vertex1.Index);
            if (checkID == -1)
            {
                //なければ作る
                var alpha = Math.Abs(vertex0.MaxDerivaribe);
                var beta = Math.Abs(vertex1.MaxDerivaribe);
                var position = Interaction.Inter(alpha, beta, vertex0.Position, vertex1.Position);
                checkID = ridges.AddPoint(ridgeGraph.AddEdge(vertex0, vertex1), position, vertex0.Index, vertex1.Index, alpha, beta);
            }

            return checkID;
        }

        /// <summary>
        /// 頂点の追加
        /// </summary>
        /// <returns></returns>
        private int AddConcavePoint(HalfEdgeVertex vertex0, HalfEdgeVertex vertex1, Feature ridges, FeatureLineGraph ridgeGraph)
        {
            int checkID = 0;
            // あればそれを
            checkID = ridgeGraph.GetEdgeIndex(vertex0.Index, vertex1.Index);
            if (checkID == -1)
            {
                //なければ作る
                var alpha = Math.Abs(vertex0.MinDerivaribe);
                var beta = Math.Abs(vertex1.MinDerivaribe);
                var position = Interaction.Inter(alpha, beta, vertex0.Position, vertex1.Position);
                checkID = ridges.AddPoint(ridgeGraph.AddEdge(vertex0, vertex1), position, vertex0.Index, vertex1.Index, alpha, beta);
            }

            return checkID;
        }

        /// <summary>
        /// パラメータの計算
        /// </summary>
        private int CalculateConvexParameter(float derivarible, HalfEdgeVertex vertex0, HalfEdgeVertex vertex1)
        {
            float ksf0;
            float ksf1;
            Vector3 direction;

            ksf0 = vertex0.MaxDerivaribe;
            if (Vector3.Dot(vertex0.MaxDirection, vertex1.MaxDirection) < Calculator.THRESHOLD05)
            {
                ksf1 = -vertex1.MaxDerivaribe;
                direction = -vertex1.MaxDirection;
            }
            else
            {
                ksf1 = vertex1.MaxDerivaribe;
                direction = vertex1.MaxDirection;
            }

            if (ksf0 * ksf1 < Calculator.THRESHOLD05)
            {
                var edge0 = vertex1 - vertex0;
                var edge1 = vertex0 - vertex1;
                if (ksf0 * Vector3.Dot(edge0, vertex0.MaxDirection) > Calculator.THRESHOLD05 ||
                    ksf1 * Vector3.Dot(edge1, direction) > Calculator.THRESHOLD05)
                {
                    return 1;
                }
            }

            return 0;
        }

        private int CalculateConcaveParameter(float derivarible, HalfEdgeVertex vertex0, HalfEdgeVertex vertex1)
        {
            float ksf0;
            float ksf1;
            Vector3 direction;

            ksf0 = vertex0.MinDerivaribe;

            // 向きを揃える
            if (Vector3.Dot(vertex0.MinDirection, vertex1.MinDirection) < Calculator.THRESHOLD05)
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
            if (ksf0 * ksf1 < Calculator.THRESHOLD05)
            {
                var edge0 = vertex1 - vertex0;
                var edge1 = vertex0 - vertex1;

                if (ksf0 * Vector3.Dot(edge0, vertex0.MinDirection) > Calculator.THRESHOLD05 ||
                    ksf1 * Vector3.Dot(edge1, direction) > Calculator.THRESHOLD05)
                {
                    return 1;
                }
            }

            return 0;
        }

        private class Feature
        {
            public Feature()
            {

            }

            public void AddPoint()
            {

            }

            public int AddPoint(int v, Vector3 position, int index1, int index2, float alpha, float beta)
            {
                throw new NotImplementedException();
            }

            internal void AddEdge(int checkID1, int checkID2, int index)
            {
                throw new NotImplementedException();
            }
        }
    }
}
