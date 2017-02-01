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
            STLFiles = new List<ThumbnailFileViewModel>();
            STLFiles.Add(new ThumbnailFileViewModel(new STLFile("AAA")));
            STLFiles.Add(new ThumbnailFileViewModel(new STLFile("BBB")));
            STLFiles.Add(new ThumbnailFileViewModel(new STLFile("CCC")));
        }

        private void OnSelectedSTLFile()
        {
        }
        public override void UpdateProperty()
        {
            SceneManager.Instance.AddObject(@"C:\Users\ido\Documents\GitHub\renderapp\STLBrowser\TestFolder\StanfordBunny.stl");
        }
    }
}
