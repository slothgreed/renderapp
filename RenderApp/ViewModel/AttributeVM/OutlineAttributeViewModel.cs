using KI.Asset.Attribute;
using KI.Presentation.ViewModel;

namespace RenderApp.ViewModel
{
    class OutlineAttributeViewModel : AttributeViewModel
    {
        /// <summary>
        /// モデル
        /// </summary>
        private OutlineAttribute Model;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        /// <param name="model">モデル</param>
        public OutlineAttributeViewModel(ViewModelBase parent, OutlineAttribute model)
                : base(parent, model)
        {
            Model = model;
        }

        public float Offset
        {
            get
            {
                return Model.uOffset;
            }

            set
            {
                Model.uOffset = value;
                OnPropertyChanged(nameof(Offset));
            }
        }
    }
}
