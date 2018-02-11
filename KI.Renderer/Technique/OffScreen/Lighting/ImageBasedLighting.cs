using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// IBL
    /// </summary>
    public class ImageBasedLighting : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageBasedLighting(string vertexShader, string fragShader)
            : base("IBL", vertexShader, fragShader, RenderTechniqueType.IBL, RenderType.Original)
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture);
            foreach (var probe in scene.RootNode.AllChildren())
            {
                if (probe.KIObject is EnvironmentProbe)
                {
                    EnvironmentProbe env = probe.KIObject as EnvironmentProbe;
                    Plane.Polygon.AddTexture(TextureKind.Cubemap, env.Cubemap);
                    Plane.Render(scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            var textures = Global.Renderer.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            Plane.Polygon.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.OutputTextureType.Posit]);
            Plane.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Normal]);
            Plane.Polygon.AddTexture(TextureKind.World, textures[(int)GBuffer.OutputTextureType.Color]);
            Plane.Polygon.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.OutputTextureType.Light]);
        }
    }
}
