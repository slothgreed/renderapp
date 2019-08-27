using KI.Asset.Attribute;
using KI.Presenter.ViewModel;

namespace RenderApp.ViewModel
{
    class WireFrameAttributeViewModel : AttributeViewModel
    {
        /// <summary>
        /// モデル
        /// </summary>
        private WireFrameAttribute Model;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        /// <param name="model">モデル</param>
        public WireFrameAttributeViewModel(ViewModelBase parent, WireFrameAttribute model)
                : base(parent, model)
        {
            Model = model;
        }

        private System.Windows.Media.Color selectColor;

        public System.Windows.Media.Color SelectColor
        {
            get
            {
                return selectColor;
            }

            set
            {
                selectColor = value;
                Model.Color = new OpenTK.Vector4(selectColor.R / 255.0f, selectColor.G / 255.0f, selectColor.B / 255.0f, 1.0f);
                OnPropertyChanged(nameof(SelectColor));
            }
        }
    }
}
