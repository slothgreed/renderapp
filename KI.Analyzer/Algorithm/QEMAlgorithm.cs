using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// QEMアルゴリズム
    /// </summary>
    public class QEMAlgorithm
    {
        /// <summary>
        /// QEMパラメータ
        /// </summary>
        public class Parameter
        {
            /// <summary>
            /// Q行列
            /// </summary>
            public Matrix4 Quadric { get; set; }
            
            /// <summary>
            /// 誤差
            /// </summary>
            public float Cost { get; set; }
        }

        /// <summary>
        /// ハーフエッジ
        /// </summary>
        private HalfEdgeDS halfEdgeDS = null;

        /// <summary>
        /// 削減する頂点数
        /// </summary>
        private int deleteVertexNum;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="halfEdgeDS">ハーフエッジ形状</param>
        /// <param name="deleteVertexNum">削除する頂点数</param>
        public QEMAlgorithm(HalfEdgeDS halfEdge, int deleteVertexNum)
        {
            halfEdgeDS = halfEdge;
            this.deleteVertexNum = deleteVertexNum;
            Initialize();
            Calculate();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                Matrix4 matrix = Matrix4.Zero;

                foreach (var mesh in vertex.AroundMesh)
                {
                    float a = mesh.Plane.X;
                    float b = mesh.Plane.Y;
                    float c = mesh.Plane.Z;
                    float d = mesh.Plane.W;

                    matrix += new Matrix4(
                        a * a, a * b, a * c, a * d,
                        a * b, b * b, b * c, b * d,
                        a * c, b * c, c * c, c * d,
                        a * d, b * d, c * d, d * d);
                }

                var cost = CalculateCost(vertex.Position, matrix);

                vertex.TmpParameter = new Parameter() { Quadric = matrix, Cost = cost };
            }
        }

        /// <summary>
        /// コストの算出
        /// </summary>
        /// <param name="vector">ベクトル</param>
        /// <param name="matrix">マトリクス</param>
        /// <returns>ベクトル</returns>
        private float CalculateCost(Vector3 vector, Matrix4 matrix)
        {
            Vector4 result = Vector4.Zero;
            Vector4 vec = new Vector4(vector, 1);
            result.X = vec.X * matrix.Row0.X + vec.Y * matrix.Row1.X + vec.Z * matrix.Row2.X + vec.W * matrix.Row3.X;
            result.Y = vec.X * matrix.Row0.Y + vec.Y * matrix.Row1.Y + vec.Z * matrix.Row2.Y + vec.W * matrix.Row3.Y;
            result.Z = vec.X * matrix.Row0.Z + vec.Y * matrix.Row1.Z + vec.Z * matrix.Row2.Z + vec.W * matrix.Row3.Z;
            result.W = vec.X * matrix.Row0.W + vec.Y * matrix.Row1.W + vec.Z * matrix.Row2.W + vec.W * matrix.Row3.W;

            return Vector4.Dot(result,vec);
        }


        /// <summary>
        /// コストの算出
        /// </summary>
        /// <param name="vector">ベクトル</param>
        /// <param name="matrix">マトリクス</param>
        /// <returns>ベクトル</returns>
        private float CalculateEdgeCost(HalfEdge edge)
        {
            return 
                CalculateCost(edge.Start.Position, ((Parameter)edge.Start.TmpParameter).Quadric) +
                CalculateCost(edge.End.Position, ((Parameter)edge.Start.TmpParameter).Quadric);
        }


        /// <summary>
        /// 計算
        /// </summary>
        private void Calculate()
        {
            halfEdgeDS.Editor.StartEdit();
            for (int i = 0; i < deleteVertexNum; i++)
            {
                var minValue = float.MaxValue;
                HalfEdge minCostEdge = null;
                foreach (var edge in halfEdgeDS.HalfEdges)
                {
                    if(edge.DeleteFlag)
                    {
                        continue;
                    }

                    var cost = CalculateEdgeCost(edge);
                    if (cost < minValue)
                    {
                        minValue = cost;
                        minCostEdge = edge;
                    }
                }

                var quadric = ((Parameter)minCostEdge.Start.TmpParameter).Quadric + ((Parameter)minCostEdge.End.TmpParameter).Quadric;

                var invert = quadric.Inverted();

                var vertex = halfEdgeDS.Editor.EdgeCollapse(minCostEdge, (minCostEdge.Start + minCostEdge.End) / 2/* new Vector3(invert.Row0.W, invert.Row1.W, invert.Row2.W)*/);

                if (vertex != null)
                {
                    ((Parameter)vertex.TmpParameter).Quadric = quadric;
                }
                else
                {
                    ((Parameter)minCostEdge.Start.TmpParameter).Quadric = Matrix4.Identity * 100;
                    ((Parameter)minCostEdge.End.TmpParameter).Quadric = Matrix4.Identity * 100;
                }
            }

            halfEdgeDS.Editor.FinEdit();
        }
    }
}
