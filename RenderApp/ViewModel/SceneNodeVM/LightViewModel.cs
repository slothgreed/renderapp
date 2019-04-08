using KI.Asset;
using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    /// <summary>
    /// ライトのビューモデル
    /// </summary>
    public class LightViewModel : SceneNodeViewModel
    {
        public LightViewModel(ViewModelBase parent, LightNode light)
            : base(parent, light, "LightProperty", Place.RightUp)
        {
        }
    }
}
