using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace RenderApp.GLUtil.Buffer
{
    public class RenderBuffer : BufferObject
    {
        public override void GenBuffer()
        {
            ID = GL.GenRenderbuffer();
        }

        public override void BindBuffer()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer,ID);
        }

        public override void UnBindBuffer()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public override void Dispose()
        {
            GL.DeleteRenderbuffer(0);
        }
    }
}
