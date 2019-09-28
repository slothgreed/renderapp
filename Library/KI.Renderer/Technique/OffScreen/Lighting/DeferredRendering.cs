using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// Deferred Rendering
    /// </summary>
    public class DeferredRendering : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeferredRendering(RenderSystem renderer, string vertexShader, string fragShader)
            : base("Deferred", renderer, vertexShader, fragShader,RenderType.Forward)
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget();
            Rectanle.Render(scene);
            RenderTarget.UnBindRenderTarget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            var textures = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            Rectanle.Polygon.Material.AddTexture(TextureKind.World, textures[(int)GBuffer.OutputTextureType.Posit]);
            Rectanle.Polygon.Material.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Normal]);
            Rectanle.Polygon.Material.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.OutputTextureType.Color]);
            Rectanle.Polygon.Material.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.OutputTextureType.Light]);
        }
    }
}
