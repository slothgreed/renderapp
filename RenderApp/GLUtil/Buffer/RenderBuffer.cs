using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.GLUtil.Buffer
{
    public class RenderBuffer : BufferObject
    {
        RenderbufferStorage BufferStorage
        {
            get;
            set;
        }
        public override void PreGenBuffer()
        {
            ID = GL.GenRenderbuffer();
        }

        public override void PreBindBuffer()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer,ID);
        }

        public override void PreUnBindBuffer()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }
        public void Storage(RenderbufferStorage storage,int width,int height)
        {
            BufferStorage = storage;
            SizeChanged(width,height);
        }
        public void SizeChanged(int width,int height)
        {
            BindBuffer();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, BufferStorage , width, height);
            UnBindBuffer();
            Output.GLLog(Output.LogLevel.Error);

        }
        public override void PreDispose()
        {
            GL.DeleteRenderbuffer(ID);
        }
    }
}
