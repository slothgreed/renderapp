using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KITexture;

namespace KI.Gfx.Buffer
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
    public partial class TextureBuffer : BufferObject
    {

        /// <summary>
        /// MinMag兼ねたフィルタ
        /// </summary>
        private TextureMinFilter filter = TextureMinFilter.Linear;

        /// <summary>
        /// デフォルトはクランプ
        /// </summary>
        private TextureWrapMode wrapMode = TextureWrapMode.Repeat;

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

            InternalFormat = PixelInternalFormat.Rgba8;
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">テクスチャ種類</param>
        public TextureBuffer(string name, TextureType type)
            : this(type)
        {
            CreateTextureBuffer(type);
            Name = name;
        }

        /// <summary>
        /// 空のテクスチャの作成
        /// </summary>
        /// <param name="name">テクスチャ名</param>
        /// <param name="type">テクスチャタイプ</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public TextureBuffer(string name, TextureType type, int width, int height, PixelFormat format = PixelFormat.Rgba)
            : this(type)
        {
            CreateTextureBuffer(type);
            SetEmpty(width, height, format);
            Name = name;
        }

        /// <summary>
        /// 横
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 縦
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// テクスチャの種類
        /// </summary>
        public List<TextureTarget> Targets { get; private set; }

        /// <summary>
        /// 内部のテクスチャフォーマット
        /// </summary>
        public PixelInternalFormat InternalFormat { get; private set; }

        /// <summary>
        /// テクスチャフォーマット
        /// </summary>
        public PixelFormat Format { get; private set; }

        /// <summary>
        /// テクスチャの種類
        /// </summary>
        public TextureTarget Target
        {
            get
            {
                return Targets.FirstOrDefault();
            }
        }


        /// <summary>
        /// MinMag兼ねたフィルタ
        /// </summary>
        public TextureMinFilter Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                BindFilter(filter);
            }
        }

        /// <summary>
        /// TextureWrapMode
        /// </summary>
        public TextureWrapMode WrapMode
        {
            get { return wrapMode; }
            set
            {
                if (wrapMode != value)
                {
                    wrapMode = value;
                    BindWrapMode(wrapMode);
                }
            }
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
            GL.TexImage2D(Target, 0, InternalFormat, width, height, 0, Format, PixelType.Byte, IntPtr.Zero);
            UnBindBuffer();
        }

        /// <summary>
        /// 空バッファの作成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SetEmpty(int width, int height, PixelFormat format = PixelFormat.Rgba)
        {
            BindBuffer();
            if(format == PixelFormat.DepthComponent)
            {
                GL.TexImage2D(Target, 0, PixelInternalFormat.DepthComponent, width, height, 0, format, PixelType.Byte, IntPtr.Zero);
                InternalFormat = PixelInternalFormat.DepthComponent;
            }
            else
            {
                GL.TexImage2D(Target, 0, InternalFormat, width, height, 0, format, PixelType.Byte, IntPtr.Zero);
            }

            Format = format;
            Width = width;
            Height = height;
            UnBindBuffer();
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
        /// バッファの生成
        /// </summary>
        protected override void GenBufferCore()
        {
            DeviceID = GL.GenTexture();
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        protected override void BindBufferCore()
        {
            GL.BindTexture(Target, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        protected override void UnBindBufferCore()
        {
            GL.BindTexture(Target, 0);
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        protected override void DisposeCore()
        {
            GL.DeleteTexture(DeviceID);
        }

        /// <summary>
        /// テクスチャターゲットの設定
        /// </summary>
        /// <param name="type">テクスチャの種類</param>
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


        /// <summary>
        /// キューブマップテクスチャの作成
        /// </summary>
        /// <param name="images">画像</param>
        public void SetCubemapFromImage(List<ImageInfo> images)
        {
            if (images == null)
            {
                Logger.Log(Logger.LogLevel.Error, "not set image");
            }

            if (images.Count != 6)
            {
                Logger.Log(Logger.LogLevel.Error, "can not set up cubemap texture");
            }

            BindBuffer();
            TextureTarget[] target = new TextureTarget[6];
            target[0] = TextureTarget.TextureCubeMapPositiveX;
            target[1] = TextureTarget.TextureCubeMapPositiveY;
            target[2] = TextureTarget.TextureCubeMapPositiveZ;
            target[3] = TextureTarget.TextureCubeMapNegativeX;
            target[4] = TextureTarget.TextureCubeMapNegativeY;
            target[5] = TextureTarget.TextureCubeMapNegativeZ;

            for (int i = 0; i < 6; i++)
            {
                ImageInfo image = images[i];
                image.ReadLock();
                //targetのPX～は、cubemapの2つ目以降から
                SetupTexImage2D(target[i], image);
                image.UnLock();
            }

            UnBindBuffer();
        }

        /// <summary>
        /// ファイルからのテクスチャ設定
        /// </summary>
        /// <param name="image">画像ファイル情報</param>
        public void SetTextureFromImage(ImageInfo image)
        {
            BindBuffer();
            Width = image.Width;
            Height = image.Height;
            image.ReadLock();
            SetupTexImage2D(Target, image);
            image.UnLock();
            UnBindBuffer();
        }

        /// <summary>
        /// 配列からのテクスチャ生成
        /// </summary>
        /// <param name="rgba">色情報</param>
        public void SetTextureFromArray(float[,,] rgba)
        {
            SetEmpty(rgba.GetLength(0), rgba.GetLength(1));
            BindBuffer();

            SetupTexImage2D(Target, PixelInternalFormat.Rgba, rgba);

            UnBindBuffer();
        }

        /// <summary>
        /// テクスチャの読み込み
        /// </summary>
        /// <param name="type">テクスチャ種類</param>
        private void CreateTextureBuffer(TextureType type)
        {
            GenBuffer();
            BindBuffer();
            GL.TexParameter(Target, TextureParameterName.TextureWrapS, Convert.ToInt32(this.WrapMode));
            GL.TexParameter(Target, TextureParameterName.TextureWrapT, Convert.ToInt32(this.WrapMode));
            GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)this.Filter);
            GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)this.Filter);
            UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// テクスチャのセット
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <param name="pixelFormat">フォーマット</param>
        /// <param name="rgba">色</param>
        private void SetupTexImage2D(TextureTarget target, PixelInternalFormat pixelFormat, float[,,] rgba)
        {
            GL.TexImage2D(target, 0, pixelFormat, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, rgba);
        }

        /// <summary>
        /// テクスチャのセット
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <param name="image">画像</param>
        private void SetupTexImage2D(TextureTarget target, ImageInfo image)
        {
            if (image.Format == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
                                0, PixelFormat.Bgr, PixelType.UnsignedByte, image.Scan0);
            }
            else if (image.Format == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
                                0, PixelFormat.Bgra, PixelType.UnsignedByte, image.Scan0);
            }
            else
            {
                GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
                                0, PixelFormat.ColorIndex, PixelType.UnsignedByte, image.Scan0);
            }
        }

        /// <summary>
        /// wrapmodeの設定
        /// </summary>
        /// <param name="wrapMode">wrap種類</param>
        private void BindWrapMode(TextureWrapMode wrapMode)
        {
            BindBuffer();
            GL.TexParameter(Target, TextureParameterName.TextureWrapS, Convert.ToInt32(wrapMode));
            GL.TexParameter(Target, TextureParameterName.TextureWrapT, Convert.ToInt32(wrapMode));
            UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// フィルタの設定
        /// </summary>
        /// <param name="filter">テクスチャフィルタ</param>
        private void BindFilter(TextureMinFilter filter)
        {
            BindBuffer();
            GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)filter);
            GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)filter);
            UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
    }
}
