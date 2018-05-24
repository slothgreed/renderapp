using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Gfx.Geometry
{
    /// <summary>
    /// 線分
    /// </summary>
    public class Line
    {
        /// <summary>
        /// 長さ
        /// </summary>
        private float length = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        public Line(Vertex startVertex, Vertex endVertex)
        {
            Start = startVertex;
            End = endVertex;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        public Line(Vector3 startVertex, Vector3 endVertex)
        {
            Start = new Vertex(0, startVertex);
            End = new Vertex(0, endVertex);
        }

        /// <summary>
        /// 頂点1
        /// </summary>
        public virtual Vertex Start { get; set; }

        /// <summary>
        /// 頂点2
        /// </summary>
        public virtual Vertex End { get; set; }
        
        public override string ToString()
        {
            return "Start:" + Start.ToString() + "End:" + End.ToString();
        }
    }
}
