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
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIImage;

namespace KI.Gfx.KIAsset
{

    public class Texture : KIObject
    {
        #region [member]

        private KIImageInfo _imageInfo;
        public KIImageInfo ImageInfo
        {
            get
            {
                return _imageInfo;
            }
            set
            {
                _imageInfo = value;
            }
        }
        public TextureBuffer TextureBuffer
        {
            get;
            set;
        }
        
        /// <summary>
        /// テクスチャID
        /// </summary>
        public int DeviceID { 
            get
            {
                return TextureBuffer.DeviceID;
            }
            set
            {
                TextureBuffer.DeviceID = value;
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

        public Texture(string name, string path)
            : base(name)
        {
            CreateTextureBuffer2D();
        }
        public Texture(string name, int width, int height)
            : base(name)
        {
            CreateTextureBuffer2D();
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
        public void LoadTexture(KIImageInfo image)
        {
            ImageInfo = image;

            if (!ImageInfo.Loaded)
            {
                ImageInfo.LoadImageData();
            }

            TextureBuffer.BindBuffer();

            ImageInfo.Lock();

            if(ImageInfo.Format == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                GL.TexImage2D(TextureBuffer.Target, 0, PixelInternalFormat.Rgba, ImageInfo.Width, ImageInfo.Height,
                                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, ImageInfo.Scan0);
            }
            else if(ImageInfo.Format == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                GL.TexImage2D(TextureBuffer.Target, 0, PixelInternalFormat.Rgba, ImageInfo.Width, ImageInfo.Height,
                                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, ImageInfo.Scan0);
            }
            else
            {
                GL.TexImage2D(TextureBuffer.Target, 0, PixelInternalFormat.Rgba, ImageInfo.Width, ImageInfo.Height,
                                0, OpenTK.Graphics.OpenGL.PixelFormat.ColorIndex, PixelType.UnsignedByte, ImageInfo.Scan0);
            }

            ImageInfo.UnLock();
            TextureBuffer.UnBindBuffer();
        }
    
        /// <summary>
        /// テクスチャの読み込み
        /// </summary>
        /// <param name="filename"></param>
        /// <rereturns>バインドしたテクスチャID</rereturns>
        public void CreateTextureBuffer2D()
        {
            TextureBuffer = new TextureBuffer();
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
