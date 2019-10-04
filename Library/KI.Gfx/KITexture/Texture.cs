using System;
using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KITexture;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// テクスチャ種類
    /// </summary>
    public enum TextureKind
    {
        None = -1,
        Albedo,
        Normal,
        Specular,
        Height,
        Emissive,
        World,
        Lighting,
        Cubemap
    }

    /// <summary>
    /// テクスチャ
    /// </summary>
    public partial class TextureBuffer : BufferObject
    {
        #region [constructor]

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

        #endregion

        #region [member]

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
        #endregion

        #region [テクスチャ周り]

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

        #endregion
    }
}
