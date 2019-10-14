using KI.Asset;
using KI.Gfx.Render;
using OpenTK.Graphics.OpenGL;
using System;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// ZPrepassRender
    /// </summary>
    public abstract class GBufferTechnique : RenderTechnique
    {
        public int OutputBufferNum
        {
            get;
            private set;
        }

        public GBufferTechnique(string name, RenderSystem renderer, int outputBufferNum, bool useDepthTexture)
            : base(name, renderer, useDepthTexture)
        {
            OutputBufferNum = outputBufferNum;
        }

        protected override void CreateRenderTarget()
        {
            var renderTexture = new RenderTexture[OutputBufferNum];
            for (int i = 0; i < OutputBufferNum; i++)
            {
                renderTexture[i] = TextureFactory.Instance.CreateRenderTexture("Texture:" + Name + i.ToString());
            }


            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget("RenderTarget:" + Name, 1, 1, false);
            RenderTarget.SetRenderTexture(renderTexture);
        }
    }
}
