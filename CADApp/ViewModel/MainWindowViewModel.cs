﻿using KI.UI.ViewModel;

namespace CADApp.ViewModel
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

        public MainWindowViewModel()
            : base(null)
        {
            ViewportViewModel = new ViewportViewModel(this);
        }
    }
}