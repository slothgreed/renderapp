using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Asset;
using KI.UI.ViewModel;
using OpenTK;

namespace RenderApp.ViewModel
{
    /// <summary>
    /// ライトのビューモデル
    /// </summary>
    public class LightViewModel : SceneNodeViewModel
    {
        public LightViewModel(ViewModelBase parent, Light light)
            : base(parent, light, "LightProperty", Place.RightUp)
        {
        }
    }
}
