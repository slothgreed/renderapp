using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class RendererViewModel : TabItemViewModel
    {
        private IRenderer model;
        public IRenderer Model
        {
            get
            {
                return model;
            }
            set
            {
                model =  value;
                TextureIndex = Model.ProcessingTexture.IndexOf(Model.OutputTexture);
                OnPropertyChanged(nameof(Model));
            }
        }

        private int textureIndex;

        public int TextureIndex
        {
            get
            {
                return textureIndex;
            }

            private set
            {
                textureIndex = value;
                Model.OutputTexture = Model.ProcessingTexture[value];
                OnPropertyChanged(nameof(TextureIndex));
            }
        }

        public bool PostProcessMode
        {
            get
            {
                return Model.PostProcessMode;
            }
            set
            {
                Model.PostProcessMode =  value;
                OnPropertyChanged(nameof(PostProcessMode));
            }
        }


        public RendererViewModel(ViewModelBase parent)
            : base(parent)
        {
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
