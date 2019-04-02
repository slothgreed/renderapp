using KI.Asset.Attribute;
using KI.UI.ViewModel;

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
                var color = new OpenTK.Vector3(selectColor.R / 255.0f, selectColor.G / 255.0f, selectColor.B / 255.0f);
                Model.UpdateColor(color);
                OnPropertyChanged(nameof(SelectColor));
            }
        }
    }
}
