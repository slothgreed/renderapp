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
        
        private UInt32 imageSize;
        private BitmapData imageData;
        private UInt32 internalFormat;
        private UInt32 width;
        private UInt32 bpp;

        public UInt32 ID;
        public TGAImage(string path) :
            base(path)
        {

        }
        bool ReadTGAImage(string filename)
        {
            Stream fp = File.Open(filename, FileMode.Open);
            if (fp == null)
                return false;

            BinaryReader binary = new BinaryReader(fp);
            fp.Close();

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
            BitPerPixel = binary.ReadByte();
            Discripter = binary.ReadByte();

            
            binary.Close();

            return true;
        }
        UInt32 Load(string filename)
        {
            return 0;
        }

    }
}
