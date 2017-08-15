using KI.Foundation.Utility;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// エッジクラス
    /// </summary>
    public class HalfEdge
    {
        /// <summary>
        /// 三角形を構成するエッジの角度thisと前のエッジの反対の角度
        /// </summary>
        private float radian = 0.0f;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <param name="index">要素番号</param>
        public HalfEdge(Vertex start, Vertex end, int index = -1)
        {
            Start = start;
            End = end;
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
        public HalfEdge(Mesh mesh, Vertex start, Vertex end, int index = -1)
        {
            Mesh = mesh;
            Start = start;
            End = end;
            Index = index;
            Start.AddEdge(this);
        }

        /// <summary>
        /// 始点
        /// </summary>
        public Vertex Start { get; set; }

        /// <summary>
        /// 終点
        /// </summary>
        public Vertex End { get; set; }

        /// <summary>
        /// メッシュ
        /// </summary>
        public Mesh Mesh { get; set; }

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
        public object CalcFlag { get; set; }

        /// <summary>
        /// degree エッジと、前のエッジの角度
        /// </summary>
        public float Radian
        {
            get
            {
                if (radian == 0.0f)
                {
                    radian = KICalc.Radian((End - Start), (Before.Opposite.End - Before.Opposite.Start));
                }

                return radian;
            }
        }

        /// <summary>
        /// エッジの長さ
        /// </summary>
        public float Length
        {
            get
            {
                return (Start - End).Length;
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
    }
}
