using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// HalfEdgeデータ構造のUtility
    /// </summary>
    public static class HalfEdgeDSUtility
    {
        /// <summary>
        /// コタンジェントの算出
        /// </summary>
        /// <param name="edge">このエッジと前のエッジのCot</param>
        public static float Cot(HalfEdge edge)
        {
            var prev = -edge.Before.Vector;

            return (float)(Math.Cos(edge.Radian) / Math.Sin(edge.Radian));
            return Vector3.Dot(edge.Vector, prev) / Vector3.Cross(edge.Vector, prev).Length;
        }
    }
}
