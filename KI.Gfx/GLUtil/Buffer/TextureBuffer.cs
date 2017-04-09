using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace KI.Gfx.GLUtil
{
    public enum TextureType
    {
        Texture1D,
        Texture2D,
        Texture3D,
        Cubemap
    }
    
    public class TextureBuffer : BufferObject
    {
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }
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
        public void SetTextureTargte(TextureType type)
        {
            switch (type)
            {
                case TextureType.Texture1D:
                    Target = TextureTarget.Texture1D;
                    break;
                case TextureType.Texture2D:
                    Target = TextureTarget.Texture2D;
                    break;
                case TextureType.Texture3D:
                    Target = TextureTarget.Texture3D;
                    break;
                case TextureType.Cubemap:
                    Target = TextureTarget.TextureCubeMap;
                    break;
                default:
                    Target = TextureTarget.Texture2D;
                    break;
            }


        }
        public TextureBuffer(TextureType type)
        {
            SetTextureTargte(type);
            Format = PixelInternalFormat.Rgba8;
        }
        public override void PreGenBuffer()
        {
            DeviceID = GL.GenTexture();
        }
        public void SetData()
        {

        }
        public override void PreBindBuffer()
        {
            GL.BindTexture(Target, DeviceID);
        }
        public override void PreUnBindBuffer()
        {
            GL.BindTexture(Target, 0);
        }
        public override void PreDispose()
        {
            GL.DeleteTexture(DeviceID);
        }
        public void SizeChanged(int width, int height)
        {
            BindBuffer();
            Width = width;
            Height = height;
            GL.TexImage2D(Target, 0, Format, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte, IntPtr.Zero);
            UnBindBuffer();
        }
        public void SetEmpty(int width, int height)
        {
            BindBuffer();
            GL.TexImage2D(Target, 0, Format, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte, IntPtr.Zero);
            Width = width;
            Height = height;
            UnBindBuffer();
        }
    }
}
