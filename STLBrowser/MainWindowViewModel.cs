using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.ViewModel;
namespace STLBrowser.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
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
                SetValue(ref _STLBrowserViewModel, value);
            }
        }
        private ViewportViewModel _ViewportViewModel;
        public ViewportViewModel ViewportViewModel
        {
            get
            {
                return _ViewportViewModel;
            }
            set
            {
                SetValue(ref _ViewportViewModel, value);
            }
        }
        public MainWindowViewModel()
        {
            FileTreeViewModel = new FileTreeViewModel(@"C:\Users\ido\Documents\KIProject\RenderApp");
            STLBrowserViewModel = new STLBrowserViewModel(@"C:\Users\ido\Documents\KIProject\RenderApp");
            ViewportViewModel = new ViewportViewModel();
        }
    }
}
