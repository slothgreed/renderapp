using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer.Attribute;
using KI.UI.ViewModel;

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
                return Model.Offset;
            }

            set
            {
                Model.Offset = value;
                OnPropertyChanged(nameof(Offset));
            }
        }
    }
}
