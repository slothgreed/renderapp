using System.Collections.ObjectModel;
using System.Linq;
using KI.Asset;
using KI.Asset.Attribute;
using KI.UI.ViewModel;
using OpenTK;

namespace RenderApp.ViewModel
{
    public class RenderObjectViewModel : SceneNodeViewModel
    {
        public RenderObjectViewModel(ViewModelBase parent)
            : base(parent, null, "No Geometry", Place.RightUp)
        {
        }

        public RenderObjectViewModel(ViewModelBase parent, RenderObject model)
            : base(parent, model, "No Geometry", Place.RightUp)
        {
            Attributes = new ObservableCollection<ViewModelBase>();

            foreach (var attribute in model.Attributes.Where(p => (p is PolygonAttribute) == false))
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
