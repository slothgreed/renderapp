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
        /// 三角形を構成するエッジの角度thisと前のエッジの反対の角度
        /// </summary>
        public float Angle { get; set; }

        public Edge(Mesh meshIndex, Vertex start, Vertex end)
        {
            Mesh = meshIndex;
            Start = start;
            End = end;
        }



    }
}
