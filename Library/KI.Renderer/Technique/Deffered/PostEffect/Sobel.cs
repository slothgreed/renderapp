using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// Sobel
    /// </summary>
    public partial class Sobel : DefferedTechnique
    {
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
        private int _uWidth;
        public int uWidth
        {
            get
            {
                return _uWidth;
            }

            set
            {
                SetValue<int>(ref _uWidth, value);
            }
        }
        private int _uHeight;
        public int uHeight
        {
            get
            {
                return _uHeight;
            }

            set
            {
                SetValue<int>(ref _uHeight, value);
            }
        }
        private float _uThreshold;
        public float uThreshold
        {
            get
            {
                return _uThreshold;
            }

            set
            {
                SetValue<float>(ref _uThreshold, value);
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
        public Sobel(RenderSystem renderer, string vertexShader, string fragShader)
            : base("Sobel", renderer, vertexShader, fragShader)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uThreshold = 0.001f;
            uWidth = 1;
            uHeight = 1;
            uTarget = null;
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public override void SizeChanged(int width, int height)
        {
            base.SizeChanged(width, height);
            uWidth = width;
            uHeight = height;
        }
    }
}
