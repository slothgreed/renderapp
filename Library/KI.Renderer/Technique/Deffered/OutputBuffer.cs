using KI.Gfx;
using KI.Gfx.GLUtil;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// 最終出力用のバッファ
    /// </summary>
    public partial class OutputBuffer : DefferedTechnique
    {
        private TextureBuffer _uSelectMap;
        public TextureBuffer uSelectMap
        {
            get
            {
                return _uSelectMap;
            }

            set
            {
                SetValue<TextureBuffer>(ref _uSelectMap, value);
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

        private TextureBuffer _uBackGround;
        public TextureBuffer uBackGround
        {
            get
            {
                return _uBackGround;
            }

            set
            {
                SetValue<TextureBuffer>(ref _uBackGround, value);
            }
        }

        private TextureBuffer _uForeGround;
        public TextureBuffer uForeGround
        {
            get
            {
                return _uForeGround;
            }

            set
            {
                SetValue<TextureBuffer>(ref _uForeGround, value);
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
