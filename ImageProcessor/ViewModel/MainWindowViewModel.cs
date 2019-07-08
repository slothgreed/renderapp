using KI.Presenter.ViewModel;

namespace ImageProcessor.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
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

        private PropertyViewModel _PropertyViewModel;
        public PropertyViewModel PropertyViewModel
        {
            get { return _PropertyViewModel; }
            set
            {
                SetValue(ref _PropertyViewModel, value);
            }
        }


        public MainWindowViewModel()
            : base(null)
        {
            ViewportViewModel = new ViewportViewModel(this);
            ViewportViewModel.OnInitialized += ViewportViewModel_OnInitialized;
        }

        private void ViewportViewModel_OnInitialized(object sender, System.EventArgs e)
        {
            PropertyViewModel = new PropertyViewModel(this, ViewportViewModel.RenderSystem);
            PropertyViewModel.PropertyChanged += PropertyViewModel_PropertyChanged;
        }

        private void PropertyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }
    }
}
