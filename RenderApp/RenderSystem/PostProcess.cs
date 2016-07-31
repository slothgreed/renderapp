using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.AssetModel.ShaderModel;
namespace RenderApp
{
    public class PostProcess
    {
        private Geometry Plane;
        FrameBuffer frameBuffer;
        public PostProcess(Shader shader,FrameBuffer frame)
        {
            frameBuffer = frame;
            Plane = new Plane();
            Plane.MaterialItem.SetShader(shader);
        }

        public void SetFrameBuffer(FrameBuffer frame)
        {
            frameBuffer = frame;
        }
        public void Render()
        {
            frameBuffer.BindBuffer();
            Plane.Render();
            frameBuffer.UnBindBuffer();
        }
        public void SizeChanged(int width,int height)
        {
            frameBuffer.SizeChanged(width, height);
        }
        public void Dispose()
        {
            Plane.Dispose();
        }
    }
}
