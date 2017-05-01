using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;

namespace KI.Gfx.Analyzer
{
    public class ScalarParameter : IParameter
    {
        /// <summary>
        /// 曲率最大値最小値平均値分散確率密度関数
        /// </summary>
        public List<float> m_value = null;
        private float m_Max = float.MinValue;
        private float m_Min = float.MaxValue;
        private float m_Mean = 0;       //相加平均
        private float m_Valiance = 0;   //σ^2

        public float Max
        {
            get
            {
                if(m_Max == float.MinValue)
                {
                    m_Max = m_value.Max();
                }
                return m_Max;
            }
        }
        public float Min
        {
            get
            {
                if(m_Min == float.MaxValue)
                {
                    m_Min = m_value.Min();
                }
                return m_Min;
            }
        }
        public float Mean
        {
            get
            {
                if(m_Mean == 0)
                {
                    float sum = m_value.Sum();
                    m_Mean = sum / m_value.Count;
                }
                return m_Mean;
            }
        }
        public float Valiance
        {
            get
            {
                if(m_Valiance == 0)
                {
                    for (int i = 0; i < m_value.Count; i++)
                    {
                        m_Valiance += (m_Mean - m_value[i]) * (m_Mean - m_value[i]);
                    }
                    m_Valiance /= m_value.Count;
                }
                return m_Valiance;
            }
        }
        /// <summary>
        /// パラメータの数
        /// </summary>
        public int m_Num;
        /// <summary>
        /// パラメータを加える
        /// </summary>
        /// <param name="value"></param>
        public void AddValue(object value)
        {
            if(m_value == null)
            {
                m_value = new List<float>();
            }
            m_value.Add((float)value);
        }
        public object GetValue(int index)
        {
            if(m_value.Count < index)
            {
                Logger.Log(Logger.LogLevel.Warning, "Not Found Value:" + index);
                return 0;
            }
            if(index < 0 )
            {
                Logger.Log(Logger.LogLevel.Warning, "Not Found Value:" + index); 
                return 0;
            }
            return m_value[index];            
        }
        /// <summary>
        /// 確率密度関数の値
        /// </summary>
        public float GetPDF(float value)
        {
            float k = 1/(float)Math.Sqrt((double)(2 * Math.PI * m_Valiance));
            float e = -((value - m_Mean) * (value - m_Mean)) / (2 * m_Valiance);
            return k * (float)Math.Exp((double)e);
        }

    }
}
