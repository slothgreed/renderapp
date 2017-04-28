using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KI.Foundation.ViewModel;
using STLBrowser.Model;
using RenderApp;
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

        public STLBrowserViewModel(string path)
        {
            path = @"C:\Users\ido\Documents\KIProject\renderapp\RenderApp\Resource\Model";

            STLFiles = new List<ThumbnailFileViewModel>();


            if (Directory.Exists(path))
            {
                string[] directory = System.IO.Directory.GetDirectories(path, "*");
                string[] files = System.IO.Directory.GetFiles(path);
                foreach (var file in files)
                {
                    if(Path.GetExtension(file) == ".stl")
                    {
                        STLFiles.Add(new ThumbnailFileViewModel(Path.GetFileName(file), file));
                    }
                }
            }
        }

        private void OnSelectedSTLFile()
        {
        }
        public override void UpdateProperty()
        {
            SceneManager.Instance.AddObject(@"C:\Users\ido\Documents\KIProject\renderapp\STLBrowser\TestFolder\StanfordBunny.stl");
        }
    }
}
