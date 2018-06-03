using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// 線形代数用
    /// </summary>
    public static class LinearAlgebra
    {
        /// <summary>
        /// 固有値固有ベクトルの算出
        /// </summary>
        /// <param name="matrix">行列</param>
        /// <param name="eivenVector">固有ベクトル</param>
        /// <param name="eigenValue">固有値</param>
        public static void  EigenValue(float[,] matrix, out float[,] eigenVector,out float[] eigenValue)
        {
            Mat cvMatrix = new Mat(matrix.GetLength(0), matrix.GetLength(1), MatType.CV_32FC1);

            cvMatrix.SetArray(0, 0, matrix[0, 0]);
            cvMatrix.SetArray(0, 1, matrix[0, 1]);
            cvMatrix.SetArray(1, 0, matrix[1, 0]);
            cvMatrix.SetArray(1, 1, matrix[1, 1]);

            Mat cvEigenValue = new Mat(1, 2, MatType.CV_32FC1);
            Mat cvEigenVector = new Mat(2, 2, MatType.CV_32FC1);

            Cv2.Eigen(cvMatrix, cvEigenValue, cvEigenVector);

            eigenVector = Convert2dMatrix(cvMatrix);
            eigenValue = ConvertValues(cvEigenValue);
        }

        /// <summary>
        /// 2次元配列に変換
        /// </summary>
        /// <param name="cvMatrix">cvのマトリクス</param>
        /// <returns>2次元の配列</returns>
        private static float[,] Convert2dMatrix(Mat cvMatrix)
        {
            float[,] matrix = new float[cvMatrix.Width, cvMatrix.Height];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = cvMatrix.Get<float>(i, j);
                }
            }

            return matrix;
        }

        /// <summary>
        /// 値の変換
        /// </summary>
        /// <param name="cvMat">cvのマトリクス</param>
        /// <returns>値</returns>
        private static float[] ConvertValues(Mat cvMat)
        {
            var values = new float[2];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = cvMat.Get<float>(0, i);
            }

            return values;
        }
    }
}
