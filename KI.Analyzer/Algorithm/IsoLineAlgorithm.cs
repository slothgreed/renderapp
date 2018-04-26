using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// 等値線のアルゴリズム(Z)だけ対応
    /// </summary>
    public class IsoLineAlgorithm
    {
        /// <summary>
        /// ハーフエッジデータ構造
        /// </summary>
        private HalfEdgeDS halfEdgeDS;

        public IsoLineSpace[] isoSpace;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="halfEdgeDS">ハーフエッジデータ構造</param>
        public IsoLineAlgorithm(HalfEdgeDS halfEdgeDS)
        {
            this.halfEdgeDS = halfEdgeDS;
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="space">間隔</param>
        public void Calculate(float space)
        {
            PreProcess(space);
        }

        /// <summary>
        /// 平面の間隔毎にポリゴンを振り分ける
        /// </summary>
        /// <param name="space">空間</param>
        private void PreProcess(float space)
        {
            BDB bdb = new BDB(halfEdgeDS.Vertexs.Select(p => p.Position).ToList());
            int spaceNum = (int)((bdb.Max.Z - bdb.Min.Z) / space) + 1;

            IsoLineSpace[] isoLines = new IsoLineSpace[spaceNum];

            // 空間の計算
            for(int i = 0; i < isoLines.Length; i++)
            {
                float min = bdb.Min.Z + i * space;
                float max = bdb.Min.Z + ((i + 1) * space);
                isoLines[i] = new IsoLineSpace(min, max);
            }

            //空間内のエッジの計算
            foreach (var isoLineSpace in isoLines)
            {
                foreach (var edge in halfEdgeDS.HalfEdges)
                {
                    if (isoLineSpace.InSpace(edge.Start.Position, edge.End.Position))
                    {
                        isoLineSpace.Edges.Add(edge);
                    }
                }
            }

            isoSpace = isoLines;
        }

        /// <summary>
        /// 等値線毎の空間
        /// </summary>
        public class IsoLineSpace
        {
            /// <summary>
            /// 等値線上の面リスト
            /// </summary>
            public List<HalfEdge> Edges = new List<HalfEdge>();

            /// <summary>
            /// 等値線最小値
            /// </summary>
            public float Min;

            /// <summary>
            /// 等値線最大値
            /// </summary>
            public float Max;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="min">最小値</param>
            /// <param name="max">最大値</param>
            public IsoLineSpace(float min, float max)
            {
                Min = min;
                Max = max;
            }

            /// <summary>
            /// 線分が空間内にあるか
            /// </summary>
            /// <param name="start">始点</param>
            /// <param name="end">終点</param>
            public bool InSpace(Vector3 start, Vector3 end)
            {
                // 両方上ならfalse
                if (start.Z < Min &&
                    end.Z < Min)
                {
                    return false;
                }

                // 両方下ならfalse
                if (start.Z > Max &&
                    end.Z > Max)
                {
                    return false;
                }

                return true;
            }
        }

    }
}
