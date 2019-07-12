using KI.Gfx.KITexture;
using KI.Presenter.ViewModel;
using KI.Renderer;
using KI.Renderer.Technique;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderTraining.ViewModel
{
    public class PropertyViewModel : ViewModelBase
    {
        private RenderSystem _model;
        public RenderSystem Model
        {
            get { return _model; }
            set
            {
                SetValue(ref _model, value);
            }
        }

        public List<Texture> Textures
        {
            get
            {
                return Model.ProcessingTexture;
            }
        }

        public int SelectedTextureIndex
        {
            get
            {
                return Model.ProcessingTexture.IndexOf(Model.OutputTexture);
            }

            set
            {
                Model.OutputTexture = Model.ProcessingTexture[value];
                OnPropertyChanged(nameof(SelectedTextureIndex));
            }
        }

        private ObservableCollection<ViewModelBase> postProcess = new ObservableCollection<ViewModelBase>();
        public ObservableCollection<ViewModelBase> PostProcessViewModel
        {
            get { return postProcess; }
            set
            {
                SetValue(ref postProcess, value);
            }
        }


        public PropertyViewModel(ViewModelBase parent, RenderSystem model)
            : base(parent)
        {
            Model = model;

            foreach (var pfx in Model.PostEffect.Items)
            {
                ViewModelBase viewModel = null;
                if (pfx is Bloom)
                {
                    viewModel = new BloomViewModel(this, (Bloom)pfx);
                }
                else if(pfx is Sobel)
                {
                    viewModel = new SobelViewModel(this, (Sobel)pfx);
                }

                if (viewModel != null)
                {
                    viewModel.PropertyChanged += PostProcessPropertyChanged;
                    postProcess.Add(viewModel);
                }
            }
        }

        private void PostProcessPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PostProcessViewModel));
        }
    }
}
