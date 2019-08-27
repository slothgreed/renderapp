using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Mathmatics
{
    /// <summary>
    /// OpenTK.Matrix3 の Utilityクラス
    /// </summary>
    public static class Matrix3Utility
    {
        /// <summary>
        /// マトリクスを2次元配列に変換
        /// // TODO: Matrix4Extension の実装
        /// </summary>
        /// <param name="matrix">Matrix4</param>
        /// <returns>2次元配列</returns>
        public static float[,] To2dArray(Matrix3 matrix)
        {
            var arr = new float[4, 4];
            arr[0, 0] = matrix.M11; arr[1, 0] = matrix.M21; arr[2, 0] = matrix.M31;
            arr[0, 1] = matrix.M12; arr[1, 1] = matrix.M22; arr[2, 1] = matrix.M32;
            arr[0, 2] = matrix.M13; arr[1, 2] = matrix.M23; arr[2, 2] = matrix.M33;

            return arr;
        }

        /// <summary>
        /// 2次元配列を matrix に変換
        /// </summary>
        /// <param name="arr">2次元配列</param>
        /// <returns>Matrix3</returns>
        public static Matrix3 ToMatrix3(float[,] arr)
        {
            return new Matrix3(
                arr[0, 0], arr[0, 1], arr[0, 2],
                arr[1, 0], arr[1, 1], arr[1, 2],
                arr[2, 0], arr[2, 1], arr[2, 2]
                );
        }

        /// <summary>
        /// Matrix3 を Matrix4 に変換する
        /// </summary>
        /// <param name="matrix">Matrix3</param>
        /// <returns>matrix4</returns>
        public static Matrix4 ToMatrix4(Matrix3 matrix)
        {
            return new Matrix4(
                matrix.M11, matrix.M12, matrix.M13, 0,
                matrix.M21, matrix.M22, matrix.M23, 0,
                matrix.M31, matrix.M32, matrix.M33, 0,
                0, 0, 0, 1
                );
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
            return result;
        }

        /// <summary>
        /// Matrix3 に float を掛ける
        /// </summary>
        /// <param name="matrix">Matrix4</param>
        /// <param name="value">値</param>
        /// <returns>Matrix3</returns>
        public static Matrix3 Multiply(Matrix3 matrix, float value)
        {
            return new Matrix3(
                    matrix.Row0 * value,
                    matrix.Row1 * value,
                    matrix.Row2 * value
                );
        }
    }
}
