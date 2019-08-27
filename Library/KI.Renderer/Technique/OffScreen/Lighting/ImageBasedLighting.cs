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
        public ImageBasedLighting(RenderSystem renderer, string vertexShader, string fragShader)
            : base("IBL", renderer, vertexShader, fragShader,RenderType.Original)
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
            foreach (var probe in scene.RootNode.AllChildren())
            {
                if (probe.KIObject is EnvironmentProbe)
                {
                    EnvironmentProbe env = probe.KIObject as EnvironmentProbe;
                    Rectanle.Polygon.Material.AddTexture(TextureKind.Cubemap, env.Cubemap);
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
            Rectanle.Polygon.Material.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.OutputTextureType.Color]);
            Rectanle.Polygon.Material.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Normal]);
            Rectanle.Polygon.Material.AddTexture(TextureKind.World, textures[(int)GBuffer.OutputTextureType.Posit]);
            Rectanle.Polygon.Material.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.OutputTextureType.Light]);
        }

        private Texture _uCubeMap;
        public Texture uCubeMap
        {
            get
            {
                return _uCubeMap;
            }

            set
            {
                SetValue(ref _uCubeMap, value);
            }
        }


    }
}
