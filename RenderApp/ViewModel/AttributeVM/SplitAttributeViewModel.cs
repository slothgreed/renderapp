using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Asset.Attribute;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class SplitAttributeViewModel : ViewModelBase
    {
        /// <summary>
        /// モデル
        /// </summary>
        private SplitAttribute Model;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        /// <param name="model">モデル</param>
        public SplitAttributeViewModel(ViewModelBase parent, SplitAttribute model)
                : base(parent, model)
        {
            Model = model;
        }

        public int Outer
        {
            get
            {
                return Model.uOuter;
            }

            set
            {
                Model.uOuter = value;
                OnPropertyChanged(nameof(Outer));
            }
        }

        public int Inner
        {
            get
            {
                return Model.uInner;
            }

            set
            {
                Model.uInner = value;
                OnPropertyChanged(nameof(Inner));
            }
        }
    }
}
