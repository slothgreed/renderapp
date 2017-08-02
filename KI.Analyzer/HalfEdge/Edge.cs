using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using KI.Foundation.Utility;

namespace KI.Analyzer
{
    /// <summary>
    /// エッジクラス
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// 三角形を構成するエッジの角度thisと前のエッジの反対の角度
        /// </summary>
        private float angle = 0.0f;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <param name="index">要素番号</param>
        public Edge(Vertex start, Vertex end, int index = -1)
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
        public Edge(Mesh mesh, Vertex start, Vertex end, int index = -1)
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
        public Edge Next { get; set; }

        /// <summary>
        /// 前のエッジ
        /// </summary>
        public Edge Before { get; set; }

        /// <summary>
        /// 反対エッジ
        /// </summary>
        public Edge Opposite { get; set; }

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

        #region [operator] 
        public static bool operator ==(Edge edge1, Edge edge2)
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

        public static bool operator !=(Edge edge1, Edge edge2)
        {
            return !(edge1 == edge2);
        }

        #endregion


        public float Angle
        {
            get
            {
                if (angle == 0.0f)
                {
                    if (Start == null || End == null || Before == null || Opposite == null)
                    {
                        Logger.GLLog(Logger.LogLevel.Error, "half edge angle error");
                        return angle = 0.0f;
                    }

                    angle = KICalc.Angle((End - Start).Normalized(), (Before.Opposite.Start - Before.Opposite.End).Normalized());
                }

                return angle;
            }
        }

        /// <summary>
        /// エッジの長さ
        /// </summary>
        public float Length
        {
            get
            {
                if (Start == null || End == null)
                {
                    Logger.GLLog(Logger.LogLevel.Error, "half edge Length error");
                    return 0.0f;
                }

                return (Start - End).Length;
            }
        }

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

        public static void SetupNextBefore(Edge edge1, Edge edge2, Edge edge3)
        {
            edge1.Next = edge2;
            edge2.Next = edge3;
            edge3.Next = edge1;

            edge1.Before = edge3;
            edge2.Before = edge1;
            edge3.Before = edge2;
        }

        public static void SetupOpposite(Edge edge, Edge oppo)
        {
            edge.Opposite = oppo;
            oppo.Opposite = edge;
        }
    }
}
