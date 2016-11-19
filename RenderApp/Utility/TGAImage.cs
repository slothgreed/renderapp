using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace RenderApp.Utility
{
    class TGAImage
    {
        private UInt32 imageSize;
        private Byte imageData;
        private UInt32 internalFormat;
        private UInt32 width;
        private UInt32 bpp;

        public UInt32 ID;
        public TGAImage()
        {

        }
        bool ReadTGAImage(string filename)
        {
            return false;
        }
        UInt32 Load(string filename)
        {
            return 0;
        }

    }
}
