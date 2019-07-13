using KI.Presenter.ViewModel;

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
            : base(null)
        {
            FileTreeViewModel = new FileTreeViewModel(this, @"E:\MyProgram\KIProject\renderapp\Resource");
            STLBrowserViewModel = new STLBrowserViewModel(this, @"E:\MyProgram\KIProject\renderapp\Resource");
            ViewportViewModel = new ViewportViewModel(this);
        }
    }
}
