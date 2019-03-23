using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    /// <summary>
    /// PropertyGrid の ViewModel
    /// </summary>
    public class PropertyGridViewModel : DockWindowViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        /// <param name="model">モデル</param>
        public PropertyGridViewModel(ViewModelBase parent, object model)
            : base(parent, model, "PropertyGrid", Place.RightDown)
        {

        }
    }
}
