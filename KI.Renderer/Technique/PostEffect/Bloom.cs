using System;

namespace KI.Renderer
{
    public partial class Bloom : RenderTechnique
    {
        private static string vertexShader = Global.ShaderDirectory + @"\PostEffect\bloom.vert";
        private static string fragShader = Global.ShaderDirectory + @"\PostEffect\bloom.frag";
        
        private float[] weight = new float[5];

        public Bloom(RenderTechniqueType tech)
            : base("Bloom", vertexShader, fragShader, tech, RenderType.OffScreen)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uHorizon = true;
            uScale = 100;
            float sigma = 100;
            float kei = (float)(1.0f / (2 * Math.PI * sigma * sigma));
            float sum = 0;
            float rad = 0;
            for (int i = 0; i < weight.Length; i++)
            {
                rad = (1.0f + 2.0f * i) * (1.0f + 2.0f * i);
                weight[i] = (float)Math.Exp(-0.5 * (rad / sigma));
                if (i != 0)
                {
                    sum += weight[i] * 2;
                }
            }

            for (int i = 0; i < weight.Length; i++)
            {
                weight[i] /= (sum);
            }

            uWeight = weight;
        }
    }
}
