using KI.Asset;
using KI.Gfx.KITexture;
using RenderApp.Globals;

namespace RenderApp.RARenderSystem
{

    public class DeferredBuffer : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\Lighthing\Defferd.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\Lighthing\Defferd.frag";

        public DeferredBuffer(RenderTechniqueType tech)
            : base("Deferred", vertexShader, fragShader, tech, RenderType.Original)
        {

        }

        public override void Render()
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var light in Workspace.SceneManager.ActiveScene.RootNode.AllChildren())
            {
                if (light.KIObject is Light)
                {
                    Plane.Shader = ShaderItem;
                    Plane.Render();
                }
            }
            RenderTarget.UnBindRenderTarget();
        }

        public override void Initialize()
        {
            Plane.AddTexture(TextureKind.Albedo, Workspace.RenderSystem.GBufferStage.OutputTexture[2]);
            Plane.AddTexture(TextureKind.Normal, Workspace.RenderSystem.GBufferStage.OutputTexture[1]);
            Plane.AddTexture(TextureKind.World, Workspace.RenderSystem.GBufferStage.OutputTexture[0]);
            Plane.AddTexture(TextureKind.Lighting, Workspace.RenderSystem.GBufferStage.OutputTexture[3]);
        }
    }
}
