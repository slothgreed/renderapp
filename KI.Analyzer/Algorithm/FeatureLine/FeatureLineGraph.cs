using System.Collections.Generic;
using OpenTK;

namespace KI.Analyzer.Algorithm.FeatureLine
{
    /// <summary>
    /// 特徴稜線用のグラフ
    /// HalfEdgeに似ているが、ポリゴン用ではないのと反対エッジも追加しているので分けている。
    /// </summary>
    public class FeatureLineGraph
    {
        List<FeatureLine> featureList = new List<FeatureLine>();
        List<HalfEdge> edges = new List<HalfEdge>();
        List<HalfEdgeVertex> vertexs = new List<HalfEdgeVertex>();

        public List<HalfEdge>[] GetFeatureLines()
        {
            List<HalfEdge>[] featureLines = new List<HalfEdge>[featureList.Count];
            for (int i = 0; i < featureList.Count; i++)
            {
                featureLines[i] = featureList[i].Lines;
            }

            return featureLines;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FeatureLineGraph()
        {
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

        /// <summary>
        /// 頂点が存在するか(生成元エッジの情報をもとに)
        /// </summary>
        /// <param name="index1">頂点1</param>
        /// <param name="index2">頂点2</param>
        /// <returns>頂点</returns>
        public HalfEdgeVertex GetVertexByGenerateEdgeIndex(int index1, int index2)
        {
            for (int i = 0; i < vertexs.Count; i++)
            {
                var param = vertexs[i].TmpParameter as VertexParameter;

                if (param.EdgeIndex1 == index1 && param.EdgeIndex2 == index2)
                {
                    return vertexs[i];
                }

                if (param.EdgeIndex1 == index2 && param.EdgeIndex2 == index1)
                {
                    return vertexs[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 頂点の追加
        /// </summary>
        /// <param name="vertex">頂点</param>
        /// <param name="edgeIndex1">エッジ番号</param>
        /// <param name="edgeIndex2">エッジ番号</param>
        /// <param name="alpha">内分点の割合</param>
        /// <param name="beta">内分点の割合</param>
        public HalfEdgeVertex AddVertex(Vector3 position, int edgeIndex1, int edgeIndex2, float alpha, float beta)
        {
            var featureVertex = new HalfEdgeVertex(position, vertexs.Count);
            featureVertex.TmpParameter = new VertexParameter(edgeIndex1, edgeIndex2, alpha, beta);

            vertexs.Add(featureVertex);
            return featureVertex;
        }


        /// <summary>
        /// エッジの追加
        /// </summary>
        /// <param name="vertex0">頂点1</param>
        /// <param name="vertex1">頂点2</param>
        /// <returns>ID</returns>
        public int AddEdge(HalfEdgeVertex vertex0, HalfEdgeVertex vertex1)
        {
            HalfEdgeVertex start = vertexs[vertex0.Index];
            HalfEdgeVertex end = vertexs[vertex1.Index];

            HalfEdge edge = new HalfEdge(start, end, edges.Count);
            edges.Add(edge);

            // 反対エッジも追加
            HalfEdge edge2 = new HalfEdge(end, start, edges.Count);
            edges.Add(edge2);

            return 0;
        }

        /// <summary>
        /// 頂点パラメータ
        /// </summary>
        private class VertexParameter
        {
            public int EdgeIndex1 { get; private set; }
            public int EdgeIndex2 { get; private set; }
            public float Alpha { get; private set; }
            public float Beta { get; private set; }

            public VertexParameter(int edgeIndex1, int edgeIndex2, float alpha, float beta)
            {
                EdgeIndex1 = edgeIndex1;
                EdgeIndex2 = edgeIndex2;
                Alpha = alpha;
                Beta = beta;
            }
        }

        /// <summary>
        ///エッジの接続を行う
        /// </summary>
        public void Connect()
        {
            // すでに特徴稜線として繋がれたエッジを再計算しないためのフラグ
            //var isAddEdge = new bool[edges.Count];

            //for (int i = 0; i < isAddEdge.Length; i++)
            //{
            //    isAddEdge[i] = false;
            //}

            //for (int i = 0; i < edges.Count; i++)
            //{
            //    if (isAddEdge[i] == false)
            //    {
            //        var feature = new FeatureLine();
            //        feature.Lines.Add(edges[i]);
            //        RecursiveConnect(feature, edges[i]);

            //        foreach (var addEdge in feature.Lines)
            //        {
            //            isAddEdge[addEdge.Index] = true;
            //        }

            //        featureList.Add(feature);

            //    }
            //}

            var feature = new FeatureLine();
            foreach (var edge in edges)
            {
                feature.Lines.Add(edge);
            }
            featureList.Add(feature);
        }

        private void RecursiveConnect(FeatureLine featureLine, HalfEdge edge)
        {
            foreach (var endAroundEdge in edge.End.AroundEdge)
            {
                // 共有エッジの場合は追加するだけ
                if (endAroundEdge.End == edge.Start)
                {
                    if (featureLine.Lines.Contains(edge) == false)
                    {
                        featureLine.Lines.Add(edge);
                    }
                }
                else
                {
                    // 共有エッジでない繋がっているエッジは再帰的に探索していく
                    if (featureLine.Lines.Contains(edge) == false)
                    {
                        featureLine.Lines.Add(edge);
                        RecursiveConnect(featureLine, endAroundEdge);
                    }
                }
            }
        }

        private class FeatureLine
        {
            public List<HalfEdge> Lines = new List<HalfEdge>();
        }

    }
}
