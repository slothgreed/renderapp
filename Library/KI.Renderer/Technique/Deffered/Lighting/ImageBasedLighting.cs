using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// IBL
    /// </summary>
    public class ImageBasedLighting : DefferedTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageBasedLighting(RenderSystem renderer, string vertexShader, string fragShader)
            : base("IBL", renderer, vertexShader, fragShader)
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
            foreach (var probe in scene.RootNode.AllChildren())
            {
                if (probe.KIObject is EnvironmentProbe)
                {
                    EnvironmentProbe env = probe.KIObject as EnvironmentProbe;
                    Rectangle.Polygon.Material.AddTexture(TextureKind.Cubemap, env.Cubemap);
                    Rectangle.Render(scene, renderInfo);
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
            Rectangle.Polygon.Material.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.OutputTextureType.Color]);
            Rectangle.Polygon.Material.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Normal]);
            Rectangle.Polygon.Material.AddTexture(TextureKind.World, textures[(int)GBuffer.OutputTextureType.Posit]);
            Rectangle.Polygon.Material.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.OutputTextureType.Light]);
        }

        private TextureBuffer _uCubeMap;
        public TextureBuffer uCubeMap
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
