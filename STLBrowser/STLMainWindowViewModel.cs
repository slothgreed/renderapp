using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.ViewModel;
namespace STLBrowser.ViewModel
{
    public class STLMainWindowViewModel : ViewModelBase
    {
        private FileTreeViewModel _FileTreeViewModel;
        public FileTreeViewModel FileTreeViewModel
        {
            get
            {
                return _FileTreeViewModel;
            }
            set
            {
                SetValue(ref _FileTreeViewModel, value);
            }
        }
        private STLBrowserViewModel _STLBrowserViewModel;
        public STLBrowserViewModel STLBrowserViewModel
        {
            get
            {
                return _STLBrowserViewModel;
            }
            set
            {
                _STLBrowserViewModel = value;
            }
        }
        public STLMainWindowViewModel()
        {
            FileTreeViewModel = new FileTreeViewModel(@"C:\Users\ido\Documents\GitHub\RenderApp");
            STLBrowserViewModel = new STLBrowserViewModel(@"C:\Users\ido\Documents\GitHub\RenderApp");
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
