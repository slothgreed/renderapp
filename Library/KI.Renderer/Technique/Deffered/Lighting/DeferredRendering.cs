using KI.Gfx;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// Deferred Rendering
    /// </summary>
    public class DeferredRendering : DefferedTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeferredRendering(RenderSystem renderer, string vertexShader, string fragShader)
            : base("Deferred", renderer, vertexShader, fragShader)
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="renderInfo">レンダリング情報</param>
        public override void Render(Scene scene, RenderInfo renderInfo)
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget();
            Rectangle.Render(scene, renderInfo);
            RenderTarget.UnBindRenderTarget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            var textures = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            Rectangle.Polygon.Material.AddTexture(TextureKind.World, textures[(int)GBuffer.OutputTextureType.Posit]);
            Rectangle.Polygon.Material.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Normal]);
            Rectangle.Polygon.Material.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.OutputTextureType.Color]);
            Rectangle.Polygon.Material.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.OutputTextureType.Light]);
        }
    }
}
