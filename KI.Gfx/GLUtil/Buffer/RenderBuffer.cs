using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using KI.Foundation.Utility;
namespace KI.Gfx.GLUtil
{
    public class RenderBuffer : BufferObject
    {
        internal RenderBuffer()
        {
        }

        RenderbufferStorage BufferStorage { get; set; }

        public override void PreGenBuffer()
        {
            DeviceID = GL.GenRenderbuffer();
        }

        public override void PreBindBuffer()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DeviceID);
        }

        public override void PreUnBindBuffer()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public void Storage(RenderbufferStorage storage, int width, int height)
        {
            BufferStorage = storage;
            SizeChanged(width, height);
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            BindBuffer();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, BufferStorage, width, height);
            UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        public override void PreDispose()
        {
            GL.DeleteRenderbuffer(DeviceID);
        }
    }
}
