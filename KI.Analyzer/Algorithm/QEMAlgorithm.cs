#define BINARYTREE

using System;
using System.IO;
using System.Linq;
using KI.Analyzer.Paramter;
using KI.Foundation.Tree;
using KI.Mathmatics;
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
        public class Parameter : IComparable<Parameter>, IVertexColorParameter
        {
            /// <summary>
            /// 誤差のバッキングフィールド
            /// </summary>
            private float cost;

            /// <summary>
            /// 保持オブジェクト
            /// </summary>
            public object Owner;

            /// <summary>
            /// Q行列
            /// </summary>
            public Matrix4 Quadric { get; set; }

            /// <summary>
            /// 誤差
            /// </summary>
            public float Cost
            {
                get
                {
                    return cost;
                }

                set
                {
                    if (Math.Abs(value) < Calculator.THRESHOLD05)
                    {
                        cost = 0;
                    }

                    cost = value;
                }
            }

            public float Value
            {
                get
                {
                    return cost;
                }
            }

            /// <summary>
            /// 比較関数
            /// </summary>
            /// <param name="obj">Parameterクラス</param>
            /// <returns></returns>
            public int CompareTo(Parameter other)
            {
                if (this.Cost == other.Cost)
                {
                    if (Owner == other.Owner)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (this.Cost < other.Cost)
                {
                    return -1;
                }
                else // if (this.Cost > other.Cost)
                {
                    return 1;
                }
            }

            public override string ToString()
            {
                return Cost + " : " + Owner.ToString();
            }
        }

        /// <summary>
        /// バイナリツリー
        /// </summary>
        private BinaryTree<Parameter> binaryTree;

        /// <summary>
        /// ハーフエッジ
        /// </summary>
        private HalfEdgeDS halfEdgeDS = null;

        /// <summary>
        /// 削減する頂点数
        /// </summary>
        private int deleteVertexNum;

        private StreamWriter writer;

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
#if BINARYTREE
            writer = new StreamWriter("BinaryDebug.txt");
#else
            writer = new StreamWriter("LinearDebug.txt");
#endif
            Calculate();
            writer.Close();
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

                    matrix *= 0.01f;
                }

                var cost = CalculateCost(vertex.Position, matrix);

                vertex.TmpParameter = new Parameter() { Owner = vertex, Quadric = matrix, Cost = cost };
            }
        }

        /// <summary>
        /// コストの算出
        /// </summary>
        /// <param name="vector">ベクトル</param>
        /// <param name="matrix">マトリクス</param>
        /// <returns>コスト</returns>
        private float CalculateCost(Vector3 vector, Matrix4 matrix)
        {
            Vector4 result = Vector4.Zero;
            Vector4 vec = new Vector4(vector, 1);
            result.X = vec.X * matrix.Row0.X + vec.Y * matrix.Row1.X + vec.Z * matrix.Row2.X + vec.W * matrix.Row3.X;
            result.Y = vec.X * matrix.Row0.Y + vec.Y * matrix.Row1.Y + vec.Z * matrix.Row2.Y + vec.W * matrix.Row3.Y;
            result.Z = vec.X * matrix.Row0.Z + vec.Y * matrix.Row1.Z + vec.Z * matrix.Row2.Z + vec.W * matrix.Row3.Z;
            result.W = vec.X * matrix.Row0.W + vec.Y * matrix.Row1.W + vec.Z * matrix.Row2.W + vec.W * matrix.Row3.W;

            return Vector4.Dot(result, vec);
        }

        /// <summary>
        /// コストの算出
        /// </summary>
        /// <param name="edge">ハーフエッジ</param>
        /// <returns>コスト</returns>
        private float CalculateEdgeCost(HalfEdge edge)
        {
            return
                CalculateCost(edge.Start.Position, ((Parameter)edge.Start.TmpParameter).Quadric) +
                CalculateCost(edge.End.Position, ((Parameter)edge.End.TmpParameter).Quadric);
        }

