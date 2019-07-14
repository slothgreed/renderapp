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
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                vertex.TmpParameter = SubdivInfo.Original;
            }

            List<HalfEdge> halfEdges = halfEdgeDS.HalfEdges.ToList();
            int edgeNum = halfEdges.Count;

            halfEdgeDS.Editor.StartEdit();
            // 中点を加えていくので、エッジ数が増えていく
            for (int i = 0; i < edgeNum; i++)
            {
                // はじめのエッジが削除された時
                // 反対のエッジは削除されている。
                if (halfEdges[i].DeleteFlag == false)
                {
                    HalfEdge[] split;
                    HalfEdge[] create;
                    halfEdgeDS.Editor.EdgeSplit(halfEdges[i], out split, out create);

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

            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                vertex.TmpParameter = null;
            }
        }
    }
}
