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
            : base("Deferred", renderer, vertexShader, fragShader,RenderType.Original)
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
            foreach (var light in scene.RootNode.AllChildren())
            {
                if (light.KIObject is Light)
                {
                    Rectanle.Render(scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            var textures = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            Rectanle.Polygon.AddTexture(TextureKind.World, textures[(int)GBuffer.OutputTextureType.Posit]);
            Rectanle.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Normal]);
            Rectanle.Polygon.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.OutputTextureType.Color]);
            Rectanle.Polygon.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.OutputTextureType.Light]);
        }
    }
}
