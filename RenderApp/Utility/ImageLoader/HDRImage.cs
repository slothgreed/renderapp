using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace RenderApp.Utility
{
    public class HDRImage : RAImageInfo
    {
        public HDRImage(string path)
            :base(path)
        {

        }

        private bool LoadHDRImageData()
        {
            Stream reader = File.Open(FilePath, FileMode.Open);
            if (reader == null)
                return false;
            


            return true;
        }



        public override bool LoadImageData()
        {
            return LoadHDRImageData();
        }

    }
}
