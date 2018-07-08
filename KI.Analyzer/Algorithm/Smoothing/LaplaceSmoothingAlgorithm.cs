using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// 重心によるスムージングアルゴリズム
    /// </summary>
    public class LaplaceSmoothingAlgorithm
    {
        /// <summary>
        /// 繰り返し回数
        /// </summary>
        private int loopNum = 0;

        /// <summary>
        /// ハーフエッジデータ構造
        /// </summary>
        private HalfEdgeDS halfEdgeDS = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="halfEdge">ハーフエッジ</param>
        /// <param name="num">繰り返し変え数</param>
        public LaplaceSmoothingAlgorithm(HalfEdgeDS halfEdge, int num)
        {
            halfEdgeDS = halfEdge;
            loopNum = num;
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        public void Calculate()
        {
            Vector3[] laplacianPoint = new Vector3[halfEdgeDS.Vertexs.Count];
            int i = 0;
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                Vector3 sum = Vector3.Zero;
                foreach (var around in vertex.AroundVertex)
                {
                    sum += around.Position - vertex.Position;
                }

                laplacianPoint[i] = sum / vertex.AroundVertex.Count() + vertex.Position;
                i++;
            }

            i = 0;
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                vertex.Position = laplacianPoint[i];
                i++;
            }
        }
    }
}
