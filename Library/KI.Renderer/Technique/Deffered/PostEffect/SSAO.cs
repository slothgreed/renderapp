using System;
using KI.Gfx.KITexture;
using KI.Gfx.Buffer;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// SSAO
    /// </summary>
    public partial class SSAO : DefferedTechnique
    {
        private TextureBuffer _uPosition;
        public TextureBuffer uPosition
        {
            get
            {
                return _uPosition;
            }

            set
            {
                SetValue<TextureBuffer>(ref _uPosition, value);
            }
        }
        private TextureBuffer _uTarget;
        public TextureBuffer uTarget
        {
            get
            {
                return _uTarget;
            }

            set
            {
                SetValue<TextureBuffer>(ref _uTarget, value);
            }
        }
        private float[] _uSample;
        public float[] uSample
        {
            get
            {
                return _uSample;
            }

            set
            {
                SetValue<float[]>(ref _uSample, value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSAO(RenderSystem renderer, string vertexShader, string fragShader)
            : base("SSAO", renderer, vertexShader, fragShader)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uSample = new float[1200];
            float radius = 0.02f;//10.0f / m_ViewPort.w;
            Random rand = new Random();
            float[] val = new float[uSample.Length];

            for (int i = 0; i < val.Length; i += 3)
            {
                float r = (float)(radius * rand.NextDouble());
                float t = (float)(Math.PI * 2 * rand.NextDouble());
                float cp = (float)(2 * rand.NextDouble()) - 1.0f;
                float sp = (float)Math.Sqrt(1.0f - cp * cp);
                float ct = (float)Math.Cos(t);
                float st = (float)Math.Sin(t);
                val[i] = r * sp * ct;
                val[i + 1] = r * sp * st;
                val[i + 2] = r * cp;
            }

            uSample = val;
        }
    }
}
