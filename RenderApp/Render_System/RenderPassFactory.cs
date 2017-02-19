using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIAsset;
using RenderApp.GLUtil;
using KI.Gfx.Render;
namespace RenderApp.Render_System
{
    public class RenderPassFactory
    {
        private static RenderPassFactory _instance = new RenderPassFactory();
        public static RenderPassFactory Instance
        {
            get
            {
                return _instance;
            }
        }
        private List<RenderTarget> renderTargetList = new List<RenderTarget>();
        public RenderTarget CreateDefaultLithingBuffer(int width, int height)
        {
            Texture texture = TextureFactory.Instance.CreateTexture("LightBuffer", width, height);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture);

            RenderTarget frame = new RenderTarget("LightBuffer", width, height, texture);
            renderTargetList.Add(frame);
            return frame;
        }


        internal GBuffer CreateGBuffer(int width, int height)
        {
            GBuffer gBuffer = new GBuffer(width,height);
            renderTargetList.Add(gBuffer);
            return gBuffer;
        }

        internal void Dispose()
        {
            foreach(var frame in renderTargetList)
            {
                frame.Dispose();
            }
        }
    }

}
