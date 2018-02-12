using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Mathmatics
{
    public static class Inside
    {
        /// <summary>
        /// ボックス内にあるか
        /// </summary>
        /// <param name="point">頂点</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        public static bool Box(Vector3 point, Vector3 min, Vector3 max)
        {
            if (min.X < point.X && point.X < max.X)
            {
                if (min.Y < point.Y && point.Y < max.Y)
                {
                    if (min.Z < point.Z && point.Z < max.Z)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 内外判定.三角形内部の場合true外の場合false
        /// http://www.sousakuba.com/Programming/gs_hittest_point_triangle.html
        /// </summary>
        /// <returns></returns>
        public static bool Triangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 ab = b - a;
            Vector3 bp = p - b;
            Vector3 bc = c - b;
            Vector3 cp = p - c;
            Vector3 ca = a - c;
            Vector3 ap = p - a;

            Vector3 c1 = Vector3.Cross(ab, bp);
            Vector3 c2 = Vector3.Cross(bc, cp);
            Vector3 c3 = Vector3.Cross(cp, ap);

            double dot12 = Vector3.Dot(c1, c2);
            double dot13 = Vector3.Dot(c1, c3);
            if (dot12 > 0 && dot13 > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 4角形の内外判定
        /// </summary>
        /// <returns></returns>
        public static bool Rectangle(Vector3 point, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            if (Triangle(point, a, b, c) || Triangle(point, a, c, d))
            {
                return true;
            }

            return false;
        }
    }
}
