using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.GLUtil.Buffer
{
    public class TextureBuffer : BufferObject
    {
        public TextureTarget Target
        {
            get;
            private set;
        }
        public PixelInternalFormat Format
        {
            get;
            private set;
        }
        public TextureBuffer()
        {
            Target = TextureTarget.Texture2D;
            Format = PixelInternalFormat.Rgba8;
        }
        public TextureBuffer(TextureTarget target)
        {
            Target = target;

        }
        public override void PreGenBuffer()
        {
            ID = GL.GenTexture();
        }
        public void SetData()
        {

        }
        public override void PreBindBuffer()
        {
            GL.BindTexture(Target, ID);
        }
        public override void PreUnBindBuffer()
        {
            GL.BindTexture(Target, 0);
        }
        public override void PreDispose()
        {
            GL.DeleteTexture(ID);
        }
        public void SizeChanged(int width, int height)
        {
            BindBuffer();
            GL.TexImage2D(Target, 0, Format, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte, IntPtr.Zero);
            UnBindBuffer();
        }
        public void SetEmpty(int width, int height)
        {
            BindBuffer();
            GL.TexImage2D(Target, 0, Format, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte, IntPtr.Zero);
            UnBindBuffer();
        }
    }
}
