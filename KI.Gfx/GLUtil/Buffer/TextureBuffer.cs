using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// テクスチャ種類
    /// </summary>
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

    /// <summary>
    /// テクスチャバッファ
    /// </summary>
    public class TextureBuffer : BufferObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">テクスチャ種類</param>
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

        /// <summary>
        /// 横
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 縦
        /// </summary>
        public int Height { get; set; }

        public List<TextureTarget> Targets { get; private set; }

        public PixelInternalFormat Format { get; private set; }

        public TextureTarget Target
        {
            get
            {
                return Targets.FirstOrDefault();
            }
        }

        
        /// <summary>
        /// バッファの生成
        /// </summary>
        public override void GenBufferCore()
        {
            DeviceID = GL.GenTexture();
        }

        public void SetData()
        {
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        /// <param name="i"></param>
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

        /// <summary>
        /// バッファのバインド
        /// </summary>
        public override void BindBufferCore()
        {
            GL.BindTexture(Target, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        public override void UnBindBufferCore()
        {
            GL.BindTexture(Target, 0);
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        public override void DisposeCore()
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

        /// <summary>
        /// 空バッファの作成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SetEmpty(int width, int height)
        {
            BindBuffer();
            GL.TexImage2D(Target, 0, Format, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte, IntPtr.Zero);
            Width = width;
            Height = height;
            UnBindBuffer();
        }

        /// <summary>
        /// テクスチャターゲットの設定
        /// </summary>
        /// <param name="type"></param>
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
