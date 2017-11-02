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
                SetValue(ref model, value);
                TextureIndex = Model.ProcessingTexture.IndexOf(Model.OutputTexture);
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
                SetValue<int>(ref textureIndex, value);
                Model.OutputTexture = Model.ProcessingTexture[value];
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
