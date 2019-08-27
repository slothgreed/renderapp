using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// Loop 細分割曲面
    /// </summary>
    public class LoopSubdivisionAlgorithm
    {
        /// <summary>
        /// Loop 細分割曲面
        /// </summary>
        public LoopSubdivisionAlgorithm()
        {
        }

        /// <summary>
        /// TmpParameter用
        /// </summary>
        public enum SubdivInfo
        {
            /// <summary>
            /// 元形状
            /// </summary>
            Original,

            /// <summary>
            /// 作成されたもの
            /// </summary>
            Create
        }


        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="halfEdgeDS">ハーフエッジデータ構造</param>
        /// <param name="loopNum">繰り返し回数</param>
        public void Calculate(HalfEdgeDS halfEdgeDS, int loopNum)
        {
            for(int i = 0; i < loopNum; i++)
            {
                CalculateInternal(halfEdgeDS);
            }
        }

        private void CalculateInternal(HalfEdgeDS halfEdgeDS)
        {
            // 最初にエッジ全部の中点に点を追加する。
            // その後、追加した線のうち、始点がオリジナルのもの、終点が新しいものの場合、フリップすると
            // トライフォース型に割れる。
            // TmpParameter はオリジナルかどうか
            Vector3[] movePositions = new Vector3[halfEdgeDS.HalfEdgeVertexs.Count()];
            int n = 0;
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                vertex.TmpParameter = SubdivInfo.Original;
                movePositions[n] = GetMovePosition(vertex);
                n++;
            }

            List<HalfEdge> halfEdges = halfEdgeDS.HalfEdges.ToList();
            int edgeNum = halfEdges.Count;

            halfEdgeDS.Editor.StartEdit();

            // EdgeSplitで位相情報が変わるのであらかじめ計算しておく。
            Vector3[] insertPositions = new Vector3[edgeNum];
            for (int i = 0; i < edgeNum; i++)
            {
                insertPositions[i] = GetInsertPosition(halfEdges[i]);
            }

            // 中点を加えていくので、エッジ数が増えていく
            for (int i = 0; i < edgeNum; i++)
            {
                // はじめのエッジが削除された時
                // 反対のエッジは削除されている。
                if (halfEdges[i].DeleteFlag == false)
                {
                    HalfEdge[] split;
                    HalfEdge[] create;
                    halfEdgeDS.Editor.EdgeSplit(halfEdges[i], insertPositions[i], out split, out create);

                    for (int j = 0; j < create.Length; j++)
                    {
                        create[j].TmpParameter = SubdivInfo.Create;
                    }
                }
            }

            // 追加されているため、リストの作り直し
            halfEdges = halfEdgeDS.HalfEdges.ToList();
            int edgeCount = halfEdges.Count;
            // 追加したエッジのみ捜査
            for (int i = 0; i < edgeCount; i++)
            {
                HalfEdge edge = halfEdges[i];
                if (edge.DeleteFlag == true)
                {
                    continue;
                }

                if (edge.TmpParameter is SubdivInfo)
                {
                    if (edge.Start.TmpParameter == null &&
                        edge.End.TmpParameter is SubdivInfo)
                    {
                        halfEdgeDS.Editor.EdgeFlips(edge);
                    }

                    edge.TmpParameter = null;
                }
            }

            halfEdgeDS.Editor.EndEdit();

            // 事前に計算したオリジナルの頂点位置を再計算する。
            // インデックス情報がオリジナルの者と同様なことが前提条件
            n = 0;
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                if (vertex.TmpParameter != null)
                {
                    vertex.Position = movePositions[n];
                    n++;
                }

                vertex.TmpParameter = null;
            }
        }

        /// <summary>
        /// 元の頂点の移動位置の取得
        /// </summary>
        /// <param name="vertex">頂点</param>
        /// <returns>移動位置</returns>
        private Vector3 GetMovePosition(HalfEdgeVertex vertex)
        {
            Vector3 aroundSum = Vector3.Zero;
            int aroundNum = vertex.AroundEdge.Count();
            float beta = 0;

            if (aroundNum == 3)
            {
                beta = 3.0f / 16.0f;
            }
            else
            {
                beta = 3.0f / (8.0f * aroundNum);
            }

            aroundSum = vertex.Position * (1 - aroundNum * beta);
            foreach (var loop in vertex.AroundEdge)
            {
                aroundSum += loop.End.Position * beta;  
            }

            return aroundSum;
        }

        /// <summary>
        /// LoopSubdivの頂点位置を計算
        /// </summary>
        /// <param name="edge">エッジ</param>
        /// <returns>挿入位置</returns>
        private Vector3 GetInsertPosition(HalfEdge edge)
        {
            Vector3 VA = edge.Start.Position * 3.0f / 8.0f;
            Vector3 VB = edge.End.Position * 3.0f / 8.0f;
            Vector3 VC = edge.Next.End.Position / 8.0f;
            Vector3 VD = edge.Opposite.Next.End.Position / 8.0f;
            return VA + VB + VC + VD;
        }
    }
}
