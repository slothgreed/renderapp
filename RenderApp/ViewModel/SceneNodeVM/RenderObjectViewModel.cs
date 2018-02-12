﻿using System.Collections.ObjectModel;
using System.Linq;
using KI.Renderer;
using KI.Renderer.Attribute;
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

            SelectedGeometryAttribute = model.PolygonAttribute;
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
            get
            {
                return ((RenderObject)Model).PolygonAttribute;
            }
            set
            {
                ((RenderObject)Model).PolygonAttribute = value;
            }
        }

        private void Collection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Attributes));
        }

    }
}
