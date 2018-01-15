using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
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
    }
}
