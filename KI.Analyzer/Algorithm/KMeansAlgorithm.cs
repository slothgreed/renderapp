using System;
using System.Collections.Generic;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// K-Meansクラスタリングアルゴリズム
    /// </summary>
    public class KMeansAlgorithm
    {
        /// <summary>
        /// クラスタ数
        /// </summary>
        private int clusterNum = 0;

        /// <summary>
        /// ハーフエッジデータ構造
        /// </summary>
        private HalfEdgeDS halfedgeDS = null;

        /// <summary>
        /// 繰り返し回数
        /// </summary>
        private int iterateNum = 0;

        /// <summary>
        /// 最終的にクラスタに含まれる頂点
        /// </summary>
        public List<Vertex>[] Cluster
        {
            get;
            private set;
        }

        /// <summary>
        /// Constractor.
        /// </summary>
        /// <param name="halfEdge">ハーフエッジ</param>
        /// <param name="clusterNum">クラスタ数</param>
        /// <param name="iterate">繰り返し回数</param>
        public KMeansAlgorithm(HalfEdgeDS halfEdge, int cluster, int iterate)
        {
            halfedgeDS = halfEdge;
            clusterNum = cluster;
            iterateNum = iterate;
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        public void Calculate()
        {
            var seeds = new Vector3[clusterNum];

            Random rand = new Random();
            for (int i = 0; i < seeds.Length; i++)
            {
                var index = rand.Next(halfedgeDS.Vertexs.Count - 1);
                seeds[i] = halfedgeDS.Vertexs[index].Position;
            }

            for (int i = 0; i < iterateNum; i++)
            {
                CalculateCore(seeds);
            }

            Cluster = new List<Vertex>[clusterNum];
            for (int i = 0; i < clusterNum; i++)
            {
                Cluster[i] = new List<Vertex>();
            }

            foreach (var vertex in halfedgeDS.HalfEdgeVertexs)
            {
                Cluster[(int)vertex.TmpParameter].Add(vertex);
            }
        }

        /// <summary>
        /// アルゴリズムの計算コア部分
        /// </summary>
        /// <param name="seeds">シード</param>
        /// <returns>変更があったか</returns>
        private void CalculateCore(Vector3[] seeds)
        {
            // 頂点に最も近いシードを設定
            float minLength;
            foreach (var vertex in halfedgeDS.HalfEdgeVertexs)
            {
                minLength = float.MaxValue;
                for (int i = 0; i < seeds.Length; i++)
                {
                    var length = (seeds[i] - vertex.Position).Length;
                    if (length < minLength)
                    {
                        minLength = length;
                        vertex.TmpParameter = i;
                    }
                }
            }

            // シードの初期化
            var vertexNum = new int[seeds.Length];
            for (int i = 0; i < seeds.Length; i++)
            {
                seeds[i] = Vector3.Zero;
                vertexNum[i] = 0;
            }

            // シードの再計算
            foreach (var vertex in halfedgeDS.HalfEdgeVertexs)
            {
                seeds[(int)vertex.TmpParameter] += vertex.Position;
                vertexNum[(int)vertex.TmpParameter]++;
            }

            // 各シードの平均値算出
            for (int i = 0; i < seeds.Length; i++)
            {
                seeds[i] /= vertexNum[i];
            }
        }
    }
}
