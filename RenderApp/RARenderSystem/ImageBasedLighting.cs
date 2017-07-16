using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.KIAsset;
namespace RenderApp.RARenderSystem
{
    class ImageBasedLighting : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\Lighthing\ibl.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\Lighthing\ibl.frag";

        public ImageBasedLighting(RenderTechniqueType tech)
            : base("IBL", vertexShader, fragShader, tech, RenderType.Original)
        {
            
        }

        public override void Render()
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var probe in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            {
                if (probe.KIObject is EnvironmentProbe)
                {
                    EnvironmentProbe env = probe.KIObject as EnvironmentProbe;
                    Plane.AddTexture(TextureKind.Cubemap, env.Cubemap);
                    Plane.Shader = ShaderItem;
                    Plane.Render();
                }
            }
            RenderTarget.UnBindRenderTarget();
        }

        public override void Initialize()
        {
            Plane.AddTexture(TextureKind.Albedo, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[2]);
            Plane.AddTexture(TextureKind.Normal, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[1]);
            Plane.AddTexture(TextureKind.World, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[0]);
            Plane.AddTexture(TextureKind.Lighting, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[3]);
        }
    }
}
