using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.KITexture
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
    public class Texture : KIObject
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
        public Texture(string name, TextureType type)
            : base(name)
        {
            CreateTextureBuffer(type);
        }

        /// <summary>
        /// 空のテクスチャの作成
        /// </summary>
        /// <param name="name">テクスチャ名</param>
        /// <param name="type">テクスチャタイプ</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public Texture(string name, TextureType type, int width, int height)
            : base(name)
        {
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

        /// <summary>
        /// テクスチャバッファ
        /// </summary>
        public TextureBuffer TextureBuffer { get; set; }

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

        /// <summary>
        /// キューブマップテクスチャの作成
        /// </summary>
        /// <param name="images">画像</param>
        public void GenCubemapTexture(List<ImageInfo> images)
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
                ImageInfo image = images[i];
                image.Lock();
                //targetのPX～は、cubemapの2つ目以降から
                SetupTexImage2D(target[i], image);
                image.UnLock();
            }

            TextureBuffer.UnBindBuffer();
        }

        /// <summary>
        /// ファイルからのテクスチャ生成
        /// </summary>
        /// <param name="image">画像ファイル情報</param>
        public void GenTexture(ImageInfo image)
        {
            TextureBuffer.BindBuffer();
            TextureBuffer.Width = image.Width;
            TextureBuffer.Height = image.Height;
            image.LoadImageData();
            image.Lock();
            SetupTexImage2D(TextureBuffer.Target, image);
            image.UnLock();
            TextureBuffer.UnBindBuffer();
        }

        /// <summary>
        /// 配列からのテクスチャ生成
        /// </summary>
        /// <param name="rgba">色情報</param>
        public void GenTexture(float[,,] rgba)
        {
            TextureBuffer.SetEmpty(rgba.GetLength(0), rgba.GetLength(1));
            TextureBuffer.BindBuffer();

            SetupTexImage2D(TextureBuffer.Target, PixelInternalFormat.Rgba, rgba);

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
            TextureBuffer.BindBuffer();
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapS, Convert.ToInt32(wrapMode));
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapT, Convert.ToInt32(wrapMode));
            TextureBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// フィルタの設定
        /// </summary>
        /// <param name="filter">テクスチャフィルタ</param>
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
