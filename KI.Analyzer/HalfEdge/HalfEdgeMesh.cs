using System.Collections.Generic;
using System.Linq;
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
        /// 面積
        /// </summary>
        private float area = 0;

        /// <summary>
        /// 法線
        /// </summary>
        private Vector3 normal = Vector3.Zero;

        /// <summary>
        /// 平面の公式
        /// </summary>
        private Vector4 plane = Vector4.Zero;

        /// <summary>
        /// 重心
        /// </summary>
        private Vector3 gravity = Vector3.Zero;

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
            SetupEdge(edge1, edge2, edge3);
            Index = index;
        }

        /// <summary>
        /// 要素番号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// テンポラリ計算用フラグ
        /// </summary>
        public object TmpParameter { get; set; }

        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlag { get; set; }

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
        /// 面積
        /// </summary>
        public float Area
        {
            get
            {
                if (area == 0)
                {
                    area = Mathmatics.Geometry.TriangleArea(Lines[0].Start.Position, Lines[1].Start.Position, Lines[2].Start.Position);
                }

                return area;
            }
        }

        /// <summary>
        /// 法線
        /// </summary>
        public Vector3 Normal
        {
            get
            {
                if (normal == Vector3.Zero)
                {
                    normal = Mathmatics.Geometry.Normal(
                        Lines[1].Start.Position - Lines[0].Start.Position,
                        Lines[2].Start.Position - Lines[0].Start.Position);
                }

                return normal;
            }
        }

        /// <summary>
        /// 平面の公式
        /// </summary>
        public Vector4 Plane
        {
            get
            {
                if (plane == Vector4.Zero)
                {
                    var positions = Lines.Select(p => p.Start.Position).ToArray();
                    plane = KI.Mathmatics.Plane.Formula(positions[0], positions[1], positions[2]);
                }

                return plane;
            }
        }

        /// <summary>
        /// 重心
        /// </summary>
        public Vector3 Gravity
        {
            get
            {
                if (gravity == Vector3.Zero)
                {
                    foreach (var position in Lines.Select(p => p.Start.Position))
                    {
                        gravity += position;
                    }

                    gravity /= Lines.Count;
                }

                return gravity;
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

            SetupEdge(edge1, edge2, edge3);
        }

        /// <summary>
        /// エッジに情報を加える
        /// </summary>
        /// <param name="edge1">エッジ1</param>
        /// <param name="edge2">エッジ2</param>
        /// <param name="edge3">エッジ3</param>
        public void SetupEdge(HalfEdge edge1, HalfEdge edge2, HalfEdge edge3)
        {
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
