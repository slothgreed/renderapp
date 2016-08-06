using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using RenderApp.AssetModel;
namespace RenderApp.GLUtil
{

    public class Texture : Asset
    {
        #region [member]
        
        /// <summary>
        /// テクスチャID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 1D,2D,CubeMap etc
        /// </summary>
        private TextureType _type = TextureType.Texture2D;
        public TextureType TexType
        {
            get { return _type; }
            set { _type = value; }
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
                BindFilter(value);
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

        #region [constructor]
        private void Initialize()
        {
            Scene.ActiveScene.AddSceneObject(FileName, this);
        }
        public Texture()
        {
        }
        public Texture(string path, TextureType target = TextureType.Texture2D)
        {
            LoadTexture(path, target);
        }
        public Texture(string name, int width, int height)
        {
            this.Key = name;
            CreateFrameBuffer2D(width,height);
        }
        private void CreateFrameBuffer2D(int width,int height)
        {
            this.ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, this.ID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb32f, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Byte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public void LoadTexture(string path, TextureType target = TextureType.Texture2D)
        {
            this.FilePath = path;
            this.TexType = target;

            switch (target)
            {
                case TextureType.Texture2D:
                    CreateTextureBuffer2D();
                    break;
            }
            Load2DTexture();
            Initialize();
        }

        /// <summary>
        /// TODO:cubemapは別クラスへ処理
        /// </summary>
        public Texture(string px, string py, string pz, string nx, string ny, string nz)
        {
            this.FilePath = px;
            this.ID = CreateCubeMapTexture(px, nx, py, ny, pz, nz);
            this.TexType = TextureType.CubeMap;
            Initialize();
        }
        /// <summary>
        /// TODO:cubemapは別クラスへ処理
        /// </summary>
        public Texture(string name, int id)
        {
            this.FilePath = name;
            this.ID = id;
        }

        #endregion
        

        private void BindWrapMode(TextureWrapMode wrapMode)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.ID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, Convert.ToInt32(wrapMode));
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, Convert.ToInt32(wrapMode));
            GL.BindTexture(TextureTarget.Texture2D, 0);
            Output.GLError();
        }
        private void BindFilter(TextureMinFilter filter)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.ID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)filter);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            Output.GLError();
        }

        public override void Dispose()
        {
            GL.DeleteTexture(this.ID);
        }
        #region [テクスチャ周り]
        private void Load2DTexture()
        {
            try
            {
                Bitmap image = new Bitmap(this.FilePath);
                GL.BindTexture(TextureTarget.Texture2D, this.ID);
                //image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                //データ読み込み
                BitmapData bmp_data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
                image.UnlockBits(bmp_data);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            catch (Exception)
            {
                Output.Error("BindTextureBufferの失敗");
            }
        }
        /// <summary>
        /// テクスチャの読み込み
        /// </summary>
        /// <param name="filename"></param>
        /// <rereturns>バインドしたテクスチャID</rereturns>
        public void CreateTextureBuffer2D()
        {
            this.ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, this.ID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, Convert.ToInt32(this.WrapMode));
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, Convert.ToInt32(this.WrapMode));
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)this.Filter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)this.Filter);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            Output.GLError();
        }
        #endregion
        #region [CubeMap]

        /// <summary>
        /// CubeMapTextureの作成
        /// </summary>
        /// <rereturns>バインドしたテクスチャID</rereturns>
        public int CreateCubeMapTexture(string PXfile, string NXfile, string PYfile, string NYfile, string PZfile, string NZfile)
        {
            try
            {
                Bitmap PX = new Bitmap(PXfile);
                Bitmap NX = new Bitmap(NXfile);
                Bitmap PY = new Bitmap(PYfile);
                Bitmap NY = new Bitmap(NYfile);
                Bitmap PZ = new Bitmap(PZfile);
                Bitmap NZ = new Bitmap(NZfile);

                Bitmap[] image = { PX, NX, PZ, NZ, PY, NY };
                int texId = GL.GenTexture();
                GL.BindTexture(TextureTarget.TextureCubeMap, texId);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                for (int i = 0; i < 6; i++)
                {
                    //データ読み込み
                    BitmapData bmp_data = image[i].LockBits(new Rectangle(0, 0, image[i].Width, image[i].Height),
                        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
                        image[i].Width, image[i].Height,
                        0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
                    image[i].UnlockBits(bmp_data);
                }

                GL.BindTexture(TextureTarget.TextureCubeMap, 0);
                Output.GLError();
                return texId;

            }
            catch (Exception)
            {
                throw new System.IO.FileLoadException(PXfile + PYfile + PZfile + NXfile + NYfile + NZfile);
            }

        }
        #endregion
    }
}
