using System;
namespace KI.Renderer
{
    public partial class Bloom : RenderTechnique
    {
        private static string vertexShader = Global.ShaderDirectory + @"\PostEffect\bloom.vert";
        private static string fragShader = Global.ShaderDirectory + @"\PostEffect\bloom.frag";
        
        private float[] m_Weight = new float[5];

        public Bloom(RenderTechniqueType tech)
            : base("Bloom", vertexShader, fragShader, tech, RenderType.OffScreen)
        {
        }
        public override void Initialize()
        {
            uHorizon = true;
            uScale = 100;
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
            uWeight = m_Weight;
        }
    }
}
