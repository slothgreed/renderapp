﻿using KI.Renderer.Technique;
using KI.Presenter.ViewModel;

namespace KI.Presenter.ViewModel
{
    public class SobelViewModel : ViewModelBase
    {
        /// <summary>
        /// モデル
        /// </summary>
        private Sobel Model;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        /// <param name="model">モデル</param>
        public SobelViewModel(ViewModelBase parent, Sobel model)
                : base(parent, model)
        {
            Model = model;
        }

        public float Threshold
        {
            get
            {
                return Model.uThreshold;
            }

            set
            {
                Model.uThreshold = value;
                OnPropertyChanged(nameof(Threshold));
            }
        }
    }
}
