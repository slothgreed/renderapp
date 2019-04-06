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
        public LightViewModel(ViewModelBase parent, Light light)
            : base(parent, light, "LightProperty", Place.RightUp)
        {
        }
    }
}
