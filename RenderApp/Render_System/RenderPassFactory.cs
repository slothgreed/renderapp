using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
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
            FrameBuffer frame = new FrameBuffer("LightBuffer",width,height,"LightBuffer");
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
            FrameBuffer frame = new FrameBuffer("SelectionBuffer", width, height, "SelectionBuffer");
            frameList.Add(frame);
            return frame;
        }
    }

}
