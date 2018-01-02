using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// 測地距離の算出
    /// </summary>
    public class GeodesicDistanceAlgorithm
    {
        /// <summary>
        /// ハーフエッジデータ構造
        /// </summary>
        private HalfEdgeDS halfEdgeDS;

        /// <summary>
        /// 選択頂点の要素番号
        /// </summary>
        private DenseVector selectVertex;

        /// <summary>
        /// ラプラシアンのコレスキー
        /// </summary>
        private Cholesky<double> laplaceCholesky;

        /// <summary>
        /// ヒートマップ
        /// </summary>
        private Cholesky<double> heatFlowCholesky;

        /// <summary>
        /// 拡散係数
        /// </summary>
        private float time;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="halfEdgeDS">ハーフエッジデータ構造</param>
        /// <param name="selectedPoint">選択頂点の要素番号</param>
        public GeodesicDistanceAlgorithm(HalfEdgeDS half)
        {
            halfEdgeDS = half;

            var sumLength = 0.0f;
            foreach (var edge in half.HalfEdges)
            {
                sumLength += edge.Length;
            }

            time = sumLength * sumLength;
            
            Initialize();
        }

        /// <summary>
        /// 初期化処理・ラプラシアン・熱量計算
        /// </summary>
        private void Initialize()
        {
            var LaplaceMatrix = new DenseMatrix(halfEdgeDS.Vertexs.Count, halfEdgeDS.Vertexs.Count);
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                float sum = 0;
                foreach (var edge in vertex.AroundEdge)
                {
                    var alphaCot = HalfEdgeDSUtility.Cot(edge.Next);
                    var betaCot = HalfEdgeDSUtility.Cot(edge.Opposite.Next);

                    float laplace = (alphaCot + betaCot) / 2;

                    LaplaceMatrix[vertex.Index, edge.End.Index] = -laplace;
                    sum += laplace;
                }

                LaplaceMatrix[vertex.Index, vertex.Index] = sum;
            }

            laplaceCholesky = LaplaceMatrix.Cholesky();

            var areaMatrix = new DenseMatrix(halfEdgeDS.Vertexs.Count, halfEdgeDS.Vertexs.Count);

            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                areaMatrix[vertex.Index, vertex.Index] = vertex.Voronoi;
            }

            var HeatMatrix = areaMatrix.Add(LaplaceMatrix.Multiply(time) as DenseMatrix) as DenseMatrix;
            heatFlowCholesky = HeatMatrix.Cholesky();
        }

        /// <summary>
        /// 距離場を計算
        /// </summary>
        public float[] Compute()
        {
            var u = laplaceCholesky.Solve(selectVertex);

            var x = ComputeVectorField(u as DenseVector);
            var div = ComputeDivergence(x);

            var distanceField = heatFlowCholesky.Solve(div.Negate());

            NormalizeDistance(distanceField as DenseMatrix);

            var distance = new float[halfEdgeDS.Vertexs.Count];

            for (int i = 0; i < distance.Length; i++)
            {
                distance[i] = (float)distanceField[i, 0];
            }

            return distance;
        }
        
        /// <summary>
        /// 頂点の選択
        /// </summary>
        /// <param name="index"></param>
        public void SelectPoint(int index)
        {
            selectVertex[index] = 1;
        }

        /// <summary>
        /// 頂点の選択解除
        /// </summary>
        /// <param name="index">頂点インデックスの番号</param>
        public void UnSelectPoint(int index)
        {
            selectVertex[index] = 0;
        }

        /// <summary>
        /// 選択頂点のリセット
        /// </summary>
        public void ResetSelect()
        {
            selectVertex = new DenseVector(halfEdgeDS.Vertexs.Count);
            for (int i = 0; i < halfEdgeDS.Vertexs.Count; i++)
            {
                selectVertex[i] = 0;
            }
        }

        /// <summary>
        /// ベクトル場の計算
        /// </summary>
        /// <param name="u"></param>
        /// <returns>ベクトル場</returns>
        private Vector3[] ComputeVectorField(DenseVector u)
        {
            var vectorField = new Vector3[halfEdgeDS.Meshs.Count];
            foreach (var mesh in halfEdgeDS.HalfEdgeMeshs)
            {
                var grad = new Vector3();
                foreach (var edge in mesh.AroundEdge)
                {
                    var ui = (float)u[edge.Start.Index];
                    var vector = edge.End - edge.Start;

                    grad += (ui * Vector3.Cross(mesh.Normal, vector));
                }

                grad /= 2 * mesh.Area;
                grad.Normalize();
                vectorField[mesh.Index] = grad;
            }

            return vectorField;
        }

        /// <summary>
        /// 熱量拡散の計算
        /// </summary>
        /// <param name="vectorField">ベクトル場</param>
        /// <returns></returns>
        private DenseMatrix ComputeDivergence(Vector3[] vectorField)
        {
            var divergence = DenseMatrix.Create(halfEdgeDS.Vertexs.Count, 1, 0);

            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                var sum = 0.0f;
                foreach (var edge in vertex.AroundEdge)
                {
                    var vector = vectorField[edge.Mesh.Index];
                    var theta1 = HalfEdgeDSUtility.Cot(edge.Next);
                    var theta2 = HalfEdgeDSUtility.Cot(edge.Before);
                    var e1 = -edge.Before.Vector;
                    var e2 = edge.Vector;
                    sum += (theta1 * Vector3.Dot(e1, vector) + theta2 * Vector3.Dot(e2, vector));
                }

                divergence[vertex.Index, 0] = 0.5 * sum;
            }

            return divergence;
        }

        /// <summary>
        /// 距離場の正規化
        /// </summary>
        /// <param name="distance">距離場</param>
        private void NormalizeDistance(DenseMatrix distance)
        {
            var min = distance[0, 0];
            for (int i = 0; i < distance.RowCount; i++)
            {
                min = Math.Min(min, distance[i, 0]);
            }

            for (int i = 0; i < distance.RowCount; i++)
            {
                distance[i, 0] -= min;
            }
        }
    }
}
