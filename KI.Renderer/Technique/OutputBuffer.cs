using KI.Asset;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// 最終出力用のバッファ
    /// </summary>
    public partial class OutputBuffer : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutputBuffer(string vertexShader, string fragShader)
            : base("OutputBuffer", RenderTechniqueType.Output, RenderType.Original)
        {
            Plane = RenderObjectFactory.Instance.CreateRenderObject("OutputBuffer", AssetFactory.Instance.CreatePlane("OutputPlane"));
            Plane.Shader = ShaderFactory.Instance.CreateShaderVF(vertexShader, fragShader, ShaderStage.PostEffect);

            var textures = Global.Renderer.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            Plane.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uSelectMap = null;
        }

        public Texture uSelectMap
        {
            get
            {
                return Plane.Shader.GetValue(nameof(uSelectMap)) as Texture;
            }

            set
            {
                Plane.Shader.SetValue(nameof(uSelectMap), value);
            }
        }
        public Texture uTarget
        {
            get
            {
                return Plane.Shader.GetValue(nameof(uTarget)) as Texture;
            }

            set
            {
                Plane.Shader.SetValue(nameof(uTarget), value);
            }
        }

        /// <summary>
        /// 描画テクスチャの設定
        /// </summary>
        /// <param name="textureKind">テクスチャ種類</param>
        /// <param name="outputTexture">出力テクスチャ</param>
        public void SetOutputTarget(TextureKind textureKind, Texture outputTexture)
        {
            Plane.Polygon.AddTexture(textureKind, outputTexture);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            //最終出力フレームバッファのバインドの必要なし
            Plane.Render(scene);
        }
    }
}
