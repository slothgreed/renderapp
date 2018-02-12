using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;

namespace KI.Foundation.Parameter
{
    /// <summary>
    /// 値のパラメータ
    /// </summary>
    public class ScalarParameter : IParameter
    {
        /// <summary>
        /// 最大値
        /// </summary>
        private float maxValue = float.MaxValue;

        /// <summary>
        /// 最小値
        /// </summary>
        private float minValue = float.MaxValue;

        /// <summary>
        /// 相加平均
        /// </summary>
        private float meanValue = float.MaxValue;

        /// <summary>
        /// σ^2
        /// </summary>
        private float valianceValue = float.MaxValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="parameters">パラメータ</param>
        public ScalarParameter(string name, float[] parameters)
        {
            Name = name;
            Values = parameters;
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 値
        /// </summary>
        public float[] Values { get; private set; }

        /// <summary>
        /// 最大値
        /// </summary>
        public float Max
        {
            get
            {
                if (maxValue == float.MaxValue)
                {
                    maxValue = Values.Max();
                }

                return maxValue;
            }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        public float Min
        {
            get
            {
                if (minValue == float.MaxValue)
                {
                    minValue = Values.Min();
                }

                return minValue;
            }
        }

        /// <summary>
        /// 平均値
        /// </summary>
        public float Mean
        {
            get
            {
                if (meanValue == float.MaxValue)
                {
                    float sum = Values.Sum();
                    meanValue = sum / Values.Length;
                }

                return meanValue;
            }
        }

        /// <summary>
        /// σ^2
        /// </summary>
        public float Valiance
        {
            get
            {
                if (valianceValue == float.MaxValue)
                {
                    foreach (var val in Values)
                    {
                        valianceValue += (meanValue - val) * (meanValue - val);
                    }

                    valianceValue /= Values.Length;
                }

                return valianceValue;
            }
        }

        /// <summary>
        /// 値の取得
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <returns>値</returns>
        public float GetValue(int index)
        {
            if (Values.Length < index)
            {
                Logger.Log(Logger.LogLevel.Warning, "Not Found Value:" + index);
                return 0;
            }

            if (index < 0)
            {
                Logger.Log(Logger.LogLevel.Warning, "Not Found Value:" + index);
                return 0;
            }

            return Values[index];
        }

        /// <summary>
        /// 確率密度関数の値
        /// </summary>
        /// <param name="value">要素番号</param>
        /// <returns>確率密度値</returns>
        public float GetPDF(int index)
        {
            float value = (float)GetValue(index);
            float k = 1 / (float)Math.Sqrt((double)(2 * Math.PI * Valiance));
            float e = -((value - Mean) * (value - Mean)) / (2 * Valiance);
            return k * (float)Math.Exp((double)e);
        }
    }
}
