using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace RenderApp.GLUtil.Buffer
{
    class SamplerBuffer : BufferObject
    {
        public int ActiveTexture
        {
            get;
            set;
        }

        public override void PreGenBuffer()
        {
            ID = GL.GenSampler();
        }

        public override void PreDispose()
        {
            GL.DeleteSampler(ID);
        }

        public override void PreBindBuffer()
        {
            GL.BindSampler(ActiveTexture, ID);
        }

        public override void PreUnBindBuffer()
        {
            GL.BindSampler(ActiveTexture, ID);
        }
    }
}
