using KI.Asset;
using KI.Gfx.Buffer;
using KI.Gfx.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Renderer.Technique
{
    public class HUDTechnique : RenderTechnique
    {
        public HUDTechnique( RenderSystem renderer)
            : base("HUDTechnique", renderer, false)
        {

        }

        public override void Initialize()
        {
        }

        public override void Render(Scene scene, RenderInfo renderInfo)
        {
            throw new NotSupportedException("Use Render(HUDObject hud)");
        }

        public void Render(List<HUDObject> huds, RenderInfo renderInfo)
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget();
            foreach(HUDObject hud in huds)
            {
                hud.Render(renderInfo);
            }
            RenderTarget.UnBindRenderTarget();
        }
    }
}
