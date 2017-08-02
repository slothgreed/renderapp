using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace KI.Gfx.GLUtil
{
    public class SamplerBuffer : BufferObject
    {
        internal SamplerBuffer()
        {
        }

        public int ActiveTexture
        {
            get;
            set;
        }

        public override void PreGenBuffer()
        {
            DeviceID = GL.GenSampler();
        }

        public override void PreDispose()
        {
            GL.DeleteSampler(DeviceID);
        }

        public override void PreBindBuffer()
        {
            GL.BindSampler(ActiveTexture, DeviceID);
        }

        public override void PreUnBindBuffer()
        {
            GL.BindSampler(ActiveTexture, DeviceID);
        }
    }
}
