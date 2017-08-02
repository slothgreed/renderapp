using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    public class DeferredBuffer : RenderTechnique
    {
        private static string vertexShader = Global.ShaderDirectory + @"\Lighthing\Defferd.vert";
        private static string fragShader = Global.ShaderDirectory + @"\Lighthing\Defferd.frag";

        public DeferredBuffer(RenderTechniqueType tech)
            : base("Deferred", vertexShader, fragShader, tech, RenderType.Original)
        {
        }

        public override void Render()
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var light in Global.Scene.RootNode.AllChildren())
            {
                if (light.KIObject is Light)
                {
                    Plane.Shader = ShaderItem;
                    Plane.Render(Global.Scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            Plane.AddTexture(TextureKind.Albedo, Global.RenderSystem.GBufferStage.OutputTexture[2]);
            Plane.AddTexture(TextureKind.Normal, Global.RenderSystem.GBufferStage.OutputTexture[1]);
            Plane.AddTexture(TextureKind.World, Global.RenderSystem.GBufferStage.OutputTexture[0]);
            Plane.AddTexture(TextureKind.Lighting, Global.RenderSystem.GBufferStage.OutputTexture[3]);
        }
    }
}
