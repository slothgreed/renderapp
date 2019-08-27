using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// OpenTK.Matrix3 の Utilityクラス
    /// </summary>
    public static class Vector3Utility
    {
        /// <summary>
        /// ベクトルと転置ベクトルの掛け算
        /// </summary>
        /// <param name="vectorA">ベクトル1</param>
        /// <param name="vectorB">ベクトル2(内部で転置される)</param>
        public static Matrix3 MultipleByTranspose(Vector3 vectorA, Vector3 vectorB)
        {
            return new Matrix3(
                vectorB * vectorA.X, // ax * bx , ax * by , ax * bz
                vectorB * vectorA.Y, // ay * bx , ay * by , ay * bz
                vectorB * vectorA.Z  // az * bx , az * by , az * bz
                );
        }
    }
}
