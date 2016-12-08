using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
namespace RenderApp.Utility
{


    class TGAImage : RAImageInfo
    {
        struct TGAStruct
        {
            public Byte Low;
            public Byte High;
        }
        private Byte IDFieldLength;
        private Byte ColorMapType;
        private Byte ImageType;
        private TGAStruct ColorMapIndex;
        private TGAStruct ColorMapLength;
        private Byte ColorMap_Size;
        private TGAStruct ImageOriginX;
        private TGAStruct ImageOriginY;
        private TGAStruct ImageWidth;
        private TGAStruct ImageHeight;
        private Byte BitPerPixel;
        private Byte Discripter;
        
        private int imageSize;

        public UInt32 ID;
        public TGAImage(string path) :
            base(path)
        {

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

            Byte[] rgb = binary.ReadBytes(imageSize);

            bmpImage = new Bitmap(Width, Height);

            int counter = 0;
            for (int i = 0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    bmpImage.SetPixel(i, j, Color.FromArgb(rgb[3 * counter + 2], rgb[3 * counter + 1], rgb[3 * counter]));
                    counter++;
                }
            }

            binary.Close();
            fp.Close();
            Loaded = true;
            return true;
        }
        private int TGAStructValue(TGAStruct data)
        {
            return data.High * 256 + data.Low;
        }
        public override bool LoadImageData()
        {
            return ReadTGAImage(FilePath);
        }

    }
}
