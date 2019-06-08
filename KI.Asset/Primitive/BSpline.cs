using KI.Foundation.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Primitive
{
    public class BSpline
    {
        /// <summary>
        /// 位置情報(GL_LINES)
        /// </summary>
        public Vector3[] Position;

        /// <summary>
        /// コントロールポイント
        /// </summary>
        public Vector3[] ControlPoint;

        /// <summary>
        /// インデックス情報
        /// </summary>
        public int[] Index;

        /// <summary>
        /// ノットベクトル
        /// </summary>
        private float[] knot;

        /// <summary>
        /// 横の数
        /// </summary>
        public int UNum { get; private set; }

        /// <summary>
        /// 縦の数(1以上の場合サーフェスになる)
        /// </summary>
        public int VNum { get; private set; }

        /// <summary>
        /// 次数
        /// </summary>
        public int Dimension { get; private set; }

        /// <summary>
        /// 面かどうか
        /// </summary>
        public bool Surface { get { return VNum > 1; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controlPoint">制御点</param>
        /// <param name="dimension">次数</param>
        public BSpline(Vector3[] controlPoint, int dimension)
        {
            ControlPoint = controlPoint;
            Dimension = dimension;
            SetUniformKnotVector();
            for (int i = 0; i < knot.Length; i++)
            {
                knot[i] = i;
            }

            Create();
        }

        /// <summary>
        /// 開一様ノットベクトル
        /// </summary>
        private void SetOpenUniformKnotVector()
        {
            knot = new float[ControlPoint.Length + Dimension + 1];
            for (int i = 0; i < Dimension + 1; i++)
            {
                knot[i] = 0;
            }

            // 0と1以外の要素数
            int durNum = knot.Length - 2 * (Dimension + 1);
            float value = 1.0f / (durNum + 1); // 0 < value < 1; 
            for (int i = Dimension + 1; i < knot.Length - Dimension - 1; i++)
            {
                knot[i] = value;
            }

            for (int i = knot.Length - Dimension - 1; i < knot.Length; i++)
            {
                knot[i] = 1;
            }
        }

        /// <summary>
        /// 閉一様ノットベクトル
        /// </summary>
        private void SetUniformKnotVector()
        {
            knot = new float[ControlPoint.Length + Dimension + 1];
            knot[0] = 0;
            knot[knot.Length - 1] = 1;
            for (int i = 1; i < knot.Length -1; i++)
            {
                knot[i] = knot[0] + (knot[knot.Length - 1] - knot[0]) * i / (knot.Length - 1);
            }
        }

        /// <summary>
        /// Bスプライン基底関数
        /// </summary>
        /// <param name="j">ループインデックス</param>
        /// <param name="k">次元</param>
        /// <param name="parameter">媒介変数</param>
        /// <returns></returns>
        private float BasisFunction(int j, int k, float parameter)
        {
            if (k == 0)
            {
                if (knot[j] <= parameter && parameter < knot[j + 1])
                {
                    Logger.Log(Logger.LogLevel.Allway, j.ToString() + ":" + k.ToString() + "(1)");
                    return 1;
                }
                else
                {
                    Logger.Log(Logger.LogLevel.Allway, j.ToString() + ":" + k.ToString() + "(0)");
                    return 0;
                }
            }

            float valueL = 0;
            float valueR = 0;

            float denomL = knot[j + k] - knot[j];
            float denomR = knot[j + k + 1] - knot[j + 1];
            if (denomL != 0)
                valueL = (parameter - knot[j]) / denomL;

            if (denomR != 0)
                valueR = (knot[j + k + 1] - parameter) / denomR;

            valueL *= BasisFunction(j, k - 1, parameter);
            valueR *= BasisFunction(j + 1, k - 1, parameter);

            Logger.Log(Logger.LogLevel.Allway, j.ToString() + ":" + k.ToString() + ":(" + valueL.ToString() + "," + valueR.ToString() + ")");
            return valueL + valueR;
        }

        double BasisFunction2(int i, int k, double t)
        {
            double w1 = 0.0, w2 = 0.0;
            if (k == 1)
            {
                if (t >= knot[i] && t < knot[i + 1]) return 1.0;
                else return 0.0;
            }
            else
            {
                if ((knot[i + k] - knot[i + 1]) != 0)
                {
                    w1 = ((knot[i + k] - t) / (knot[i + k] - knot[i + 1])) * BasisFunction2(i + 1, k - 1, t);
                }
                if ((knot[i + k - 1] - knot[i]) != 0)
                {
                    w2 = ((t - knot[i]) / (knot[i + k - 1] - knot[i])) * BasisFunction2(i, k - 1, t);
                }
                return (w1 + w2);
            }
        }

        //private Vector3 ThreeDimension(float t)
        //{
        //    float a = (1 - t) * (1 - t) * (1 - t);
        //    float b = (0.5f * t * t * t) + (t * t) + (2.0f / 3.0f);
        //    float c = ((-0.5f) * t * t * t) + (0.5f * t * t) + (0.5f * t) + (1.0f / 6.0f);
        //    float d = (1.0f / 6.0f) * t * t * t;

        //    return ControlPoint[0] * a + ControlPoint[1] * b + ControlPoint[2] * c + ControlPoint[3] * d;
        //}

        //private Vector3 TwoDimension(float t)
        //{
        //    float a = (1 - t) * (1 - t);
        //    float b = 2 * (1 - t);
        //    float c = t * t;

        //    return ControlPoint[0] * a + ControlPoint[1] * b + ControlPoint[2] * c;
        //}

        private Vector3 Deboor(int j, int k, float parameter)
        {
            if(k == 0)
            {
                return ControlPoint[j];
            }

            Vector3 valueL = Deboor(j - 1, k - 1, parameter);
            Vector3 valueR = Deboor(j, k - 1, parameter);

            float blend = ((parameter - knot[j]) / knot[j + Dimension - k] - knot[j]);
            return ((valueL) * (1 - blend) + (valueR * blend));
        }

        private void Create()
        {
            List<Vector3> points = new List<Vector3>();
            List<int> lines = new List<int>();
            for (float p = 0.01f; p < 1; p += 0.1f)
            {
                Vector3 point = new Vector3();
                for (int i = 0; i < ControlPoint.Length; i++)
                {
                    //float val = BasisFunction(i, Dimension, p);
                    //point += ControlPoint[i] * val;
                    point = Deboor(i, Dimension, p);
                    Logger.Log(Logger.LogLevel.Allway, "");
                }

                points.Add(point);
            }

            //points.Clear();
            //for (float j = 0; j <= 7 * 10; j++)
            //{
            //    float t = j / 10.0f;
            //    if (j == 70)
            //    {
            //        t -= 0.001f;
            //    }

            //    Vector3 point = new Vector3();
            //    for (int i = 0; i <= 4; i++)
            //    {
            //        point += ControlPoint[i] * (float)BasisFunction2(i, Dimension, t);
            //    }

            //    points.Add(point);
            //}

            lines.Add(0);
            for(int i = 1; i < points.Count; i++)
            {
                lines.Add(i);
                lines.Add(i);
            }
            lines.Add(points.Count - 1);

            Position = points.ToArray();
            Index = lines.ToArray();
        }
    }
}
