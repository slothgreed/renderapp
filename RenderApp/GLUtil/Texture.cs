using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using RenderApp.AssetModel;
using KI.Foundation.Utility;
using RenderApp.GLUtil.Buffer;
namespace RenderApp.GLUtil
{

    public class Texture : RAObject
    {
        #region [member]
        private string DummyTexturePath
        {
            get
            {
                return ProjectInfo.TextureDirectory + @"\Dummy.png";
            }
        }
        private RAImageInfo _imageInfo;
        public RAImageInfo ImageInfo
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
        public void LoadTexture(RAImageInfo image)
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
        #region [CubeMap 現在廃止]
        ///// <summary>
        ///// CubeMapTextureの作成
        ///// </summary>
        ///// <rereturns>バインドしたテクスチャID</rereturns>
        //public int CreateCubeMapTexture(string PXfile, string NXfile, string PYfile, string NYfile, string PZfile, string NZfile)
        //{
        //    try
        //    {
        //        Bitmap PX = new Bitmap(PXfile);
        //        Bitmap NX = new Bitmap(NXfile);
        //        Bitmap PY = new Bitmap(PYfile);
        //        Bitmap NY = new Bitmap(NYfile);
        //        Bitmap PZ = new Bitmap(PZfile);
        //        Bitmap NZ = new Bitmap(NZfile);

        //        Bitmap[] image = { PX, NX, PZ, NZ, PY, NY };
        //        int texId = GL.GenTexture();
        //        GL.BindTexture(TextureTarget.TextureCubeMap, texId);
        //        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        //        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        //        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        //        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        //        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        //        for (int i = 0; i < 6; i++)
        //        {
        //            //データ読み込み
        //            BitmapData bmp_data = image[i].LockBits(new Rectangle(0, 0, image[i].Width, image[i].Height),
        //                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
        //                image[i].Width, image[i].Height,
        //                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
        //            image[i].UnlockBits(bmp_data);
        //        }

        //        GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        //        Output.GLLog(Output.LogLevel.Error);
        //        return texId;

        //    }
        //    catch (Exception)
        //    {
        //        throw new System.IO.FileLoadException(PXfile + PYfile + PZfile + NXfile + NYfile + NZfile);
        //    }

        //}
        #endregion
    }
}
