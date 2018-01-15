using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer;
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
    }
}
