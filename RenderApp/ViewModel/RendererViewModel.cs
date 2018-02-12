using System.Collections.ObjectModel;
using KI.Asset;
using KI.Asset.Technique;
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
                    ViewModelBase viewModel = null;
                    if (postprocess is Bloom)
                    {
                        viewModel = new BloomViewModel(this, postprocess as Bloom);
                    }
                    else if (postprocess is Sobel)
                    {
                        viewModel = new SobelViewModel(this, postprocess as Sobel);
                    }

                    viewModel.PropertyChanged += CollectionPropertyChanged;
                    postItem.Add(viewModel);
                }

                PostProcesses = postItem;

                OnPropertyChanged(nameof(Model));
            }
        }

        private void CollectionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PostProcesses));
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
                if (Model.PostProcessMode != value)
                {
                    Model.PostProcessMode = value;
                    OnPropertyChanged(nameof(EnablePostEffect));
                }
            }
        }

        private ObservableCollection<ViewModelBase> postProcesses = new ObservableCollection<ViewModelBase>();
        public ObservableCollection<ViewModelBase> PostProcesses
        {
            get
            {
                return postProcesses;
            }
            set
            {
                postProcesses = value;
                OnPropertyChanged(nameof(PostProcesses));
            }
        }


        public RendererViewModel(ViewModelBase parent)
            : base(parent, null, "Renderer", Place.LeftDown)
        {
        }
    }
}
