using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KI.Foundation.ViewModel;
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

        public DirectoryViewModel(string path)
        {
            FileName = Path.GetFileName(path);
            Directorys = new List<DirectoryViewModel>();


            if (Directory.Exists(path))
            {
                string[] directory = System.IO.Directory.GetDirectories(path, "*");
                foreach (var dir in directory)
                {
                    Directorys.Add(new DirectoryViewModel(dir));
                }
                string[] files = System.IO.Directory.GetFiles(path);
                foreach (var file in files)
                {
                    Directorys.Add(new DirectoryViewModel(file));
                }
            }
        }
    }
}
