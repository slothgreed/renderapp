using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// OpenTK.Matrix4 の Utilityクラス
    /// </summary>
    public static class Matrix4Utility
    {
        /// <summary>
        /// マトリクスを2次元配列に変換
        /// </summary>
        /// <param name="matrix">Matrix4</param>
        /// <returns>2次元配列</returns>
        public static float[,] To2dArray(Matrix4 matrix)
        {
            var arr = new float[4, 4];
            arr[0, 0] = matrix.M11; arr[1, 0] = matrix.M21; arr[2, 0] = matrix.M31; arr[3, 0] = matrix.M41;
            arr[0, 1] = matrix.M12; arr[1, 1] = matrix.M22; arr[2, 1] = matrix.M32; arr[3, 1] = matrix.M42;
            arr[0, 2] = matrix.M13; arr[1, 2] = matrix.M23; arr[2, 2] = matrix.M33; arr[3, 2] = matrix.M43;
            arr[0, 3] = matrix.M14; arr[1, 3] = matrix.M24; arr[2, 3] = matrix.M34; arr[3, 3] = matrix.M44;

            return arr;
        }

        /// <summary>
        /// 2次元配列を matrix に変換
        /// </summary>
        /// <param name="arr">2次元配列</param>
        /// <returns>Matrix4</returns>
        public static Matrix4 ToMatrix4(float[,] arr)
        {
            return new Matrix4(
                arr[0, 0], arr[0, 1], arr[0, 2], arr[0, 3],
                arr[1, 0], arr[1, 1], arr[1, 2], arr[1, 3],
                arr[2, 0], arr[2, 1], arr[2, 2], arr[2, 3],
                arr[3, 0], arr[3, 1], arr[3, 2], arr[3, 3]
                );
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
        public static Vector3 Multiply(Matrix4 matrix, Vector3 vector)
        {
            Vector3 result = new Vector3();
            result.X = matrix.Column0[0] * vector.X + matrix.Column0[1] * vector.Y + matrix.Column0[2] * vector.Z + matrix.Column0[3];
            result.Y = matrix.Column1[0] * vector.X + matrix.Column1[1] * vector.Y + matrix.Column1[2] * vector.Z + matrix.Column1[3];
            result.Z = matrix.Column2[0] * vector.X + matrix.Column2[1] * vector.Y + matrix.Column2[2] * vector.Z + matrix.Column2[3];

            return result;
        }

        /// <summary>
        /// Matrix4 に float を掛ける
        /// </summary>
        /// <param name="matrix">Matrix4</param>
        /// <param name="value">値</param>
        /// <returns>matrix4</returns>
        public static Matrix4 Multiply(Matrix4 matrix, float value)
        {
            return new Matrix4(
                    matrix.Row0 * value,
                    matrix.Row1 * value,
                    matrix.Row2 * value,
                    matrix.Row3 * value
                );
        }
    }
}
