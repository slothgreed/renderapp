using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Foundation.KIMath
{
    /// <summary>
    /// 線文
    /// </summary>
    public class Line
    {
        /// <summary>
        /// 線分
        /// </summary>
        /// <param name="vertex0">頂点1</param>
        /// <param name="vertex1">頂点2</param>
        public Line(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// 頂点1
        /// </summary>
        public Vector3 Start { get; private set; }

        /// <summary>
        /// 頂点2
        /// </summary>
        public Vector3 End { get; private set; }
    }
}
