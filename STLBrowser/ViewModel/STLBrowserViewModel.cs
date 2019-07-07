using System.Collections.Generic;
using System.IO;
using KI.Presenter.ViewModel;

namespace STLBrowser.ViewModel
{
    public class STLBrowserViewModel : ViewModelBase
    {
        private List<ThumbnailFileViewModel> _STLFiles;
        public List<ThumbnailFileViewModel> STLFiles
        {
            get
            {
                return _STLFiles;
            }

            set
            {
                SetValue(ref _STLFiles, value);
            }
        }

        public STLBrowserViewModel(ViewModelBase parent, string path)
            : base(parent)
        {
            path = @"C:\Users\ido\Documents\KIProject\renderapp\RenderApp\Resource\Model";

            STLFiles = new List<ThumbnailFileViewModel>();


            if (Directory.Exists(path))
            {
                string[] directory = System.IO.Directory.GetDirectories(path, "*");
                string[] files = System.IO.Directory.GetFiles(path);
                foreach (var file in files)
                {
                    if (Path.GetExtension(file) == ".stl")
                    {
                        STLFiles.Add(new ThumbnailFileViewModel(this, Path.GetFileName(file), file));
                    }
                }
            }
        }

        private void OnSelectedSTLFile()
        {
        }
    }
}
