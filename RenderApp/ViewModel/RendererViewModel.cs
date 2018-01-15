using System.Collections.ObjectModel;
using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class RendererViewModel : DockWindowViewModel
    {
        private Renderer model;
        public Renderer Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
                TextureIndex = Model.ProcessingTexture.IndexOf(Model.OutputTexture);

                var postItem = new ObservableCollection<ViewModelBase>();
                foreach (var postprocess in model.PostEffect.Items)
                {
                    if (postprocess is Bloom)
                    {
                        postItem.Add(new BloomViewModel(this, postprocess as Bloom));
                    }
                    else if(postprocess is Sobel)
                    {
                        postItem.Add(new SobelViewModel(this, postprocess as Sobel));
                    }
                }

                PostProcessItem = postItem;
                OnPropertyChanged(nameof(PostProcessItem));

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

        public bool EnablePostEffect
        {
            get
            {
                if (Model == null)
                {
                    return false;
                }

                return Model.PostProcessMode;
            }
            set
            {
                Model.PostProcessMode = value;
                OnPropertyChanged(nameof(EnablePostEffect));
            }
        }

        public ObservableCollection<ViewModelBase> PostProcessItem
        {
            get;
            set;
        }


        public RendererViewModel(ViewModelBase parent)
            : base(parent, null, "Renderer", Place.LeftDown)
        {
        }

        public override void UpdateProperty()
        {
        }
    }
}
