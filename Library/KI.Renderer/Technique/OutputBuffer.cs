using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// 最終出力用のバッファ
    /// </summary>
    public partial class OutputBuffer : DefferedTechnique
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
            : base("OutputBuffer", renderer, vertexShader, fragShader)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uSelectMap = null;
            var textures = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            Rectangle.Polygon.Material.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="renderInfo">レンダリング情報</param>
        public override void Render(Scene scene, RenderInfo renderInfo)
        {
            //最終出力フレームバッファのバインドの必要なし
            Rectangle.Render(scene, renderInfo);
        }
    }
}
