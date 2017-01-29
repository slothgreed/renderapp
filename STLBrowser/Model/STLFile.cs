using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace STLBrowser.Model
{
    public class STLFile
    {
        public string FullPath
        {
            get;
            private set;
        }

        public string FileName
        {
            get;
            private set;
        }

        public BitmapImage Thumbnail
        {
            get;
            private set;
        }

        public STLInfo Info
        {
            get;
            private set;
        }

        public STLFile(string path)
        {
            FullPath = path;
            FileName = path;
            Info = new STLInfo(path);
        }

    }
}
