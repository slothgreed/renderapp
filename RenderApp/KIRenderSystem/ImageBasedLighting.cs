using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.KIAsset;
namespace RenderApp.KIRenderSystem
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

            Plane.Shader = ShaderItem;
            Plane.Render();

            RenderTarget.UnBindRenderTarget();

        }

        public override void Initialize()
        {
        }
    }
}
