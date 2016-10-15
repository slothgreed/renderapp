using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.Analyzer
{
    public class Edge
    {
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
        public bool DeleteFlg { get; set; }
        /// <summary>
        /// 三角形を構成するエッジの角度thisと前のエッジの反対の角度
        /// </summary>
        private float _angle = 0.0f;
        /// <summary>
        /// 初期のIndex
        /// </summary>
        public int Index { get; set; }
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
        public static bool operator !=(Edge edge1,Edge edge2)
        {
            return !(edge1 == edge2);
        }
        public float Angle
        {
            get
            {
                if(_angle == 0.0f)
                {
                    if(Start == null || End == null || Before == null || Opposite == null)
                    {
                        GLUtil.Output.GLError("half edge angle error");
                        return _angle = 0.0f;
                    }

                    _angle = Utility.CCalc.Angle((End - Start).Normalized(), (Before.Opposite.Start - Before.Opposite.End).Normalized());
                }
                return _angle;
            }

        }

        public float Length
        {
            get
            {
                if(Start == null || End == null)
                {
                    GLUtil.Output.GLError("half edge Length error");
                    return 0.0f;
                }
                return (Start - End).Length;
            }
        }
        public Edge(Mesh mesh, Vertex start, Vertex end,int index)
        {
            Mesh = mesh;
            Start = start;
            End = end;
            Index = index;
        }
        public void Dispose()
        {
            DeleteFlg = true;
            Start = null;
            End = null;
            Next = null;
            Mesh = null;
            Before = null;
            Opposite = null;
        }

        public bool ErrorEdge()
        {
            return DeleteFlg;
        }
    }
}
