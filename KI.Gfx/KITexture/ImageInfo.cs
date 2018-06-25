using System;
using System.Drawing;
using System.Drawing.Imaging;
using KI.Foundation.Core;

namespace KI.Gfx.KITexture
{
    /// <summary>
    /// 画像種類
    /// </summary>
    public enum ImageKind
    {
        None,
        PNG,
        JPG,
        BMP,
        TGA,
        HDR,
        Num
    }

    /// <summary>
    /// 画像情報
    /// </summary>
    public class ImageInfo : KIObject
    {
        /// <summary>
        /// ロック中か
        /// </summary>
        private bool isLock;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">パス</param>
        public ImageInfo(string name)
            : base(name)
        {
            Format = PixelFormat.Format32bppArgb;
        }

        /// <summary>
        /// 横
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// 縦
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// フォーマット
        /// </summary>
        public PixelFormat Format { get; set; }

        /// <summary>
        /// ビットマップ
        /// </summary>
        public Bitmap BmpImage { get; protected set; }

        /// <summary>
        /// ビットマップデータ
        /// </summary>
        public BitmapData BmpData { get; private set; }

        /// <summary>
        /// ポインタデータ
        /// </summary>
        public IntPtr Scan0
        {
            get
            {
                if (!isLock)
                {
                    Logger.Log(Logger.LogLevel.Warning, "Not Lock");
                }

                return BmpData.Scan0;
            }
        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>成功か</returns>
        public virtual bool Load(string filePath)
        {
            BmpImage = new Bitmap(filePath);
            if (System.IO.Path.GetExtension(filePath) == ".bmp")
            {
                BmpImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            Width = BmpImage.Width;
            Height = BmpImage.Height;
            return true;
        }

        public bool LoadRenderImage(int width, int height)
        {
            Width = width;
            Height = height;
            Format = PixelFormat.Format24bppRgb;

            BmpImage = new Bitmap(width, height);

            WriteLock();

            OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, width, height,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
                OpenTK.Graphics.OpenGL.PixelType.UnsignedByte,
                BmpData.Scan0);

            UnLock();

            return true;
        }

        public void WriteLock()
        {
            isLock = true;
            BmpData = BmpImage.LockBits(new Rectangle(0, 0, Width, Height),
                ImageLockMode.WriteOnly, Format);
        }

        /// <summary>
        /// ロック
        /// </summary>
        public void ReadLock()
        {
            isLock = true;
            BmpData = BmpImage.LockBits(new Rectangle(0, 0, Width, Height),
                ImageLockMode.ReadOnly, Format);
        }

        /// <summary>
        /// ロック解除
        /// </summary>
        public void UnLock()
        {
            BmpImage.UnlockBits(BmpData);
            isLock = false;
        }

        public override void Dispose()
        {
            BmpImage.Dispose();
        }
    }
}
