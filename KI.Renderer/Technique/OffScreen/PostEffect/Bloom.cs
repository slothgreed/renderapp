using System;
using KI.Gfx.KITexture;

namespace KI.Asset.Technique
{
    /// <summary>
    /// bloom
    /// </summary>
    public partial class Bloom : RenderTechnique
    {
        /// <summary>
        /// 重み
        /// </summary>
        private float[] weight = new float[5];

        /// <summary>
        /// Scale最小値
        /// </summary>
        public int Min { get; set; } = 0;

        /// <summary>
        /// Scale最大値
        /// </summary>
        public int Max { get; set; } = 1;

        /// <summary>
        /// 重みの値
        /// </summary>
        public float Sigma { get; set; } = 1;

        private float _uScale;
        public float uScale
        {
            get
            {
                return _uScale;
            }

            set
            {
                SetValue<float>(ref _uScale, value);
            }
        }
        private float[] _uWeight;
        public float[] uWeight
        {
            get
            {
                return _uWeight;
            }

            set
            {
                SetValue<float[]>(ref _uWeight, value);
            }
        }
        private Texture _uTarget;
        public Texture uTarget
        {
            get
            {
                return _uTarget;
            }

            set
            {
                SetValue<Texture>(ref _uTarget, value);
            }
        }
        private bool _uHorizon;
        public bool uHorizon
        {
            get
            {
                return _uHorizon;
            }

            set
            {
                SetValue<bool>(ref _uHorizon, value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Bloom(string vertexShader, string fragShader)
            : base("Bloom", vertexShader, fragShader, RenderType.OffScreen)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uHorizon = true;

            uScale = 100;
            UpdateWeight();
        }

        /// <summary>
        /// 重みの更新
        /// </summary>
        public void UpdateWeight()
        {
            float kei = (float)(1.0f / (2 * Math.PI * Sigma * Sigma));
            float sum = 0;
            float rad = 0;
            for (int i = 0; i < weight.Length; i++)
            {
                rad = (1.0f + 2.0f * i) * (1.0f + 2.0f * i);
                weight[i] = (float)Math.Exp(-0.5 * (rad / Sigma));
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
