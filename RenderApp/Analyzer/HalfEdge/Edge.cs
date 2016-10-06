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
        public Edge(Mesh meshIndex, Vertex start, Vertex end)
        {
            Mesh = meshIndex;
            Start = start;
            End = end;
        }
        public void Dispose()
        {
            DeleteFlg = true;
            Start = null;
            End = null;
            Next = null;
            Before = null;
            Opposite = null;
        }


    }
}
