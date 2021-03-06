﻿using System.Collections.ObjectModel;
using KI.Renderer;
using KI.Renderer.Technique;
using KI.Presenter.ViewModel;

namespace RenderApp.ViewModel
{
    public class RendererViewModel : DockWindowViewModel
    {
        private RenderSystem model;
        public RenderSystem Model
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

                    if (viewModel != null)
                    {
                        viewModel.PropertyChanged += CollectionPropertyChanged;
                        postItem.Add(viewModel);
                    }
                }

                PostProcesses = postItem;
                EnablePostEffect = model.PostProcessMode;
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
                Model.PostProcessMode = value;
                OnPropertyChanged(nameof(EnablePostEffect));
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
