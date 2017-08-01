using System;
using System.Drawing.Imaging;
using System.Drawing;
using KI.Foundation.Utility;
using KI.Foundation.Core;

namespace KI.Gfx.KITexture
{
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
    /// BMPで保持
    /// </summary>
    public class KIImageInfo : KIFile
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public Bitmap bmpImage { get; set; }

        public BitmapData bmpData { get; set; }

        private bool NowLock { get; set; }

        public bool Loaded { get; set; }

        public PixelFormat Format { get; set; }

        public KIImageInfo(string path)
            : base(path)
        {
            Format = PixelFormat.Format32bppArgb;
        }
        public virtual bool LoadImageData()
        {
            if (Loaded)
            {
                return true;
            }
            bmpImage = new Bitmap(FilePath);
            if (System.IO.Path.GetExtension(FilePath) == ".bmp")
            {
                bmpImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            Width = bmpImage.Width;
            Height = bmpImage.Height;
            Loaded = true;
            return true;
        }

        public void Lock()
        {
            NowLock = true;
            bmpData = bmpImage.LockBits(new Rectangle(0, 0, Width, Height),
                ImageLockMode.ReadOnly, Format);

        }

        public IntPtr Scan0
        {
            get
            {
                if (!NowLock)
                {
                    Logger.Log(Logger.LogLevel.Warning, "Not Lock");
                }
                return bmpData.Scan0;
            }
        }

        public void UnLock()
        {
            bmpImage.UnlockBits(bmpData);
            NowLock = false;
        }

    }
}
