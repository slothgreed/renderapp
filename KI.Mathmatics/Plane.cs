using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// 平面
    /// </summary>
    public static class Plane
    {
        /// <summary>
        /// 平面の公式の正規化
        /// </summary>
        /// <param name="plane">平面の公式</param>
        /// <returns>正規化後</returns>
        public static Vector4 Normalize(Vector4 plane)
        {
            Vector4 result = new Vector4();
            Vector3 posit = Vector3.Zero;
            if (plane.X != 0)
            {
                posit.X = plane.W / plane.X;
                posit.Y = 0;
                posit.Z = 0;
            }

            if (plane.Y != 0)
            {
                posit.X = 0;
                posit.Y = plane.W / plane.Y;
                posit.Z = 0;
            }

            if (plane.Z != 0)
            {
                posit.X = 0;
                posit.Y = 0;
                posit.Z = plane.W / plane.Z;
            }

            float scalar = new Vector3(plane).Length;
            result.X = plane[0] / scalar;
            result.Y = plane[1] / scalar;
            result.Z = plane[2] / scalar;
            result.W = -Vector3.Dot(new Vector3(result), posit);
            return result;
        }

        /// <summary>
        /// 平面の公式の算出
        /// </summary>
        /// <param name="v1">頂点1</param>
        /// <param name="v2">頂点2</param>
        /// <param name="v3">頂点3</param>
        /// <returns>平面の公式</returns>
        public static Vector4 Formula(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 ex;
            Vector3 edge1 = v2 - v1;
            Vector3 edge2 = v3 - v1;
            Vector4 result = new Vector4();
            ex = Vector3.Cross(edge1, edge2);
            result.X = ex.X;
            result.Y = ex.Y;
            result.Z = ex.Z;
            result.W = -Vector3.Dot(v1, ex);

            return result;
        }

        /// <summary>
        /// 平面の公式の算出
        /// </summary>
        /// <param name="position">頂点</param>
        /// <param name="normal">法線</param>
        /// <returns>平面の公式</returns>
        public static Vector4 Formula(Vector3 position, Vector3 normal)
        {
            Vector3 normalize = normal.Normalized();
            float d = -Vector3.Dot(position, normalize);

            Vector4 plane = new Vector4(normalize.X, normalize.Y, normalize.Z, d);
            return plane;
        }
    }
}
