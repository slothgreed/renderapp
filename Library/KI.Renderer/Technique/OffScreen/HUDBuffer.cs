using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Gfx.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Renderer.Technique
{
    public class HUDBuffer : RenderTechnique
    {
        public HUDBuffer( RenderSystem renderer)
            : base("HUD", renderer, RenderType.Forward)
        {

        }

        public override void Initialize()
        {
        }

        public override void Render(Scene scene)
        {
            throw new NotSupportedException("Use Render(HUDObject hud)");
        }

        public void Render(List<HUDObject> huds)
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget();
            foreach(HUDObject hud in huds)
            {
                hud.Render();
            }
            RenderTarget.UnBindRenderTarget();
        }
    }
}
