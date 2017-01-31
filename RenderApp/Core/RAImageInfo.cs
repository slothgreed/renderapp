using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using KI.Foundation.Utility;
using KI.Foundation.Core;

namespace RenderApp
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
    /// <summary>
    /// BMPで保持
    /// </summary>
    public class RAImageInfo : KIFile
    {
        public int Width
        {
            get;
            set;
        }
        
        public int Height
        {
            get;
            set;
        }

        public Bitmap bmpImage
        {
            get;
            set;
        }
        public BitmapData bmpData
        {
            get;
            set;
        }
        private bool NowLock
        {
            get;
            set;
        }
        public bool Loaded
        {
            get;
            set;
        }
        public PixelFormat Format
        {
            get;
            set;
        }

        public RAImageInfo(string path)
            :base(path)
        {
            Format = PixelFormat.Format32bppArgb;
        }
        public virtual bool LoadImageData()
        {
            if(Loaded)
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
            bmpData = bmpImage.LockBits(new Rectangle(0, 0, Width,Height),
                ImageLockMode.ReadOnly, Format);

        }
        public IntPtr Scan0
        {
            get
            {
                if(!NowLock)
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

        private void Save(string path, TextureKind kind)
        {

        }

    }
}
