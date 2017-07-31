using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    class ImageBasedLighting : RenderTechnique
    {
        private static string vertexShader = Global.ShaderDirectory + @"\Lighthing\ibl.vert";
        private static string fragShader = Global.ShaderDirectory + @"\Lighthing\ibl.frag";

        public ImageBasedLighting(RenderTechniqueType tech)
            : base("IBL", vertexShader, fragShader, tech, RenderType.Original)
        {
            
        }

        public override void Render()
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var probe in Global.Scene.RootNode.AllChildren())
            {
                if (probe.KIObject is EnvironmentProbe)
                {
                    EnvironmentProbe env = probe.KIObject as EnvironmentProbe;
                    Plane.AddTexture(TextureKind.Cubemap, env.Cubemap);
                    Plane.Shader = ShaderItem;
                    Plane.Render(Global.Scene);
                }
            }
            RenderTarget.UnBindRenderTarget();
        }

        public override void Initialize()
        {
            Plane.AddTexture(TextureKind.Albedo, Global.RenderSystem.GBufferStage.OutputTexture[2]);
            Plane.AddTexture(TextureKind.Normal, Global.RenderSystem.GBufferStage.OutputTexture[1]);
            Plane.AddTexture(TextureKind.World, Global.RenderSystem.GBufferStage.OutputTexture[0]);
            Plane.AddTexture(TextureKind.Lighting, Global.RenderSystem.GBufferStage.OutputTexture[3]);
        }
    }
}
