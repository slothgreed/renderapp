using KI.Asset;
using KI.Gfx.Render;
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Technique
{
    public class HUDTechnique : RenderTechnique
    {
        public HUDTechnique( RenderSystem renderer)
            : base("HUDTechnique", renderer, false)
        {

        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// レンダーターゲットの作成
        /// </summary>
        protected override void CreateRenderTarget()
        {
            var texture = new RenderTexture[] { TextureFactory.Instance.CreateRenderTexture("Texture:" + Name) };
            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget("RenderTarget:" + Name, 1, 1, false);
            RenderTarget.SetRenderTexture(texture);
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
