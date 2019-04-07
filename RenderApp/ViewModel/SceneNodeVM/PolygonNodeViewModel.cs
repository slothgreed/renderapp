using System.Collections.ObjectModel;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class PolygonNodeViewModel : SceneNodeViewModel
    {
        public PolygonNodeViewModel(ViewModelBase parent)
            : base(parent, null, "No Geometry", Place.RightUp)
        {
        }

        public PolygonNodeViewModel(ViewModelBase parent, PolygonNode model)
            : base(parent, model, "No Geometry", Place.RightUp)
        {
            Attributes = new ObservableCollection<ViewModelBase>();

            foreach (var attribute in model.Attributes)
            {
                ViewModelBase viewModel = null;
                if (attribute is VertexParameterAttribute)
                {
                    viewModel = new VertexParameterAttributeViewModel(this, attribute as VertexParameterAttribute);
                }
                else if (attribute is WireFrameAttribute)
                {
                    viewModel = new WireFrameAttributeViewModel(this, attribute as WireFrameAttribute);
                }
                else if (attribute is OutlineAttribute)
                {
                    viewModel = new OutlineAttributeViewModel(this, attribute as OutlineAttribute);
                }
                else if (attribute is SplitAttribute)
                {
                    viewModel = new SplitAttributeViewModel(this, attribute as SplitAttribute);
                }
                else
                {
                    viewModel = new DefaultAttributeViewModel(this, attribute);
                }


                if (viewModel != null)
                {
                    viewModel.PropertyChanged += Collection_PropertyChanged;
                    Attributes.Add(viewModel);
                }
            }
        }

        public ObservableCollection<ViewModelBase> Attributes
        {
            get;
            set;
        }

        private void Collection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Attributes));
        }

    }
}
