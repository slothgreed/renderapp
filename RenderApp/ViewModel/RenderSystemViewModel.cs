using KI.Renderer;

namespace RenderApp.ViewModel
{
    public class RenderSystemViewModel : TabItemViewModel
    {
        public RenderSystem Model
        {
            get;
            set;
        }
        private int _textureIndex;
        public int TextureIndex
        {
            get
            {
                return _textureIndex;
            }
            private set
            {
                SetValue<int>(ref _textureIndex, value);
                Model.OutputTexture = Model.ProcessingTexture[value];
            }
        }
        public RenderSystemViewModel(RenderSystem _model)
        {
            Model = _model;
            TextureIndex = Model.ProcessingTexture.IndexOf(Model.OutputTexture);
        }

        public override string Title
        {
            get
            {
                return "RenderSystem";
            }
        }



        public override void UpdateProperty()
        {
        }
    }
}
