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
        /// 頂点1
        /// </summary>
        public virtual Vertex Start { get; protected set; }

        /// <summary>
        /// 頂点2
        /// </summary>
        public virtual Vertex End { get; protected set; }

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
    }
}
