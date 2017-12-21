using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
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
            : base("OutputBuffer", vertexShader, fragShader, RenderTechniqueType.Output, RenderType.Original)
        {
            Plane = RenderObjectFactory.Instance.CreateRenderObject("OutputBuffer", AssetFactory.Instance.CreatePlane("OutputPlane"));
            var textures = Global.RenderSystem.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            Plane.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uSelectMap = null;
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
        public override void Render(IScene scene)
        {
            //最終出力フレームバッファのバインドの必要なし
            Plane.Shader = ShaderItem;
            Plane.Render(scene);
        }
    }
}
