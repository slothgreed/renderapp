using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// エッジクラス
    /// </summary>
    public class HalfEdge : Line
    {
        /// <summary>
        /// 三角形を構成するエッジの角度thisと前のエッジの反対の角度
        /// </summary>
        private float radian = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <param name="index">要素番号</param>
        public HalfEdge(HalfEdgeVertex start, HalfEdgeVertex end, int index = -1)
            : base(start, end)
        {
            Index = index;
            Start.AddEdge(this);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mesh">メッシュ</param>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <param name="index">要素番号</param>
        public HalfEdge(HalfEdgeMesh mesh, HalfEdgeVertex start, HalfEdgeVertex end, int index = -1)
            : base(start, end)
        {
            Mesh = mesh;
            Index = index;
            Start.AddEdge(this);
        }

        /// <summary>
        /// 始点
        /// </summary>
        public new HalfEdgeVertex Start
        {
            get
            {
                return base.Start as HalfEdgeVertex;
            }

            set
            {
                base.Start = value;
            }
        }

        /// <summary>
        /// 終点
        /// </summary>
        public new HalfEdgeVertex End
        {
            get
            {
                return base.End as HalfEdgeVertex;
            }

            set
            {
                base.End = value;
            }
        }

        /// <summary>
        /// ベクトルの取得
        /// </summary>
        public Vector3 Vector
        {
            get
            {
                return this.End - this.Start;
            }
        }

        /// <summary>
        /// エッジの長さ
        /// </summary>
        public float Length
        {
            get
            {
                return (Start.Position - End.Position).Length;
            }
        }

        /// <summary>
        /// メッシュ
        /// </summary>
        public HalfEdgeMesh Mesh { get; set; }

        /// <summary>
        /// 次のエッジ
        /// </summary>
        public HalfEdge Next { get; set; }

        /// <summary>
        /// 前のエッジ
        /// </summary>
        public HalfEdge Before { get; set; }

        /// <summary>
        /// 反対エッジ
        /// </summary>
        public HalfEdge Opposite { get; set; }

        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 初期のIndex
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// テンポラリ計算用フラグ
        /// </summary>
        public object TmpParameter { get; set; }

        /// <summary>
        /// degree エッジと、前のエッジの角度
        /// </summary>
        public float Radian
        {
            get
            {
                if (radian == 0.0f)
                {
                    radian = Calculator.Radian(End - Start, Before.Opposite.End - Before.Opposite.Start);
                }

                return radian;
            }
        }

        /// <summary>
        /// コタンジェントの算出このエッジと前のエッジのCot
        /// </summary>
        public float Cot
        {
            get
            {
                var edge = this.Next.Next;

                var prev = -edge.Before.Vector;

                return  Vector3.Dot(edge.Vector, prev) / Vector3.Cross(edge.Vector, prev).Length;
            }
        }

        /// <summary>
        /// 境界かどうか
        /// </summary>
        public bool IsBound
        {
            get
            {
                return Opposite != null;
            }
        }

        /// <summary>
        /// エラーエッジ
        /// </summary>
        public bool ErrorEdge
        {
            get
            {
                return Start.DeleteFlag ||
                !Start.ContainsEdge(this) ||
                End.DeleteFlag ||
                Mesh.DeleteFlag ||
                Next.DeleteFlag ||
                Opposite.DeleteFlag ||
                Before.DeleteFlag ||
                Mesh == Opposite.Mesh ||
                Next == Before ||
                DeleteFlag;
            }
        }

        #region [operator] 

        // 削除すること
        public static bool operator ==(HalfEdge edge1, HalfEdge edge2)
        {
            //参照が同じならTrue
            if (object.ReferenceEquals(edge1, edge2))
            {
                return true;
            }

            if ((object)edge1 == null || (object)edge2 == null)
            {
                return false;
            }
            //共有EdgeでもTrue
            return (edge1.Start == edge2.End && edge1.End == edge2.Start);
        }

        public static bool operator !=(HalfEdge edge1, HalfEdge edge2)
        {
            return !(edge1 == edge2);
        }

        #endregion

        /// <summary>
        /// 次と前のエッジの設定
        /// </summary>
        /// <param name="edge1">エッジ1</param>
        /// <param name="edge2">エッジ2</param>
        /// <param name="edge3">エッジ3</param>
        public static void SetupNextBefore(HalfEdge edge1, HalfEdge edge2, HalfEdge edge3)
        {
            edge1.Next = edge2;
            edge2.Next = edge3;
            edge3.Next = edge1;

            edge1.Before = edge3;
            edge2.Before = edge1;
            edge3.Before = edge2;
        }

        /// <summary>
        /// 反対エッジの設定
        /// </summary>
        /// <param name="edge">エッジ</param>
        /// <param name="oppo">反対</param>
        public static void SetupOpposite(HalfEdge edge, HalfEdge oppo)
        {
            edge.Opposite = oppo;
            oppo.Opposite = edge;
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            Start.RemoveAroundEdge(this);
            DeleteFlag = true;
            Start = null;
            End = null;
            Next = null;
            Mesh = null;
            Before = null;
            Opposite = null;
        }

        public bool HasVertex(HalfEdgeVertex vertex)
        {
            if (Start == vertex || End == vertex)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "Index:" + Index.ToString() + "Start:" + Start.ToString() + "End:" + End.ToString();
        }
    }
}
