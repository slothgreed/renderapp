using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KI.Analyzer;
using KI.Foundation.Parameter;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.Renderer.Attribute;
using KI.Tool.Command;
using KI.UI.ViewModel;
using OpenTK;
using RenderApp.Globals;

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
                if (attribute is VertexParameterAttribute)
                {
                    Attributes.Add(new VertexParameterAttributeViewModel(this, attribute as VertexParameterAttribute));
                }
                else if(attribute is WireFrameAttribute)
                {
                    Attributes.Add(new WireFrameAttributeViewModel(this, attribute as WireFrameAttribute));
                }
            }
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

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
