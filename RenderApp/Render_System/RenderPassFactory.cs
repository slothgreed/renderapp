using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIAsset;
using RenderApp.GLUtil;
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
        private List<FrameBuffer> frameList = new List<FrameBuffer>();
        public FrameBuffer CreateDefaultLithingBuffer(int width,int height)
        {
            Texture texture = TextureFactory.Instance.CreateTexture("LightBuffer", width, height);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture);

            FrameBuffer frame = new FrameBuffer("LightBuffer",width,height,texture);
            frameList.Add(frame);
            return frame;
        }


        internal GBuffer CreateGBuffer(int width, int height)
        {
            GBuffer gBuffer = new GBuffer(width,height);
            frameList.Add(gBuffer);
            return gBuffer;
        }

        internal void Dispose()
        {
            foreach(var frame in frameList)
            {
                frame.Dispose();
            }
        }

        internal FrameBuffer CreateSelectionBuffer(int width, int height)
        {
            Texture texture = TextureFactory.Instance.CreateTexture("SelectionBuffer", width, height);
            FrameBuffer frame = new FrameBuffer("SelectionBuffer", width, height, texture);
            frameList.Add(frame);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture);
            return frame;
        }
    }

}
