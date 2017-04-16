using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using KI.Foundation.Utility;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;

namespace KI.Gfx.KIAsset
{

    public class Texture : KIObject
    {
        #region [member]
        public int Width
        {
            get
            {
                return TextureBuffer.Width;
            }
        }
        public int Height
        {
            get
            {
                return TextureBuffer.Height;
            }
        }

        public TextureBuffer TextureBuffer
        {
            get;
            set;
        }

        public List<KIImageInfo> ImageInfos
        {
            get;
            private set;
        }

        public KIImageInfo ImageInfo
        {
            get
            {
                if(ImageInfos == null)
                {
                    return null;
                }
                return ImageInfos.FirstOrDefault();
            }
        }
        
        /// <summary>
        /// テクスチャID
        /// </summary>
        public int DeviceID { 
            get
            {
                return TextureBuffer.DeviceID;
            }
        }
        /// <summary>
        /// MinMag兼ねたフィルタ
        /// </summary>
        private TextureMinFilter _filter = TextureMinFilter.Linear;
        public TextureMinFilter Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                BindFilter(_filter);
            }
        }
        /// <summary>
        /// デフォルトはクランプ
        /// </summary>
        private TextureWrapMode _wrapMode = TextureWrapMode.Repeat;
        public TextureWrapMode WrapMode
        {
            get { return _wrapMode; }
            set
            {
                if (_wrapMode != value)
                {
                    _wrapMode = value;
                    BindWrapMode(_wrapMode);
                }
            }
        }
        #endregion

        public static readonly Texture Empty;
        #region [constructor]

        internal Texture(string name,TextureType type)
            : base(name)
        {
            ImageInfos = new List<KIImageInfo>();
            CreateTextureBuffer(type);
        }
        internal Texture(string name, TextureType type, int width, int height)
            : base(name)
        {
            ImageInfos = new List<KIImageInfo>();
            CreateTextureBuffer(type);
            TextureBuffer.SetEmpty(width, height);
        }

        #endregion
        private void BindWrapMode(TextureWrapMode wrapMode)
        {
            TextureBuffer.BindBuffer();
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapS, Convert.ToInt32(wrapMode));
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapT, Convert.ToInt32(wrapMode));
            TextureBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
        private void BindFilter(TextureMinFilter filter)
        {
            TextureBuffer.BindBuffer();
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureMinFilter, (int)filter);
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureMagFilter, (int)filter);
            TextureBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        public override void Dispose()
        {
            TextureBuffer.Dispose();
        }
        #region [テクスチャ周り]

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
                SetupTexImage2D((target[i], image);
                image.UnLock();
                ImageInfos.Add(image);
            }
            TextureBuffer.UnBindBuffer();

        }

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
        /// テクスチャの読み込み
        /// </summary>
        /// <param name="filename"></param>
        /// <rereturns>バインドしたテクスチャID</rereturns>
        public void CreateTextureBuffer(TextureType type)
        {
            TextureBuffer = new TextureBuffer(type);
            TextureBuffer.GenBuffer();
            TextureBuffer.BindBuffer();
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapS, Convert.ToInt32(this.WrapMode));
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureWrapT, Convert.ToInt32(this.WrapMode));
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureMinFilter, (int)this.Filter);
            GL.TexParameter(TextureBuffer.Target, TextureParameterName.TextureMagFilter, (int)this.Filter);
            TextureBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
        #endregion
    }
}
