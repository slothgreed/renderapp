using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;

namespace RenderApp
{
    enum ImageKind
    {
        PNG,
        JPG,
        BMP,
        TGA
    }

    /// <summary>
    /// BMPで保持
    /// </summary>
    public class RAImageInfo : RAFile
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

        Bitmap bmpImage
        {
            get;
            set;
        }
        BitmapData bmpData
        {
            get;
            set;
        }
        private bool NowLock
        {
            get;
            set;
        }
        
        public RAImageInfo(string path)
            :base(path)
        {

        }
        public virtual void LoadImageData()
        {
            bmpImage = new Bitmap(FilePath);
            if (System.IO.Path.GetExtension(FilePath) == ".bmp")
            {
                bmpImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            Width = bmpImage.Width;
            Height = bmpImage.Height;
        }

        public void Lock()
        {
            NowLock = true;
            bmpData = bmpImage.LockBits(new Rectangle(0, 0, Width,Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        }
        public IntPtr Scan0
        {
            get
            {
                if(!NowLock)
                {
                    Utility.Output.Log(Utility.Output.LogLevel.Warning, "Not Lock");
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
