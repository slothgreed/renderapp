using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// 最終出力用のバッファ
    /// </summary>
    public partial class OutputBuffer : RenderTechnique
    {
        private Texture _uSelectMap;
        public Texture uSelectMap
        {
            get
            {
                return _uSelectMap;
            }

            set
            {
                SetValue<Texture>(ref _uSelectMap, value);
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

        private Texture _uBackGround;
        public Texture uBackGround
        {
            get
            {
                return _uBackGround;
            }

            set
            {
                SetValue<Texture>(ref _uBackGround, value);
            }
        }

        private Texture _uForeGround;
        public Texture uForeGround
        {
            get
            {
                return _uForeGround;
            }

            set
            {
                SetValue<Texture>(ref _uForeGround, value);
            }
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutputBuffer(RenderSystem renderer, string vertexShader, string fragShader)
            : base("OutputBuffer", renderer, vertexShader, fragShader, RenderType.Forward)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uSelectMap = null;
            var textures = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            Rectanle.Polygon.Material.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
        }

        protected override void CreateRenderTarget(int width, int height)
        {
        }

        public override void SizeChanged(int width, int height)
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            //最終出力フレームバッファのバインドの必要なし
            Rectanle.Render(scene);
        }
    }
}
