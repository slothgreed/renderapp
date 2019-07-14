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
        /// 計算処理
        /// </summary>
        /// <param name="halfEdgeDS">ハーフエッジデータ構造</param>
        /// <param name="loopNum">繰り返し回数</param>
        public void Calculate(HalfEdgeDS halfEdgeDS, int loopNum)
        {
            // 最初にエッジ全部の中点に点を追加する。
            // その後、あるエッジの次のエッジのネクスト、反対のエッジのネクストが追加点の場合フリップすると
            // トライフォース型に割れる。

            foreach(var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                vertex.TmpParameter = new LoopSubdivParameter(true);
            }

            List<HalfEdge> halfEdges = halfEdgeDS.HalfEdges.ToList();
            int edgeNum = halfEdges.Count;

            halfEdgeDS.Editor.StartEdit();
            // 中点を加えていくので、エッジ数が増えていく
            for(int i = 0; i < edgeNum; i ++)
            {
                // はじめのエッジが削除された時
                // 反対のエッジは削除されている。
                if(halfEdges[i].DeleteFlag == false)
                {
                    halfEdgeDS.Editor.EdgeSplit(halfEdges[i]);
                }
            }

            // 追加されているため、リストの作り直し
            halfEdges = halfEdgeDS.HalfEdges.ToList();

            // 追加したエッジのみ捜査
            //for (int i = 0; i < halfEdges.Count; i++)
            //{
            //    HalfEdge edge = halfEdges[i];
            //    if (edge.DeleteFlag == true)
            //    {
            //        continue;
            //    }

            //    HalfEdge opposite = edge.Opposite;
            //    if(opposite.DeleteFlag == true)
            //    {
            //        continue;
            //    }

            //    // null は追加された頂点
            //    if(edge.Next.End.TmpParameter == null && opposite.Next.End.TmpParameter == null)
            //    {
            //        halfEdgeDS.Editor.EdgeFlips(edge);
            //    }
            //}

            halfEdgeDS.Editor.EndEdit();

            foreach(var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                vertex.TmpParameter = null;
            }
        }

        /// <summary>
        /// 細分割曲面用のパラメータ作成
        /// </summary>
        private class LoopSubdivParameter
        {
            public bool original;
            public LoopSubdivParameter(bool orig)
            {
                original = orig;
            }
        }
    }
}
