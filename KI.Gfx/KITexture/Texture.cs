using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.KITexture
{
    /// <summary>
    /// テクスチャ
    /// </summary>
    public class Texture : KIObject
    {
        #region [constructor]

        public static readonly Texture Empty;

        /// <summary>
        /// MinMag兼ねたフィルタ
        /// </summary>
        private TextureMinFilter filter = TextureMinFilter.Linear;

        /// <summary>
        /// デフォルトはクランプ
        /// </summary>
        private TextureWrapMode wrapMode = TextureWrapMode.Repeat;

        public Texture(string name, TextureType type)
            : base(name)
        {
            ImageInfos = new List<KIImageInfo>();
            CreateTextureBuffer(type);
        }

        /// <summary>
        /// 空のテクスチャの作成
        /// </summary>
        /// <param name="name">テクスチャ名</param>
        /// <param name="type">テクスチャタイプ</param>
        /// <param name="width">横<param>
        /// <param name="height">縦</param>
        public Texture(string name, TextureType type, int width, int height)
            : base(name)
        {
            ImageInfos = new List<KIImageInfo>();
            CreateTextureBuffer(type);
            TextureBuffer.SetEmpty(width, height);
        }

        #endregion

        #region [member]
        /// <summary>
        /// 横
        /// </summary>
        public int Width
        {
            get
            {
                return TextureBuffer.Width;
            }
        }

        /// <summary>
        /// 縦
        /// </summary>
        public int Height
        {
            get
            {
                return TextureBuffer.Height;
            }
        }

        public TextureBuffer TextureBuffer { get; set; }

        public List<KIImageInfo> ImageInfos { get; private set; }

        public KIImageInfo ImageInfo
        {
            get
            {
                if (ImageInfos == null)
                {
                    return null;
                }

                return ImageInfos.FirstOrDefault();
            }
        }

        /// <summary>
        /// テクスチャID
        /// </summary>
        public int DeviceID
        {
            get
            {
                return TextureBuffer.DeviceID;
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
        #endregion

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            TextureBuffer.Dispose();
        }
        #region [テクスチャ周り]

        public void GenCubemapTexture(List<KIImageInfo> images)
        {
            if (images == null)
            {
                Logger.Log(Logger.LogLevel.Error, "not set image");
            }

            if (images.Count != 6)
            {
                Logger.Log(Logger.LogLevel.Error, "can not set up cubemap texture");
            }

            TextureBuffer.BindBuffer();
            TextureTarget[] target = new TextureTarget[6];
            target[0] = TextureTarget.TextureCubeMapPositiveX;
            target[1] = TextureTarget.TextureCubeMapPositiveY;
            target[2] = TextureTarget.TextureCubeMapPositiveZ;
            target[3] = TextureTarget.TextureCubeMapNegativeX;
            target[4] = TextureTarget.TextureCubeMapNegativeY;
            target[5] = TextureTarget.TextureCubeMapNegativeZ;

            for (int i = 0; i < 6; i++)
            {
                KIImageInfo image = images[i];
                if (!image.Loaded)
                {
                    image.LoadImageData();
                }

                image.Lock();
                //targetのPX～は、cubemapの2つ目以降から
                SetupTexImage2D(target[i], image);
                image.UnLock();
                ImageInfos.Add(image);
            }

            TextureBuffer.UnBindBuffer();
        }

        /// <summary>
        /// ファイルからのテクスチャ生成
        /// </summary>
        /// <param name="image">画像ファイル情報</param>
        public void GenTexture(KIImageInfo image)
        {
            if (!image.Loaded)
            {
                image.LoadImageData();
            }

            TextureBuffer.BindBuffer();
            TextureBuffer.Width = image.Width;
            TextureBuffer.Height = image.Height;

            image.Lock();
            SetupTexImage2D(TextureBuffer.Target, image);
            ImageInfos.Add(image);
            image.UnLock();
            TextureBuffer.UnBindBuffer();
        }

        /// <summary>
        /// 配列からのテクスチャ生成
        /// </summary>
        /// <param name="rgba">色情報</param>
        public void GenTexture(float[,,] rgba)
        {
            TextureBuffer.BindBuffer();

            SetupTexImage2D(TextureBuffer.Target, PixelInternalFormat.Four, rgba);

            TextureBuffer.UnBindBuffer();
        }

        /// <summary>
        /// テクスチャの読み込み
        /// </summary>
        /// <param name="type">テクスチャ種類</param>
        private void CreateTextureBuffer(TextureType type)
        {
            TextureBuffer = BufferFactory.Instance.CreateTextureBuffer(type);
            TextureBuffer.GenBuffer();
            TextureBuffer.BindBuffer();
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapS, Convert.ToInt32(this.WrapMode));
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapT, Convert.ToInt32(this.WrapMode));
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureMinFilter, (int)this.Filter);
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureMagFilter, (int)this.Filter);
            TextureBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        private void SetupTexImage2D(TextureTarget target, PixelInternalFormat pixelFormat, float[,,] rgba)
        {
            GL.TexImage2D(target, 0, pixelFormat, Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, rgba);
        }

        private void SetupTexImage2D(TextureTarget target, KIImageInfo image)
        {
            if (image.Format == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
                                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, image.Scan0);
            }
            else if (image.Format == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
                                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, image.Scan0);
            }
            else
            {
                GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
                                0, OpenTK.Graphics.OpenGL.PixelFormat.ColorIndex, PixelType.UnsignedByte, image.Scan0);
            }
        }

        /// <summary>
        /// wrapmodeの設定
        /// </summary>
        /// <param name="wrapMode"></param>
        private void BindWrapMode(TextureWrapMode wrapMode)
        {
            TextureBuffer.BindBuffer();
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapS, Convert.ToInt32(wrapMode));
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapT, Convert.ToInt32(wrapMode));
            TextureBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// フィルタの設定
        /// </summary>
        /// <param name="filter"></param>
        private void BindFilter(TextureMinFilter filter)
        {
            TextureBuffer.BindBuffer();
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureMinFilter, (int)filter);
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureMagFilter, (int)filter);
            TextureBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        #endregion
    }
}