#if BINARYTREE

        /// <summary>
        /// 計算
        /// </summary>
        private void Calculate()
        {
            foreach (var edge in halfEdgeDS.HalfEdges)
            {
                var cost = CalculateEdgeCost(edge);
                edge.TmpParameter = new Parameter() { Owner = edge, Cost = cost };
            }

            binaryTree = new BinaryTree<Parameter>(halfEdgeDS.HalfEdges.Select(p => p.TmpParameter).Cast<Parameter>());

            halfEdgeDS.Editor.StartEdit();
            for (int i = 0; i < deleteVertexNum; i++)
            {
                var minCostEdge = binaryTree.Min.Owner as HalfEdge;
                while (minCostEdge.DeleteFlag)
                {
                    // 消したものはバイナリツリーから削除
                    binaryTree.Remove((Parameter)minCostEdge.TmpParameter);
                    minCostEdge = binaryTree.Min.Owner as HalfEdge;
                    continue;
                }

                var quadric = ((Parameter)minCostEdge.Start.TmpParameter).Quadric + ((Parameter)minCostEdge.End.TmpParameter).Quadric;

                //var invert = quadric.Inverted();
                if (minCostEdge.Start.Index > minCostEdge.End.Index)
                {
                    var str = string.Format("Delete Index {0},{1} : Cost {2},{3} : Edge index{4}",
                    minCostEdge.Start.Index,
                    minCostEdge.End.Index,
                    ((Parameter)minCostEdge.Start.TmpParameter).Cost,
                    ((Parameter)minCostEdge.End.TmpParameter).Cost,
                    minCostEdge.Index);
                    writer.WriteLine(str);
                }
                else
                {
                    var str = string.Format("Delete Index {0},{1} : Cost {2},{3} : Edge index{4}",
                    minCostEdge.End.Index,
                    minCostEdge.Start.Index,
                    ((Parameter)minCostEdge.End.TmpParameter).Cost,
                    ((Parameter)minCostEdge.Start.TmpParameter).Cost,
                    minCostEdge.Index);
                    writer.WriteLine(str);
                }

                var vertex = halfEdgeDS.Editor.EdgeCollapse(minCostEdge, (minCostEdge.Start + minCostEdge.End) / 2/* new Vector3(invert.Row0.W, invert.Row1.W, invert.Row2.W)*/);

                if (vertex != null)
                {
                    // 消したものはバイナリツリーから削除
                    binaryTree.Remove((Parameter)minCostEdge.TmpParameter);
                    UpdateQuadric(vertex, quadric);
                }
                else
                {
                    // 消せなかったら消せたことに
                    UpdateQuadric(minCostEdge.Start, Matrix4.Identity * 100);
                    UpdateQuadric(minCostEdge.End, Matrix4.Identity * 100);
                }
            }

            halfEdgeDS.Editor.FinEdit();
        }

        /// <summary>
        /// 頂点の行列を更新します。
        /// </summary>
        /// <param name="vertex">頂点</param>
        /// <param name="quadric">誤差行列</param>
        private void UpdateQuadric(HalfEdgeVertex vertex, Matrix4 quadric)
        {
            ((Parameter)vertex.TmpParameter).Quadric = quadric;

            foreach (var edge in vertex.AroundEdge)
            {
                if (edge.DeleteFlag)
                {
                    continue;
                }

                binaryTree.Remove(edge.TmpParameter as Parameter);
                ((Parameter)edge.TmpParameter).Cost = CalculateEdgeCost(edge);
                binaryTree.Insert(edge.TmpParameter as Parameter);

                binaryTree.Remove(edge.Opposite.TmpParameter as Parameter);
                ((Parameter)edge.Opposite.TmpParameter).Cost = CalculateEdgeCost(edge.Opposite);
                binaryTree.Insert(edge.Opposite.TmpParameter as Parameter);
            }
        }
#endif

#if !BINARYTREE
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
                    if (edge.DeleteFlag)
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

                //var invert = quadric.Inverted();

                if (minCostEdge.Start.Index > minCostEdge.End.Index)
                {
                    var str = string.Format("Delete Index {0},{1} : Cost {2},{3} : Edge index{4}",
                    minCostEdge.Start.Index,
                    minCostEdge.End.Index,
                    ((Parameter)minCostEdge.Start.TmpParameter).Cost,
                    ((Parameter)minCostEdge.End.TmpParameter).Cost,
                    minCostEdge.Index);
                    writer.WriteLine(str);
                }
                else
                {
                    var str = string.Format("Delete Index {0},{1} : Cost {2},{3} : Edge index{4}",
                    minCostEdge.End.Index,
                    minCostEdge.Start.Index,
                    ((Parameter)minCostEdge.End.TmpParameter).Cost,
                    ((Parameter)minCostEdge.Start.TmpParameter).Cost,
                    minCostEdge.Index);
                    writer.WriteLine(str);
                }

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
#endif
    }
}