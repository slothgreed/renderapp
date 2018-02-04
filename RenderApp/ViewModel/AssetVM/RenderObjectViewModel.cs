using System.Collections.ObjectModel;
using System.Linq;
using KI.Renderer;
using KI.Renderer.Attribute;
using KI.UI.ViewModel;
using OpenTK;

namespace RenderApp.ViewModel
{
    public class RenderObjectViewModel : DockWindowViewModel
    {
        public RenderObjectViewModel(ViewModelBase parent)
            : base(parent, null, "No Geometry", Place.RightUp)
        {
        }

        public RenderObjectViewModel(ViewModelBase parent, RenderObject model)
            : base(parent, model, "No Geometry", Place.RightUp)
        {
            Model = model;

            Attributes = new ObservableCollection<ViewModelBase>();
            foreach (var attribute in model.Attributes.Where(p => (p is GeometryAttribute) == false))
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

                if (viewModel != null)
                {
                    viewModel.PropertyChanged += Collection_PropertyChanged;
                    Attributes.Add(viewModel);
                }
            }

            GeometryAttributes = new ObservableCollection<AttributeBase>();
            foreach (var attribute in model.Attributes)
            {
                GeometryAttributes.Add(attribute);
            }

            SelectedGeometryAttribute = model.GeometryAttribute;
        }

        public Vector3 Rotate
        {
            get
            {
                return Model.Rotate;
            }

            set
            {
                Model.Rotate = value;
                OnPropertyChanged(nameof(Rotate));
            }
        }

        public Vector3 Scale
        {
            get
            {
                return Model.Scale;
            }

            set
            {
                Model.Scale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }

        public Vector3 Translate
        {
            get
            {
                return Model.Translate;
            }

            set
            {
                Model.Translate = value;
                OnPropertyChanged(nameof(Translate));
            }
        }

        private RenderObject _model;
        public RenderObject Model
        {
            get
            {
                return _model;
            }

            private set
            {
                _model = value;
                Title = Model.ToString();
            }
        }

        public ObservableCollection<ViewModelBase> Attributes
        {
            get;
            set;
        }

        public ObservableCollection<AttributeBase> GeometryAttributes
        {
            get;
            set;
        }

        public AttributeBase SelectedGeometryAttribute
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
