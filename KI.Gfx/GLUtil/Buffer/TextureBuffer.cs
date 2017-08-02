using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    public enum TextureType
    {
        Texture1D,
        Texture2D,
        Texture3D,
        Cubemap,    //Can't Create Buffer
        CubemapPX,
        CubemapPY,
        CubemapPZ,
        CubemapNX,
        CubemapNY,
        CubemapNZ,
    }

    public class TextureBuffer : BufferObject
    {
        internal TextureBuffer(TextureType type)
        {
            if (type != TextureType.Cubemap)
            {
                SetTextureTargte(type);
            }
            else
            {
                SetTextureTargte(type);
                SetTextureTargte(TextureType.CubemapPX);
                SetTextureTargte(TextureType.CubemapPY);
                SetTextureTargte(TextureType.CubemapPZ);
                SetTextureTargte(TextureType.CubemapNX);
                SetTextureTargte(TextureType.CubemapNY);
                SetTextureTargte(TextureType.CubemapNZ);
            }

            Format = PixelInternalFormat.Rgba8;
        }

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

        public List<TextureTarget> Targets
        {
            get;
            private set;
        }

        public TextureTarget Target
        {
            get
            {
                return Targets.FirstOrDefault();
            }
        }

        public PixelInternalFormat Format
        {
            get;
            private set;
        }

        public override void PreGenBuffer()
        {
            DeviceID = GL.GenTexture();
        }

        public void SetData()
        {
        }

        public void BindBuffer(int i)
        {
            GL.BindTexture(Targets[i], DeviceID);
            if (NowBind)
            {
                Logger.Log(Logger.LogLevel.Warning, "Duplicate Bind Error");
            }

            NowBind = true;
            Logger.GLLog(Logger.LogLevel.Error);
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

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
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

        private void SetTextureTargte(TextureType type)
        {
            if (Targets == null)
            {
                Targets = new List<TextureTarget>();
            }

            switch (type)
            {
                case TextureType.Texture1D:
                    Targets.Add(TextureTarget.Texture1D);
                    break;
                case TextureType.Texture2D:
                    Targets.Add(TextureTarget.Texture2D);
                    break;
                case TextureType.Texture3D:
                    Targets.Add(TextureTarget.Texture3D);
                    break;
                case TextureType.Cubemap:
                    Targets.Add(TextureTarget.TextureCubeMap);
                    break;
                case TextureType.CubemapPX:
                    Targets.Add(TextureTarget.TextureCubeMapPositiveX);
                    break;
                case TextureType.CubemapPY:
                    Targets.Add(TextureTarget.TextureCubeMapPositiveY);
                    break;
                case TextureType.CubemapPZ:
                    Targets.Add(TextureTarget.TextureCubeMapPositiveZ);
                    break;
                case TextureType.CubemapNX:
                    Targets.Add(TextureTarget.TextureCubeMapNegativeX);
                    break;
                case TextureType.CubemapNY:
                    Targets.Add(TextureTarget.TextureCubeMapNegativeY);
                    break;
                case TextureType.CubemapNZ:
                    Targets.Add(TextureTarget.TextureCubeMapNegativeX);
                    break;
                default:
                    Targets.Add(TextureTarget.Texture1D);
                    break;
            }
        }
    }
}
