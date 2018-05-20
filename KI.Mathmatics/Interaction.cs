using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// 交差判定
    /// </summary>
    public class Interaction
    {
        /// <summary>
        /// 線と平面の交差判定
        /// http://www.sousakuba.com/Programming/gs_plane_line_intersect.html
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <param name="plane1">平面の公式</param>
        /// <returns>交点</returns>
        public static bool PlaneToLine(Vector3 start, Vector3 end, Vector4 plane1, out Vector3 result)
        {
            Vector4 plane = new Vector4();
            plane = Plane.Normalize(plane1);
            Vector3 plane3 = new Vector3(plane);
            Vector3 planeV = new Vector3(plane.X * plane.W, plane.Y * plane.W, plane.Z * plane.W);
            Vector3 PA = new Vector3();
            Vector3 PB = new Vector3();
            PA = planeV - start;
            PB = planeV - end;
            float dotPA = Vector3.Dot(PA, plane3);
            float dotPB = Vector3.Dot(PB, plane3);
            if (Math.Abs(dotPA) < 0.0001) dotPA = 0;
            if (Math.Abs(dotPB) < 0.0001) dotPB = 0;

            if (dotPA == 0 && dotPB == 0)
            {
                result = Vector3.Zero;
                return false;
            }
            else if ((dotPA >= 0 && dotPB <= 0) || (dotPA <= 0 && dotPB >= 0))
            {
            }
            else
            {
                result = Vector3.Zero;
                return false;
            }

            Vector3 AB = new Vector3();
            AB = end - start;
            float ratio = (float)(Math.Abs(dotPA) / (Math.Abs(dotPA) + Math.Abs(dotPB)));

            result.X = start.X + (AB.X * ratio);
            result.Y = start.Y + (AB.Y * ratio);
            result.Z = start.Z + (AB.Z * ratio);

            return true;
        }

        /// <summary>
        /// 線と平面(4角形)の交差位置
        /// </summary>
        /// <param name="v1">平面の頂点1</param>
        /// <param name="v2">平面の頂点2</param>
        /// <param name="v3">平面の頂点3</param>
        /// <param name="v4">平面の頂点4</param>
        /// <param name="near">始点</param>
        /// <param name="far">終点</param>
        /// <param name="min">min以下の交点を算出</param>
        /// <param name="result">交点</param>
        /// <returns>あるtrue,ないfalse</returns>
        public static bool RectangleToLine(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 near, Vector3 far, ref float min, out Vector3 result)
        {
            Vector4 surface = Plane.Formula(v1, v2, v3);

            if (PlaneToLine(near, far, surface, out result) == false)
            {
                return false;
            }

            float length;
            //交点あり
            if (result.Length != 0)
            {
                length = (result - near).Length;
                if (length < min)
                {
                    //四角形ない
                    if (Inside.Rectangle(result, v1, v2, v3, v4))
                    {
                        min = length;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 線と平面(3角形)の交差位置
        /// </summary>
        /// <param name="v1">平面の頂点1</param>
        /// <param name="v2">平面の頂点2</param>
        /// <param name="v3">平面の頂点3</param>
        /// <param name="near">始点</param>
        /// <param name="far">終点</param>
        /// <param name="min">min以下の交点を算出</param>
        /// <param name="result">交点</param>
        /// <returns>あるtrue,ないfalse</returns>
        public static bool TriangleToLine(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 near, Vector3 far, ref float min, out Vector3 result)
        {
            Vector4 surface = Plane.Formula(v1, v2, v3);
            if (PlaneToLine(near, far, surface, out result) == false)
            {
                return false;
            }

            float length;
            //交点あり
            if (result.Length != 0)
            {
                length = (result - near).Length;
                if (length < min)
                {
                    //3角形ない
                    if (Inside.Triangle(result, v1, v2, v3))
                    {
                        min = length;
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// 垂線の足を算出する
        /// </summary>
        /// <param name="result">結果</param>
        /// <param name="point">入力点</param>
        /// <param name="start">エッジの始点</param>
        /// <param name="fin">エッジの終点</param>
        /// <returns></returns>
        /// http://okwave.jp/qa/q3994535.html
        //>線分ABに対して点Pから垂線を下ろすことが出来るかどうかの判定をするには？
        //ベクトル↑AP=(xp-xa,yp-ya,zp-za),ベクトル↑AB=(xb-xa,yb-ya,zb-za)となります。
        //垂線の足をHとするとベクトル↑AH=k↑AB,↑AB・↑HP=0が成立します。
        //↑HP=↑AP-↑AH=↑AP-k↑AB
        //↑AB・↑HP=↑AB・(↑AP-k↑AB)=↑AB・↑AP-k↑AB・↑AB=0
        //k=↑AB・↑AP/|↑AB|^2
        //成分で表記すると
        //k={(xp-xa)(xb-xa)+(yp-ya)(yb-ya)+(zp-za)(zb-za)}/{(xb-xa)^2+(yb-ya)^2+(zb-za)^2}

        //これを計算して0≦k≦1なら線分AB上に垂線の足Hがきます。つまり、（１）を満足する
        //ということです。また、kが求まればHの座標は

        //H:(k*xb+(1-k)xa,k*yb+(1-k)ya,k*zb+(1-k)za)
        public static bool PerpendicularPoint(Vector3 point, Vector3 start, Vector3 fin, out Vector3 result)
        {
            result = new Vector3();
            Vector3 SP = new Vector3(point - start);
            Vector3 SF = new Vector3(fin - start);
            float inner = Vector3.Dot(SP, SF) / (SF.X * SF.X + SF.Y * SF.Y + SF.Z * SF.Z);

            if (0 <= inner && inner <= 1)
            {
                result = (inner * SF) + start;
                return true;
            }

            return false;
        }
    }
}
