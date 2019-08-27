using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using KI.Gfx.KITexture;

namespace KI.Asset
{
    /// <summary>
    /// TGA画像
    /// </summary>
    public class TGAImage : ImageInfo
    {
        struct TGAStruct
        {
            public byte Low;
            public byte High;
        }

        private byte IDFieldLength;
        private byte ColorMapType;
        private byte ImageType;
        private TGAStruct ColorMapIndex;
        private TGAStruct ColorMapLength;
        private byte ColorMap_Size;
        private TGAStruct ImageOriginX;
        private TGAStruct ImageOriginY;
        private TGAStruct ImageWidth;
        private TGAStruct ImageHeight;
        private byte BitPerPixel;
        private byte Discripter;

        private int imageSize;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">名前</param>
        public TGAImage(string name) :
            base(name)
        {
        }

        public override bool Load(string filePath)
        {
            return ReadTGAImage(filePath);
        }

        private void ReadHeaderData(BinaryReader binary)
        {
            IDFieldLength = binary.ReadByte();
            ColorMapType = binary.ReadByte();
            ImageType = binary.ReadByte();
            ColorMapIndex.Low = binary.ReadByte();
            ColorMapIndex.High = binary.ReadByte();
            ColorMapLength.Low = binary.ReadByte();
            ColorMapLength.High = binary.ReadByte();
            ColorMap_Size = binary.ReadByte();
            ImageOriginX.Low = binary.ReadByte();
            ImageOriginX.High = binary.ReadByte();
            ImageOriginY.Low = binary.ReadByte();
            ImageOriginY.High = binary.ReadByte();
            ImageWidth.Low = binary.ReadByte();
            ImageWidth.High = binary.ReadByte();
            ImageHeight.Low = binary.ReadByte();
            ImageHeight.High = binary.ReadByte();
            BitPerPixel = binary.ReadByte();
            Discripter = binary.ReadByte();
        }

        private bool ReadTGAImage(string filename)
        {
            Stream fp = File.Open(filename, FileMode.Open);
            if (fp == null)
                return false;

            BinaryReader binary = new BinaryReader(fp);
            ReadHeaderData(binary);

            Width = TGAStructValue(ImageWidth);
            Height = TGAStructValue(ImageHeight);
            imageSize = Width * Height * BitPerPixel;

            byte[] rgb = binary.ReadBytes(imageSize);

            if (BitPerPixel == 24)
            {
                Format = PixelFormat.Format24bppRgb;
                BmpImage = new Bitmap(Width, Height, Width * 3, Format, Marshal.UnsafeAddrOfPinnedArrayElement(rgb, 0));
            }
            else if (BitPerPixel == 32)
            {
                Format = PixelFormat.Format32bppArgb;
                BmpImage = new Bitmap(Width, Height, Width * 4, Format, Marshal.UnsafeAddrOfPinnedArrayElement(rgb, 0));
            }
            else if (BitPerPixel == 8)
            {
                Format = PixelFormat.Format8bppIndexed;
                BmpImage = new Bitmap(Width, Height, Width, Format, Marshal.UnsafeAddrOfPinnedArrayElement(rgb, 0));
            }

            binary.Close();
            fp.Close();
            return true;
        }

        private int TGAStructValue(TGAStruct data)
        {
            return data.High * 256 + data.Low;
        }
    }
}
