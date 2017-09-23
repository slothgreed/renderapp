﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    public class AdaptiveMeshAlgorithm
    {
        /// <summary>
        /// 許容誤差
        /// </summary>
        private float toleranceError = 0;

        /// <summary>
        /// ハーフエッジ
        /// </summary>
        private HalfEdgeDS halfEdgeDS = null;

        /// <summary>
        /// 解格子適合メッシュの作成
        /// </summary>
        /// <param name="halfEdge">ハーフエッジ形状</param>
        /// <param name="parameter">許容誤差</param>
        public AdaptiveMeshAlgorithm(HalfEdgeDS halfEdge, float tolerance)
        {
            halfEdgeDS = halfEdge;
            toleranceError = tolerance;
            Calculate();
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        private void Calculate()
        {
            halfEdgeDS.Editor.StartEdit();
            foreach (var edge in halfEdgeDS.HalfEdges)
            {
                if (edge.DeleteFlag)
                {
                    continue;
                }

                var sizingField = SizingField(edge);

                if (edge.Length < 4 * sizingField / 3)
                {
                    halfEdgeDS.Editor.EdgeCollapse(edge);
                }
            }

            var edgeArray = halfEdgeDS.HalfEdges.ToArray();
            for (int i = 0; i < edgeArray.Length; i++)
            {
                var edge = edgeArray[i];
                if (edge.DeleteFlag)
                {
                    continue;
                }

                var sizingField = SizingField(edge);
                if (edge.Length > 4 + sizingField / 5)
                {
                    halfEdgeDS.Editor.EdgeSplit(edge);
                }
            }
            //foreach (var edge in halfEdgeDS.HalfEdges)
            //{
            //    if (edge.DeleteFlag)
            //    {
            //        continue;
            //    }

            //    var sizingField = SizingField(edge);
            //    if (edge.Length < 4 + sizingField / 5)
            //    {
            //        halfEdgeDS.Editor.EdgeSplit(edge);
            //    }
            //}

            var vertexArray = halfEdgeDS.HalfEdgeVertexs.ToArray();
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                if (vertex.DeleteFlag)
                {
                    continue;
                }

                int num = vertex.AroundEdge.Count();
                if (num > 6)
                {
                    halfEdgeDS.Editor.EdgeFlips(vertex.AroundEdge.First());
                }
            }

            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                Vector3 sumGravity = Vector3.Zero;

                //分子
                Vector3 numerator = Vector3.Zero;

                //分母
                float denominator = 0;

                foreach (var mesh in vertex.AroundMesh)
                {
                    float meshSizingField = SizingField(mesh);
                    numerator += mesh.Area * meshSizingField * mesh.Gravity;
                    denominator +=  mesh.Area * meshSizingField;
                }

                var result = numerator /= denominator;

                vertex.Position = result;// result + Vector3.Dot(vertex.Normal, vertex.Normal) * (vertex.Position - result);
            }

            halfEdgeDS.Editor.FinEdit();
        }

        /// <summary>
        /// サイズフィールド
        /// </summary>
        /// <param name="halfEdge">ハーフエッジ</param>
        private float SizingField(HalfEdge halfEdge)
        {
            float startSizingField = SizingField(halfEdge.Start);
            float finSizingField = SizingField(halfEdge.End);

            return Math.Min(startSizingField, finSizingField);
        }

        /// <summary>
        /// サイズフィールド
        /// </summary>
        /// <param name="halfEdge">頂点</param>
        private float SizingField(HalfEdgeVertex halfEdgeVertex)
        {
            var sizingField = (6 * toleranceError / halfEdgeVertex.MaxCurvature) - 3 * toleranceError * toleranceError;

            if (sizingField < 0)
            {
                return 0;
            }

            return (float)Math.Sqrt(sizingField);
        }

        /// <summary>
        /// サイズフィールド
        /// </summary>
        /// <param name="halfEdge">面</param>
        private float SizingField(HalfEdgeMesh halfEdgeMesh)
        {
            float sizingField = 0;
            foreach (var vertex in halfEdgeMesh.AroundVertex)
            {
                sizingField += SizingField(vertex);
            }

            return sizingField / halfEdgeMesh.AroundVertex.Count();
        }
    }
}
