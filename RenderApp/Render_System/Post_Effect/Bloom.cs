using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using KI.Gfx.KIAsset;
namespace RenderApp.Render_System
{
    public partial class Bloom : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"bloom.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"bloom.frag";
        
        /// <summary>
        /// 縦か横か
        /// </summary>
        private int _uHorizon;
        public int uHorizon
        {
            get
            {
                return _uHorizon;
            }
            set
            {
                if(value == 1 || value == 0)
                {
                    SetValue<int>(ref _uHorizon, value);
                }
            }
        }
        /// <summary>
        /// 縦か横か
        /// </summary>
        public void SetHorizon(bool horizon)
        {
            if (horizon)
            {
                uHorizon = 1;
            }
            else
            {
                uHorizon = 0;
            }
        }

        private float[] m_Weight = new float[5];

        public Bloom()
            : base("Bloom", vertexShader, fragShader)
        {

        }
        public override void Initialize()
        {
            float sigma = 100;
            float kei = (float)(1.0f / (2 * Math.PI * sigma * sigma));
            float sum = 0;
            float rad = 0;
            for (int i = 0; i < m_Weight.Length; i++)
            {
                rad = (1.0f + 2.0f * i) * (1.0f + 2.0f * i);
                m_Weight[i] = (float)Math.Exp(-0.5 * (rad / sigma));
                if (i != 0)
                {
                    sum += m_Weight[i] * 2;
                }
            }
            for (int i = 0; i < m_Weight.Length; i++)
            {
                m_Weight[i] /= (sum);
            }
        }
    }
}
