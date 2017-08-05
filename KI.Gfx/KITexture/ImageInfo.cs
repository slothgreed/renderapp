using System;
using System.Drawing;
using System.Drawing.Imaging;
using KI.Foundation.Core;
using KI.Foundation.Utility;

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
    public class ImageInfo : KIFile
    {
        /// <summary>
        /// ロック中か
        /// </summary>
        private bool isLock;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">パス</param>
        public ImageInfo(string path)
            : base(path)
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
        /// <returns>成功か</returns>
        public virtual bool LoadImageData()
        {
            BmpImage = new Bitmap(FilePath);
            if (System.IO.Path.GetExtension(FilePath) == ".bmp")
            {
                BmpImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            Width = BmpImage.Width;
            Height = BmpImage.Height;
            return true;
        }

        /// <summary>
        /// ロック
        /// </summary>
        public void Lock()
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
    }
}
