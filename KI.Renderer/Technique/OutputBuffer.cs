using System.Runtime.CompilerServices;
using KI.Asset;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;

namespace KI.Asset.Technique
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

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutputBuffer(string vertexShader, string fragShader)
            : base("OutputBuffer", vertexShader, fragShader, RenderType.Original)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uSelectMap = null;
            var textures = Global.Renderer.RenderQueue.OutputTexture<GBuffer>();
            Rectanle.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
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
