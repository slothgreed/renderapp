using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace KI.Foundation.Utility
{
    /// <summary>
    /// 計算用Utility関数
    /// </summary>
    public class KICalc
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static KICalc()
        {
            float scale = 4;
            for (int i = 0; i < 256; i++)
            {
                if (i <= 63)
                {
                    RGB[i].X = 0 / 255.0f;
                    RGB[i].Y = scale * i / 255.0f;
                    RGB[i].Z = 255 / 255.0f;
                    continue;
                }

                if (i <= 127)
                {
                    RGB[i].X = 0 / 255.0f;
                    RGB[i].Y = 255 / 255.0f;
                    RGB[i].Z = (255 - (scale * (i - 64))) / 255.0f;
                    continue;
                }

                if (i <= 191)
                {
                    RGB[i].X = (scale * (i - 127)) / 255.0f;
                    RGB[i].Y = 255 / 255.0f;
                    RGB[i].Z = 0 / 255.0f;
                    continue;
                }

                if (i <= 255)
                {
                    RGB[i].X = 255 / 255.0f;
                    RGB[i].Y = (255 - (scale * (i - 192))) / 255.0f;
                    RGB[i].Z = 0 / 255.0f;
                    continue;
                }
            }
        }

        /// <summary>
        /// 誤差範囲 0.00001
        /// </summary>
        public const float THRESHOLD05 = 0.00001f;

        /// <summary>
        /// 疑似カラー
        /// </summary>
        public static Vector3[] RGB { get; set; } = new Vector3[256];

        #region [疑似カラー]
        /// <summary>
        /// 疑似カラーの取得
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>疑似カラー</returns>
        public static Vector3 GetPseudoColor(float value, float min, float max)
        {
            if (max <= value)
            {
                return RGB[255];
            }

            if (min >= value)
            {
                return RGB[0];
            }

            if (max - min == 0)
            {
                return RGB[0];
            }

            float length = max - min;
            float scale = 255 * (value - min) / length;

            return RGB[(int)scale];
        }
        #endregion

        /// <summary>
        /// v1,v2の各要素を比較して、小さいほうを返します。
        /// </summary>
        /// <param name="v1">vector1</param>
        /// <param name="v2">vector2</param>
        /// <returns>ベクトル</returns>
        public static Vector3 MinVector(Vector3 v1, Vector3 v2)
        {
            Vector3 result = new Vector3(v1);
            if (result.X > v2.X) { result.X = v2.X; }
            if (result.Y > v2.Y) { result.Y = v2.Y; }
            if (result.Z > v2.Z) { result.Z = v2.Z; }

            return result;
        }

        /// <summary>
        /// 頂点リストから、最小値最大値を算出
        /// </summary>
        /// <param name="position">頂点</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public static void MinMax(IEnumerable<Vector3> position, out Vector3 min, out Vector3 max)
        {
            if(!position.Any())
            {
                min = Vector3.Zero;
                max = Vector3.Zero;
                return;
            }

            min = position.First();
            max = position.First();
            foreach (var pos in position)
            {
                if (min.X > pos.X) { min.X = pos.X; }
                if (min.Y > pos.Y) { min.Y = pos.Y; }
                if (min.Z > pos.Z) { min.Z = pos.Z; }

                if (max.X < pos.X) { max.X = pos.X; }
                if (max.Y < pos.Y) { max.Y = pos.Y; }
                if (max.Z < pos.Z) { max.Z = pos.Z; }
            }
        }


        /// <summary>
        /// v1,v2の各要素を比較して、大きいほうを返します。
        /// </summary>
        /// <param name="v1">vector1</param>
        /// <param name="v2">vector2</param>
        /// <returns>ベクトル</returns>
        public static Vector3 MaxVector(Vector3 v1, Vector3 v2)
        {
            Vector3 result = new Vector3(v1);
            if (result.X < v2.X) { result.X = v2.X; }
            if (result.Y < v2.Y) { result.Y = v2.Y; }
            if (result.Z < v2.Z) { result.Z = v2.Z; }

            return result;
        }

        /// <summary>
        /// 法線の算出
        /// </summary>
        /// <param name="vertex1"></param>
        /// <param name="vertex2"></param>
        /// <param name="vertex3"></param>
        /// <returns></returns>
        public static Vector3 Normal(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            Vector3 point = vertex2 - vertex1;
            Vector3 point2 = vertex3 - vertex1;
            Vector3 normal = new Vector3();
            normal = Vector3.Cross(point, point2);

            return normal.Normalized();
        }

        /// <summary>
        /// 法線の算出
        /// </summary>
        /// <param name="vector1">ベクトル1</param>
        /// <param name="vector2">ベクトル2</param>
        /// <returns>法線</returns>
        public static Vector3 Normal(Vector3 vector1, Vector3 vector2)
        {
            Vector3 normal;
            normal = Vector3.Cross(vector1, vector2);
            return normal.Normalized();
        }

        #region [角度の算出]
        /// <summary>
        /// radian 0 ~ 180
        /// </summary>
        /// <param name="vector1">ベクトル1</param>
        /// <param name="vector2">ベクトル2</param>
        /// <returns>Radian</returns>
        public static float Radian(Vector3 vector1, Vector3 vector2)
        {
            float cos = 0;

            Vector3 v1 = vector1.Normalized();
            Vector3 v2 = vector2.Normalized();
            cos = Vector3.Dot(v1, v2);
            cos = (float)Math.Acos(cos);

            return cos;
        }

        public static float Radian(Vector3 a, Vector3 b, Vector3 axis)
        {
            float cos = 0;
            Vector3 v1 = new Vector3(a);
            Vector3 v2 = new Vector3(b);
            Vector3 ex;
            v1.Normalize();
            v2.Normalize();
            ex = Vector3.Cross(v1, v2);
            cos = Vector3.Dot(v1, v2);

            cos = (float)Math.Acos(cos);

            if (Vector3.Dot(ex, axis) < 0)
            {
                return -cos;
            }
            else
            {
                return cos;
            }
        }

        #endregion
        /// <summary>
        /// 三角形の面積の算出
        /// </summary>
        /// <param name="tri1">頂点1</param>
        /// <param name="tri2">頂点2</param>
        /// <param name="tri3">頂点3</param>
        /// <returns>面積</returns>
        public static float Area(Vector3 tri1, Vector3 tri2, Vector3 tri3)
        {
            float edge1 = (tri1 - tri2).Length;
            float edge2 = (tri2 - tri3).Length;
            float edge3 = (tri3 - tri1).Length;
            float sum = (edge1 + edge2 + edge3) / 2;

            float value = sum * (sum - edge1) * (sum - edge2) * (sum - edge3);

            return (float)Math.Sqrt(value);
        }

        ///// <summary>
        ///// 平面の公式から平面の点を求める
        ///// </summary>
        ///// <param name="surface">平面の公式</param>
        ///// <param name="center">平面上の中心位置</param>
        ///// <returns></returns>
        //public static List<Vector3> GetPlanePoint(float[] surface, Vector3 center)
        //{
        //    List<Vector3> plane = new List<Vector3>();

        //    Vector3 p1 = new Vector3();
        //    if (surface[1] != 0)
        //    {
        //        p1 = new Vector3(0, -surface[3] / surface[1], 0);
        //    }
        //    else
        //    {
        //        p1 = new Vector3(-surface[3] / surface[0], 0, 0);
        //    }
        //    Vector3 p2 = new Vector3();
        //    Vector3 p3 = new Vector3();
        //    Vector3 p4 = new Vector3();
        //    p1 -= center;
        //    p1.Normalize();
        //    p1 *= 10;
        //    Vector3 normal = new Vector3(surface[0], surface[1], surface[2]);

        //    p2 = Quaternion(MathHelper.DegreesToRadians(90), normal, p1);
        //    p3 = Quaternion(MathHelper.DegreesToRadians(180), normal, p1);
        //    p4 = Quaternion(MathHelper.DegreesToRadians(270), normal, p1);
        //    plane.Add(p1 + center);
        //    plane.Add(p2 + center);
        //    plane.Add(p3 + center);
        //    plane.Add(p4 + center);
        //    return plane;

        //}

        #region [交差判定]
        /// <summary>
        /// 線と平面の交差判定
        /// http://www.sousakuba.com/Programming/gs_plane_line_intersect.html
        /// </summary>
        /// <returns></returns>
        public static Vector3 CrossPlanetoLine(Vector3 start, Vector3 fin, Vector4 plane1)
        {
            Vector3 result = new Vector3();
            Vector4 plane = new Vector4();
            plane = NormalizePlane(plane1);
            Vector3 plane3 = new Vector3(plane);
            Vector3 planeV = new Vector3(plane.X * plane.W, plane.Y * plane.W, plane.Z * plane.W);
            Vector3 PA = new Vector3();
            Vector3 PB = new Vector3();
            PA = planeV - start;
            PB = planeV - fin;
            float dotPA = Vector3.Dot(PA, plane3);
            float dotPB = Vector3.Dot(PB, plane3);
            if (Math.Abs(dotPA) < 0.0001) dotPA = 0;
            if (Math.Abs(dotPB) < 0.0001) dotPB = 0;

            if (dotPA == 0 && dotPB == 0)
            {
                result.X = 0;
                result.Y = 0;
                result.Z = 0;
                return result;
            }
            else if ((dotPA >= 0 && dotPB <= 0) || (dotPA <= 0 && dotPB >= 0))
            {
            }
            else
            {
                result.X = 0;
                result.Y = 0;
                result.Z = 0;
                return result;
            }

            Vector3 AB = new Vector3();
            AB = fin - start;
            float ratio = (float)(Math.Abs(dotPA) / (Math.Abs(dotPA) + Math.Abs(dotPB)));

            result.X = start.X + (AB.X * ratio);
            result.Y = start.Y + (AB.Y * ratio);
            result.Z = start.Z + (AB.Z * ratio);

            return result;
        }

        /// <summary>
        /// ボックス内にあるか
        /// </summary>
        /// <param name="point">頂点</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        public static bool InBox(Vector3 point, Vector3 min, Vector3 max)
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
        /// 線と平面の交差位置
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
        public static bool CrossPlanetoLinePos(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 near, Vector3 far, ref float min, out Vector3 result)
        {
            Vector4 surface = GetPlaneFormula(v1, v2, v3);
            result = CrossPlanetoLine(near, far, surface);
            float length;
            //交点あり
            if (result.Length != 0)
            {
                length = (result - near).Length;
                if (length < min)
                {
                    //四角形ない
                    if (KICalc.InnerSurface(result, v1, v2, v3, v4))
                    {
                        min = length;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 線と平面の交差位置
        /// </summary>
        /// <param name="v1">平面の頂点1</param>
        /// <param name="v2">平面の頂点2</param>
        /// <param name="v3">平面の頂点3</param>
        /// <param name="near">始点</param>
        /// <param name="far">終点</param>
        /// <param name="min">min以下の交点を算出</param>
        /// <param name="result">交点</param>
        /// <returns>あるtrue,ないfalse</returns>
        public static bool CrossPlanetoLinePos(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 near, Vector3 far, ref float min, out Vector3 result)
        {
            Vector4 surface = GetPlaneFormula(v1, v2, v3);
            result = CrossPlanetoLine(near, far, surface);
            float length;
            //交点あり
            if (result.Length != 0)
            {
                length = (result - near).Length;
                if (length < min)
                {
                    //3角形ない
                    if (KICalc.InnerSurface(result, v1, v2, v3))
                    {
                        min = length;
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion
        #region [平面]
        /// <summary>
        /// 平面の公式の正規化
        /// </summary>
        /// <param name="plane">平面の公式</param>
        /// <returns>正規化後</returns>
        public static Vector4 NormalizePlane(Vector4 plane)
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
        public static Vector4 GetPlaneFormula(Vector3 v1, Vector3 v2, Vector3 v3)
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
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static Vector4 GetPlaneFormula(Vector3 position, Vector3 normal)
        {
            Vector3 normalize = normal.Normalized();
            float d = -Vector3.Dot(position, normalize);

            Vector4 plane = new Vector4(normalize.X, normalize.Y, normalize.Z, d);
            return plane;
        }
        #endregion
        #region [内外判定]
        /// <summary>
        /// 内外判定.三角形内部の場合true外の場合false
        /// http://www.sousakuba.com/Programming/gs_hittest_point_triangle.html
        /// </summary>
        /// <returns></returns>
        public static bool InnerSurface(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
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
        /// 多角形の内外判定
        /// </summary>
        /// <returns></returns>
        public static bool InnerSurface(Vector3 point, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            if (InnerSurface(point, a, b, c) || InnerSurface(point, a, c, d))
            {
                return true;
            }

            return false;
        }
        #endregion
        #region [行列 * ベクトル]

        /// <summary>
        /// 行列の掛け算
        /// </summary>
        /// <param name="matrix">マトリクス</param>
        /// <param name="vector">ベクトル</param>
        /// <returns>ベクトル</returns>
        public static Vector3 Multiply(Matrix4 matrix, Vector3 vector)
        {
            Vector3 result = new Vector3();
            result.X = matrix.Column0[0] * vector.X + matrix.Column0[1] * vector.Y + matrix.Column0[2] * vector.Z + matrix.Column0[3];
            result.Y = matrix.Column1[0] * vector.X + matrix.Column1[1] * vector.Y + matrix.Column1[2] * vector.Z + matrix.Column1[3];
            result.Z = matrix.Column2[0] * vector.X + matrix.Column2[1] * vector.Y + matrix.Column2[2] * vector.Z + matrix.Column2[3];

            return Round(result);
        }

        /// <summary>
        /// 法線用
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 NormalMultiply(Matrix4 matrix, Vector3 vector)
        {
            Vector3 result = new Vector3();
            result.X = matrix.Column0[0] * vector.X + matrix.Column0[1] * vector.Y + matrix.Column0[2] * vector.Z;
            result.Y = matrix.Column1[0] * vector.X + matrix.Column1[1] * vector.Y + matrix.Column1[2] * vector.Z;
            result.Z = matrix.Column2[0] * vector.X + matrix.Column2[1] * vector.Y + matrix.Column2[2] * vector.Z;

            return Round(result);
        }

        /// <summary>
        /// 行列の掛け算
        /// </summary>
        /// <param name="matrix">マトリクス</param>
        /// <param name="vector">ベクトル</param>
        /// <returns>ベクトル</returns>
        public static Vector4 Multiply(Matrix4 matrix, Vector4 vector)
        {
            Vector4 result = new Vector4();
            result.X = matrix.Column0[0] * vector.X + matrix.Column0[1] * vector.Y + matrix.Column0[2] * vector.Z + matrix.Column0[3] * vector.W;
            result.Y = matrix.Column1[0] * vector.X + matrix.Column1[1] * vector.Y + matrix.Column1[2] * vector.Z + matrix.Column1[3] * vector.W;
            result.Z = matrix.Column2[0] * vector.X + matrix.Column2[1] * vector.Y + matrix.Column2[2] * vector.Z + matrix.Column2[3] * vector.W;
            result.W = matrix.Column3[0] * vector.X + matrix.Column3[1] * vector.Y + matrix.Column3[2] * vector.Z + matrix.Column3[3] * vector.W;
            return result;
        }

        /// <summary>
        /// 行列の掛け算
        /// </summary>
        /// <param name="matrix">マトリクス</param>
        /// <param name="vector">ベクトル</param>
        /// <returns>ベクトル</returns>
        public static Vector3 Multiply(Matrix3 matrix, Vector3 vector)
        {
            Vector3 result = new Vector3();
            result.X = matrix.Column0[0] * vector.X + matrix.Column0[1] * vector.Y + matrix.Column0[2] * vector.Z;
            result.Y = matrix.Column1[0] * vector.X + matrix.Column1[1] * vector.Y + matrix.Column1[2] * vector.Z;
            result.Z = matrix.Column2[0] * vector.X + matrix.Column2[1] * vector.Y + matrix.Column2[2] * vector.Z;

            //result.X = Round(result.X);
            //result.Y = Round(result.Y);
            //result.Z = Round(result.Z);

            return result;
        }
        #endregion
        #region [誤差判定]
        /// <summary>
        /// 誤差除去0~1
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>丸め値</returns>
        public static float Round(float value)
        {
            int tmp = (int)value;
            float result = value - tmp;
            //0.99999 or -0.99999
            if (1 - Math.Abs(result) < THRESHOLD05)
            {
                //0.99999
                if (result > 0)
                {
                    return tmp + 1;
                }
                else
                {
                    //-0.99999
                    return tmp - 1;
                }
            }

            //0.00001 or -0.00001
            if (Math.Abs(result) < THRESHOLD05)
            {
                return (float)tmp;
            }

            return value;
        }

        /// <summary>
        /// 誤差除去0~1
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>丸め値</returns>
        public static Vector3 Round(Vector3 value)
        {
            Vector3 ret = new Vector3();
            ret.X = Round(value.X);
            ret.Y = Round(value.Y);
            ret.Z = Round(value.Z);
            return ret;
        }

        #endregion

        /// <summary>
        /// 点と平面の距離
        /// http://keisan.casio.jp/exec/system/1202458240
        /// </summary>
        /// <param name="plane">平面</param>
        /// <param name="point">頂点</param>
        /// <returns></returns>
        public static float DistancePlane(float[] plane, Vector3 point)
        {
            float low = (float)Math.Sqrt((double)(plane[0] * plane[0] + plane[1] * plane[1] + plane[2] * plane[2]));
            float up = (float)Math.Abs((double)(plane[0] * point.X + plane[1] * point.Y + plane[2] * point.Z + plane[3]));
            return up / low;
        }

        /// <summary>
        /// 平面と点の距離
        /// </summary>
        /// <param name="plane">平面</param>
        /// <param name="point">点</param>
        /// <returns>距離</returns>
        public static float DistancePlane(Vector4 plane, Vector3 point)
        {
            float[] p = new float[4];
            p[0] = plane.X;
            p[1] = plane.Y;
            p[2] = plane.Z;
            p[3] = plane.W;
            return DistancePlane(p, point);
        }

        /// <summary>
        /// 線と線の距離
        /// </summary>
        /// <param name="start0">始点0</param>
        /// <param name="end0">終点0</param>
        /// <param name="start1">始点1</param>
        /// <param name="end1">終点1</param>
        /// <param name="distance">距離</param>
        /// <returns>成功</returns>
        public static bool DistanceLineToLine(Vector3 start0, Vector3 end0, Vector3 start1, Vector3 end1,out float distance)
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

            if (p43.Length < THRESHOLD05)
            {
                return false;
            }

            Vector3 p21 = p2 - p1;
            if (p21.Length < THRESHOLD05)
            {
                return false;
            }

            float d1343 = p13.X * p43.X + p13.Y * p43.Y + p13.Z * p43.Z;
            float d4321 = p43.X * p21.X + p43.Y * p21.Y + p43.Z * p21.Z;
            float d1321 = p13.X * p21.X + p13.Y * p21.Y + p13.Z * p21.Z;
            float d4343 = p43.X * p43.X + p43.Y * p43.Y + p43.Z * p43.Z;
            float d2121 = p21.X * p21.X + p21.Y * p21.Y + p21.Z * p21.Z;

            float denom = d2121 * d4343 - d4321 * d4321;
            if (Math.Abs(denom) < THRESHOLD05)
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

        /// <summary>
        /// GLのUnproject
        /// </summary>
        /// <param name="mosue">マウス位置、z=0or1,w=1</param>
        public static Vector3 UnProject(Vector3 mouse, Matrix4 cameraMatrix, Matrix4 projMatrix, int[] viewPort)
        {
            Vector4 projPos = new Vector4(mouse, 1.0f);
            Matrix4 modelproj = cameraMatrix * projMatrix;
            modelproj.Invert();
            Vector4 ret = new Vector4();

            projPos.X = (mouse.X - viewPort[0]) / (float)viewPort[2];
            projPos.Y = (mouse.Y - viewPort[1]) / (float)viewPort[3];

            projPos.X = (projPos.X * 2) - 1.0f;
            projPos.Y = (projPos.Y * 2) - 1.0f;
            projPos.Z = (projPos.Z * 2) - 1.0f;
            projPos.Y *= -1;
            projPos.W = 1.0f;

            ret = Vector4.Transform(projPos, modelproj);
            if (ret.W == 0.0f)
            {
                return Vector3.Zero;
            }

            ret.X /= ret.W;
            ret.Y /= ret.W;
            ret.Z /= ret.W;
            return new Vector3(ret.X, ret.Y, ret.Z);
        }

        /// <summary>
        /// Near面Far面からクリックした線分の取得
        /// </summary>
        public static void GetClickPos(Matrix4 cameraMatrix, Matrix4 projMatrix, int[] viewport, Vector2 mouse, out Vector3 near, out Vector3 far)
        {
            near = UnProject(new Vector3(mouse.X, mouse.Y, 0.0f), cameraMatrix, projMatrix, viewport);
            far = UnProject(new Vector3(mouse.X, mouse.Y, 1.0f), cameraMatrix, projMatrix, viewport);
        }

        /// <summary>
        /// ランダムの色値
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomColor()
        {
            Vector3 color = new Vector3();
            Random rand = new Random();
            color.X = rand.Next(255) / 255.0f;
            color.Y = rand.Next(255) / 255.0f;
            color.Z = rand.Next(255) / 255.0f;

            return color;
        }

        /// <summary>
        /// 平面と線分の交点
        /// </summary>
        /// <param name="surface">平面の公式</param>
        /// <param name="near">始点</param>
        /// <param name="far">終点</param>
        /// <returns>交点</returns>
        public static Vector3 CrossPoint(Vector4 surface, Vector3 near, Vector3 far)
        {
            Vector3 line = far - near;
            Vector3 result = new Vector3();

            result = KICalc.CrossPlanetoLine(near, far, surface);

            return result;
        }
    }
}
