using System;

namespace KI.Renderer
{
    /// <summary>
    /// bloom
    /// </summary>
    public partial class Bloom : RenderTechnique
    {
        /// <summary>
        /// 頂点シェーダ
        /// </summary>
        private static string vertexShader = Global.ShaderDirectory + @"\PostEffect\bloom.vert";

        /// <summary>
        /// フラグシェーダ
        /// </summary>
        private static string fragShader = Global.ShaderDirectory + @"\PostEffect\bloom.frag";
        
        /// <summary>
        /// 重み
        /// </summary>
        private float[] weight = new float[5];

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Bloom()
            : base("Bloom", vertexShader, fragShader, RenderTechniqueType.Bloom, RenderType.OffScreen)
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
                weight[i] /= sum;
            }

            uWeight = weight;
        }
    }
}
