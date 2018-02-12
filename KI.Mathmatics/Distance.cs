using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// 距離計算
    /// </summary>
    public static class Distance
    {
        /// <summary>
        /// 点と平面の距離
        /// </summary>
        /// <param name="plane">平面</param>
        /// <param name="point">点</param>
        /// <returns>距離</returns>
        public static float PlaneToPoint(Vector4 plane, Vector3 point)
        {
            float[] p = new float[4];
            p[0] = plane.X;
            p[1] = plane.Y;
            p[2] = plane.Z;
            p[3] = plane.W;
            return PlaneToPoint(p, point);
        }

        /// <summary>
        /// 点と平面の距離
        /// http://keisan.casio.jp/exec/system/1202458240
        /// </summary>
        /// <param name="plane">平面</param>
        /// <param name="point">頂点</param>
        /// <returns></returns>
        public static float PlaneToPoint(float[] plane, Vector3 point)
        {
            float low = (float)Math.Sqrt((double)(plane[0] * plane[0] + plane[1] * plane[1] + plane[2] * plane[2]));
            float up = (float)Math.Abs((double)(plane[0] * point.X + plane[1] * point.Y + plane[2] * point.Z + plane[3]));
            return up / low;
        }

        /// <summary>
        /// 線と線の距離(3D 最短距離)
        /// </summary>
        /// <param name="start0">始点0</param>
        /// <param name="end0">終点0</param>
        /// <param name="start1">始点1</param>
        /// <param name="end1">終点1</param>
        /// <param name="distance">距離</param>
        /// <returns>成功</returns>
        public static bool LineToLine(Vector3 start0, Vector3 end0, Vector3 start1, Vector3 end1, out float distance)
        {
            distance = float.MaxValue;

            Vector3 result0 = Vector3.Zero;
            Vector3 result1 = Vector3.Zero;

            Vector3 p1 = start0;
            Vector3 p2 = end0;
            Vector3 p3 = start1;
            Vector3 p4 = end1;
            Vector3 p13 = p1 - p3;
            Vector3 p43 = p4 - p3;

            if (p43.Length < Calculator.THRESHOLD05)
            {
                return false;
            }

            Vector3 p21 = p2 - p1;
            if (p21.Length < Calculator.THRESHOLD05)
            {
                return false;
            }

            float d1343 = p13.X * p43.X + p13.Y * p43.Y + p13.Z * p43.Z;
            float d4321 = p43.X * p21.X + p43.Y * p21.Y + p43.Z * p21.Z;
            float d1321 = p13.X * p21.X + p13.Y * p21.Y + p13.Z * p21.Z;
            float d4343 = p43.X * p43.X + p43.Y * p43.Y + p43.Z * p43.Z;
            float d2121 = p21.X * p21.X + p21.Y * p21.Y + p21.Z * p21.Z;

            float denom = d2121 * d4343 - d4321 * d4321;
            if (Math.Abs(denom) < Calculator.THRESHOLD05)
            {
                return false;
            }

            float numer = d1343 * d4321 - d1321 * d4343;

            float mua = numer / denom;
            float mub = (d1343 + d4321 * mua) / d4343;

            result0 = p1 + mua * p21;
            result1 = p3 + mub * p43;

            distance = (result0 - result1).Length;
            return true;
        }

    }
}
