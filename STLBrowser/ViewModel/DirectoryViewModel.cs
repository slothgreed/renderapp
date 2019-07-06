using System.Collections.Generic;
using System.IO;
using KI.Presentation.ViewModel;

namespace STLBrowser.ViewModel
{
    public class DirectoryViewModel : ViewModelBase 
    {
        private string _filename;
        public string FileName
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }

        public List<DirectoryViewModel> Directorys
        {
            get;
            set;
        }

        public DirectoryViewModel(ViewModelBase parent, string path)
            : base(parent)
        {
            FileName = Path.GetFileName(path);
            Directorys = new List<DirectoryViewModel>();

            if (Directory.Exists(path))
            {
                string[] directory = System.IO.Directory.GetDirectories(path, "*");
                foreach (var dir in directory)
                {
                    Directorys.Add(new DirectoryViewModel(this, dir));
                }
                string[] files = System.IO.Directory.GetFiles(path);
                foreach (var file in files)
                {
                    Directorys.Add(new DirectoryViewModel(this, file));
                }
            }
        }
    }
}
