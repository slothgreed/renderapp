using KI.Presenter.ViewModel;
using System;
using System.ComponentModel;

namespace ShaderTraining.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
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

        private void ViewportViewModel_OnInitialized(object sender, EventArgs e)
        {
            PropertyViewModel = new PropertyViewModel(this, ViewportViewModel.RenderSystem);
            PropertyViewModel.PropertyChanged += PropertyViewModel_PropertyChanged;
        }

        private void PropertyViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }

        private void PlaneObjectCommand()
        {
            ViewportViewModel.ChangeVisible(VisibleItem.Plane);
        }

        private void SphereObjectCommand()
        {
            ViewportViewModel.ChangeVisible(VisibleItem.Sphere);
        }
    }
}
