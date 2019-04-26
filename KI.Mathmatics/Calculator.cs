using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// 計算
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// 誤差範囲 0.00001
        /// </summary>
        public static float THRESHOLD05 { get; set; } = 0.00001f;

        /// <summary>
        /// 頂点リストから、最小値最大値を算出
        /// </summary>
        /// <param name="position">頂点</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public static void MinMax(IEnumerable<Vector3> position, out Vector3 min, out Vector3 max)
        {
            if (!position.Any())
            {
                min = Vector3.Zero;
                max = Vector3.Zero;
                return;
            }

            min = position.First();
            max = position.First();
            foreach (var pos in position)
            {
                min = Vector3.ComponentMin(min, pos);
                max = Vector3.ComponentMax(max, pos);
            }
        }

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

        /// <summary>
        /// 法線の算出
        /// </summary>
        /// <param name="vertex1">ベクトル1</param>
        /// <param name="vertex2">ベクトル2</param>
        /// <param name="vertex3">ベクトル3</param>
        /// <returns>法線</returns>
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
        /// 二項係数の計算
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        public static int BinomalCoefficients(int n, int k)
        {
            int a = Factorial(n);
            int b = Factorial(k) * Factorial(n - k);
            return a / b;
        }

        /// <summary>
        /// 階乗の計算
        /// </summary>
        /// <param name="num">値</param>
        /// <returns>戻り値</returns>
        public static int Factorial(int num)
        {
            int value = 1;
            for (int i = 1; i <= num; i++)
            {
                value = value * i;
            }

            return value;
        }


        /// <summary>
        /// バーンスタイン基底関数
        /// </summary>
        /// <param name="n">次数</param>
        /// <param name="v">v</param>
        /// <param name="x">x</param>
        public static float Bernstein(int n, int v, float x)
        {
            return (float)(BinomalCoefficients(n, v) * Math.Pow(x, v) * Math.Pow(1 - x, n - v));
        }
    }
}