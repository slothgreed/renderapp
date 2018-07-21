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
        public static void EigenValue(float[,] matrix, out float[,] eigenVector, out float[] eigenValue)
        {
            Mat cvMatrix = Convert2dCvMatrix(matrix);

            Mat cvEigenValue = new Mat(1, 2, MatType.CV_32FC1);
            Mat cvEigenVector = new Mat(2, 2, MatType.CV_32FC1);

            Cv2.Eigen(cvMatrix, cvEigenValue, cvEigenVector);

            eigenValue = Convert1dMatrix(cvEigenValue);
            eigenVector = Convert2dMatrix(cvEigenVector);
        }

        /// <summary>
        /// 特異値分解
        /// </summary>
        public static void SVD(float[,] matrix, out float[] resultW, out float[] resultU, out float[,] resultV)
        {
            Mat cvMatrix = Convert2dCvMatrix(matrix);
            OutputArray w = OutputArray.Create(new Mat(1, matrix.GetLength(0), MatType.CV_32FC1));
            OutputArray u = OutputArray.Create(new Mat(1, matrix.GetLength(1), MatType.CV_32FC1));
            OutputArray v = OutputArray.Create(new Mat(matrix.GetLength(1), matrix.GetLength(1), MatType.CV_32FC1));

            OpenCvSharp.SVD.Compute(cvMatrix, w, u, v, OpenCvSharp.SVD.Flags.FullUV);

            resultW = Convert1dMatrix(w.GetMat());
            resultU = Convert1dMatrix(u.GetMat());
            resultV = Convert2dMatrix(v.GetMat());

            // 誤差吸収
            float wmax = 0;
            for (int i = 0; i < resultW.Length; i++)
            {
                wmax = Math.Max(resultW[i], wmax);
            }

            var wmin = wmax * Calculator.THRESHOLD05;
            for (int i = 0; i < resultW.Length; i++)
            {
                if (resultW[i] < wmin)
                {
                    resultW[i] = 0;
                }
            }

        }

        /// <summary>
        /// 特異値の後退代入
        /// </summary>
        /// <param name="W">特異値の行列またはベクトル</param>
        /// <param name="U">特異値の左直交行列</param>
        /// <param name="V">特異値の右直交行列</param>
        /// <param name="B">行列Aの擬似逆行列に乗ずるための行列</param>
        /// <param name="X">出力行列．後退代入の結果．</param>
        public static void SVBKSB(float[,] u, float[] w, float[,] v, float[] b, out float[] x)
        {
            // opencv にないので自前実装
            float sum = 0;
            var tmp = new float[u.GetLength(1) + 1];
            x = new float[u.GetLength(1)];
            for (int j = 0; j < u.GetLength(1); j++)
            {
                sum = 0;
                if (w[j] != 0)
                {
                    for(int i = 0; i < u.GetLength(0); i++)
                    {
                        sum += u[i, j] * b[i];
                    }

                    sum /= w[j];
                }

                tmp[j] = sum;
            }

            for (int j = 0; j < u.GetLength(1); j++)
            {
                sum = 0;
                for (int jj = 0; jj < u.GetLength(1); jj++)
                {
                    sum += v[j, jj] * tmp[jj];
                }

                x[j] = sum;
            }
        }

        /// <summary>
        /// 2次元配列をcvMatに変換
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static Mat Convert2dCvMatrix(float[,] matrix)
        {
            Mat cvMatrix = new Mat(matrix.GetLength(0), matrix.GetLength(1), MatType.CV_32FC1);

            for (int i = 0; i < cvMatrix.Width; i++)
            {
                for (int j = 0; j < cvMatrix.Height; j++)
                {
                    cvMatrix.SetArray(j, i, matrix[j, i]);
                }
            }

            return cvMatrix;
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
        /// 1次元配列をcvMatに変換
        /// </summary>
        /// <param name="cvMat">cvのマトリクス</param>
        /// <returns>値</returns>
        private static float[] Convert1dMatrix(Mat cvMat)
        {
            float[] matrix;

            if (cvMat.Rows != 1)
            {
                matrix = new float[cvMat.Rows];
            }
            else
            {
                matrix = new float[cvMat.Cols];
            }
            
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = cvMat.Get<float>(0, i);
            }

            return matrix;
        }
    }
}
