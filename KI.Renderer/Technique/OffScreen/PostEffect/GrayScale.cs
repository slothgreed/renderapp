using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// GlayScale
    /// </summary>
    public partial class GrayScale : RenderTechnique
    {
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

        /// <summary>
        /// 閾値の最大値
        /// </summary>
        public float Max { get; set; } = 1;

        /// <summary>
        /// 閾値の最小値
        /// </summary>
        public float Min { get; set; } = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GrayScale(RenderSystem renderer, string vertexShader, string fragShader)
            : base("GrayScale", renderer, vertexShader, fragShader, RenderType.OffScreen)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uTarget = null;
        }
    }
}
