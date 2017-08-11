using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// IBL
    /// </summary>
    public class ImageBasedLighting : RenderTechnique
    {
        /// <summary>
        /// 頂点シェーダ
        /// </summary>
        private static string vertexShader = Global.ShaderDirectory + @"\Lighthing\ibl.vert";

        /// <summary>
        /// フラグシェーダ
        /// </summary>
        private static string fragShader = Global.ShaderDirectory + @"\Lighthing\ibl.frag";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageBasedLighting()
            : base("IBL", vertexShader, fragShader, RenderTechniqueType.IBL, RenderType.Original)
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(IScene scene)
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var probe in scene.RootNode.AllChildren())
            {
                if (probe.KIObject is EnvironmentProbe)
                {
                    EnvironmentProbe env = probe.KIObject as EnvironmentProbe;
                    Plane.Geometry.AddTexture(TextureKind.Cubemap, env.Cubemap);
                    Plane.Shader = ShaderItem;
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
            var textures = Global.RenderSystem.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            Plane.Geometry.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.GBufferOutputType.Posit]);
            Plane.Geometry.AddTexture(TextureKind.Normal, textures[(int)GBuffer.GBufferOutputType.Normal]);
            Plane.Geometry.AddTexture(TextureKind.World, textures[(int)GBuffer.GBufferOutputType.Color]);
            Plane.Geometry.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.GBufferOutputType.Light]);
        }
    }
}
