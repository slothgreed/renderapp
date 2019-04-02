using KI.Asset.Technique;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class BloomViewModel : ViewModelBase
    {
        /// <summary>
        /// モデル
        /// </summary>
        private Bloom Model;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        /// <param name="model">モデル</param>
        public BloomViewModel(ViewModelBase parent, Bloom model)
                : base(parent, model)
        {
            Model = model;
        }

        public float Scale
        {
            get
            {
                return Model.uScale;
            }

            set
            {
                Model.uScale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }

        public float Sigma
        {
            get
            {
                return Model.Sigma;
            }

            set
            {
                Model.Sigma = value;
                Model.UpdateWeight();
                OnPropertyChanged(nameof(Sigma));
            }
        }

        public float Max { get { return Model.Max; } }

        public float Min { get { return Model.Min; } }

    }
}
