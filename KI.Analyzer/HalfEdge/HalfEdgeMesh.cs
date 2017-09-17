using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// ハーフエッジ用のメッシュ
    /// </summary>
    public class HalfEdgeMesh : Mesh
    {
        /// <summary>
        /// エッジ
        /// </summary>
        public IEnumerable<HalfEdge> Edges
        {
            get
            {
                return Lines.OfType<HalfEdge>();
            }
        }

        /// <summary>
        /// 面積
        /// </summary>
        private float area = float.MinValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="index">要素番号</param>
        public HalfEdgeMesh(int index = -1)
        {
            Index = index;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="edge1">エッジ1</param>
        /// <param name="edge2">エッジ2</param>
        /// <param name="edge3">エッジ3</param>
        /// <param name="index">要素番号</param>
        public HalfEdgeMesh(HalfEdge edge1, HalfEdge edge2, HalfEdge edge3, int index = -1)
            : base(edge1, edge2, edge3)
        {
            SetEdge(edge1, edge2, edge3);
            Index = index;
        }

        /// <summary>
        /// 要素番号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// テンポラリ計算用フラグ
        /// </summary>
        public object CalcFlag { get; set; }

        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 面積
        /// </summary>
        public float Area
        {
            get
            {
                if (area == float.MinValue)
                {
                    var edge = Edges.ToArray();
                    area = KICalc.Area(edge[0].Start.Position, edge[1].Start.Position, edge[2].Start.Position);
                }

                return area;
            }
        }

        /// <summary>
        /// 鈍角三角形ならtrue
        /// </summary>
        public bool IsObtuse
        {
            get
            {
                return Edges.Any(p => p.Radian >= MathHelper.PiOver2);
            }
        }

        /// <summary>
        /// 周囲のエッジ
        /// </summary>
        public IEnumerable<HalfEdge> AroundEdge
        {
            get
            {
                return Edges;
            }
        }

        /// <summary>
        /// 頂点
        /// </summary>
        public IEnumerable<HalfEdgeVertex> AroundVertex
        {
            get
            {
                foreach (var edge in Edges)
                {
                    yield return edge.Start;
                }
            }
        }

        /// <summary>
        /// エラーメッシュ
        /// </summary>
        public bool ErrorMesh
        {
            get
            {
                return AroundEdge.Any(p => p.DeleteFlag)
                    || AroundVertex.Any(p => p.DeleteFlag)
                    || DeleteFlag;
            }
        }

        /// <summary>
        /// エッジのセッタ
        /// </summary>
        /// <param name="edge1">エッジ1</param>
        /// <param name="edge2">エッジ2</param>
        /// <param name="edge3">エッジ3</param>
        public void SetEdge(HalfEdge edge1, HalfEdge edge2, HalfEdge edge3)
        {
            Lines.Add(edge1);
            Lines.Add(edge2);
            Lines.Add(edge3);

            HalfEdge.SetupNextBefore(edge1, edge2, edge3);
            edge1.Mesh = this;
            edge2.Mesh = this;
            edge3.Mesh = this;
        }

        /// <summary>
        /// エッジのゲッタ
        /// </summary>
        /// <param name="index">番号</param>
        /// <returns>エッジ</returns>
        public HalfEdge GetEdge(int index)
        {
            if (Edges.Count() < index)
            {
                return null;
            }

            return Edges.ElementAt(index);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            DeleteFlag = true;
        }

        internal bool HasVertex(HalfEdgeVertex vertex)
        {
            return Vertexs.OfType<HalfEdgeVertex>().Where(p => p == vertex).Any();
        }
    }
}
