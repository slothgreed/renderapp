using System.Windows.Media.Imaging;

namespace STLBrowser.Model
{
    public class STLFile
    {
        public string FilePath
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
            FilePath = path;
            FileName = path;
            Info = new STLInfo(path);
        }

    }
}
