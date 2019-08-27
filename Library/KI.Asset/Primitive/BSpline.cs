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
            SetOpenUniformKnotVector();

            Create();
        }

        private void SetOpenUniformKnotVector()
        {
            knot = new float[ControlPoint.Length + Dimension + 1];
            for (int i = 0; i < Dimension + 1; i++)
            {
                knot[i] = 0;
                knot[knot.Length - 1 - i] = 1;
            }

            int remain = knot.Length - 2 * (Dimension + 1);
            float interval = 1.0f / (remain + 1);
            int index = 1;
            for (int i = Dimension + 1; i < knot.Length - Dimension - 1; i++)
            {
                knot[i] = interval * index;
                index++;
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
        /// <param name = "j" > ループインデックス </ param >
        /// < param name="k">次元</param>
        /// <param name = "parameter" > 媒介変数 </ param >
        /// < returns ></ returns >
        private float BasisFunction(int j, int k, float parameter)
        {
            if (k == 0)
            {
                if (knot[j] <= parameter && parameter < knot[j + 1])
                {
                    return 1;
                }
                else
                {
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

            return valueL + valueR;
        }

        /// <summary>
        /// Deboor BSplineの簡易実装
        /// </summary>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private Vector3 Deboor(int j, int k, float parameter)
        {
            if(k == 0)
            {
                return ControlPoint[j];
            }

            Vector3 valueL = Deboor(j - 1, k - 1, parameter);
            Vector3 valueR = Deboor(j, k - 1, parameter);

            float blend = (parameter - knot[j]) / (knot[j + Dimension + 1 - k] - knot[j]);
            return ((valueL) * (1 - blend) + (valueR * blend));
        }

        private void Create()
        {
            int tessNum = 100 - 1;
            List<Vector3> points = new List<Vector3>();
            List<int> lines = new List<int>();
            float interval = (knot[ControlPoint.Length] - knot[Dimension]) / tessNum;
            for (int i = 0; i <= tessNum; i++)
            {
                float parameter = knot[Dimension] + (i * interval);

                int curveSeg = -1;
                for (int j = Dimension; j < ControlPoint.Length + 2; j++)
                {
                    if (knot[j] <= parameter && parameter <= knot[j + 1])
                    {
                        curveSeg = j;
                        break;
                    }
                }

                if (curveSeg == -1)
                {
                    continue;
                }

                Vector3 point = Deboor(curveSeg, Dimension, parameter);
                points.Add(point);
            }

            lines.Add(0);
            for (int i = 1; i < points.Count; i++)
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
