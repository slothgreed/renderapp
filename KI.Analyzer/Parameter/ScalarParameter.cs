using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;

namespace KI.Analyzer
{
    /// <summary>
    /// 値のパラメータ
    /// </summary>
    public class ScalarParameter : IParameter
    {
        /// <summary>
        /// 曲率最大値最小値平均値分散確率密度関数
        /// </summary>
        private List<float> values = null;

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
        /// 最大値
        /// </summary>
        public float Max
        {
            get
            {
                if (maxValue == float.MaxValue)
                {
                    maxValue = values.Max();
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
                    minValue = values.Min();
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
                    float sum = values.Sum();
                    meanValue = sum / values.Count;
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
                    for (int i = 0; i < values.Count; i++)
                    {
                        valianceValue += (meanValue - values[i]) * (meanValue - values[i]);
                    }

                    valianceValue /= values.Count;
                }

                return valianceValue;
            }
        }

        /// <summary>
        /// パラメータを加える
        /// </summary>
        /// <param name="value">値</param>
        public void AddValue(object value)
        {
            if (values == null)
            {
                values = new List<float>();
            }

            values.Add((float)value);

            maxValue = float.MaxValue;
            minValue = float.MaxValue;
            meanValue = float.MaxValue;
            valianceValue = float.MaxValue;
        }

        /// <summary>
        /// 値の取得
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <returns>値</returns>
        public object GetValue(int index)
        {
            if (values.Count < index)
            {
                Logger.Log(Logger.LogLevel.Warning, "Not Found Value:" + index);
                return 0;
            }

            if (index < 0)
            {
                Logger.Log(Logger.LogLevel.Warning, "Not Found Value:" + index);
                return 0;
            }

            return values[index];
        }

        /// <summary>
        /// 確率密度関数の値
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>確率密度値</returns>
        public float GetPDF(float value)
        {
            float k = 1 / (float)Math.Sqrt((double)(2 * Math.PI * valianceValue));
            float e = -((value - meanValue) * (value - meanValue)) / (2 * valianceValue);
            return k * (float)Math.Exp((double)e);
        }
    }
}
