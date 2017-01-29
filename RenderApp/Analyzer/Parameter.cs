using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;

namespace RenderApp.Analyzer
{
    public class Parameter
    {
        /// <summary>
        /// 曲率最大値最小値平均値分散確率密度関数
        /// </summary>
        public List<float> m_value = null;
        private float m_Max = float.MinValue;
        private float m_Min = float.MaxValue;
        private float m_Mean = 0;       //相加平均
        private float m_Valiance = 0;   //σ^2

        public float Max { get { return m_Max; } }
        public float Min { get { return m_Min; } }
        public float Mean { get { return m_Mean; } }
        public float Valiance { get { return m_Valiance; } }
        /// <summary>
        /// パラメータの数
        /// </summary>
        public int m_Num;
        /// <summary>
        /// パラメータのセット
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(List<float> value)
        {
            m_value = value;
            m_value.Sort();
        }
        /// <summary>
        /// パラメータを加える
        /// </summary>
        /// <param name="value"></param>
        public void AddValue(float value)
        {
            if(m_value == null)
            {
                m_value = new List<float>();
            }
            m_value.Add(value);
        }
        
        /// <summary>
        /// 最大値最小値平均
        /// </summary>
        /// <param name="m_value"></param>
        public void SetMaxMinMean()
        {
            if(m_value == null)
            {
                Console.WriteLine("don't set parameter value");
                return;
            }
            for (int i = 0; i < m_value.Count; i++)
            {
                if(m_Max < m_value[i])
                {
                    m_Max = m_value[i];
                }
                if(m_Min > m_value[i])
                {
                    m_Min = m_value[i];
                }

                m_Mean += m_value[i];
            }
            m_Mean /= m_value.Count;
        }

        /// <summary>
        /// 分散
        /// </summary>
        public void SetValiance()
        {
            if(m_Mean == 0)
            {
                Logger.Log(Logger.LogLevel.Warning, "don't calc mean value");
                return;
            }
            for(int i = 0; i < m_value.Count; i++)
            {
                m_Valiance += (m_Mean - m_value[i]) * (m_Mean - m_value[i]);
            }
            m_Valiance /= m_value.Count;
            
        }
        /// <summary>
        /// 確率密度関数の値
        /// </summary>
        public float GetPDF(float value)
        {
            if(m_Valiance == 0)
            {
                Logger.Log(Logger.LogLevel.Warning, "don't calc valiace value");
                return 0;
            }
            float k = 1/(float)Math.Sqrt((double)(2 * Math.PI * m_Valiance));
            float e = -((value - m_Mean) * (value - m_Mean)) / (2 * m_Valiance);
            return k * (float)Math.Exp((double)e);
        }
        
       
        
    }
}
