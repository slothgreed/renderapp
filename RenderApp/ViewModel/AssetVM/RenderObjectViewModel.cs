using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Foundation.Core;
using KI.Foundation.Parameter;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.UI.ViewModel;
using OpenTK;

namespace RenderApp.ViewModel
{
    public class RenderObjectViewModel : TabItemViewModel
    {
        public RenderObjectViewModel(ViewModelBase parent)
            : base(parent)
        {
        }

        public RenderObjectViewModel(ViewModelBase parent, RenderObject model)
            : base(parent)
        {
            Model = model;
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
            }
        }

        public RenderMode SelectedRenderMode
        {
            get
            {
                return Model.RenderMode;
            }

            set
            {
                Model.RenderMode = value;
            }
        }

        public override string Title
        {
            get
            {
                if (Model == null)
                {
                    return "UnkownGeometry";
                }
                else
                {
                    return Model.ToString();
                }
            }
        }

        private VertexColor selectedItem;
        public VertexColor SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                selectedItem = value;

                var halfEdgeDS = Model.Polygon as HalfEdgeDS;
                if (halfEdgeDS.Parameter.ContainsKey(SelectedItem.ToString()))
                {
                    var param = halfEdgeDS.Parameter[SelectedItem.ToString()] as ScalarParameter;
                    maxValue = param.Max;
                    minValue = param.Min;
                    OnPropertyChanged(nameof(MaxValue));
                    OnPropertyChanged(nameof(MinValue));
                }

                halfEdgeDS.UpdateVertexColor(SelectedItem, MinValue, MaxValue);
            }
        }


        private float minValue;
        public float MinValue
        {
            get
            {
                return minValue;
            }

            set
            {
                minValue = value;
                if (Model.Polygon is HalfEdgeDS)
                {
                    ((HalfEdgeDS)Model.Polygon).UpdateVertexColor(SelectedItem, MinValue, MaxValue);
                }
            }
        }

        private float maxValue;
        public float MaxValue
        {
            get
            {
                return maxValue;
            }

            set
            {
                maxValue = value;
                if(Model.Polygon is HalfEdgeDS)
                {
                    ((HalfEdgeDS)Model.Polygon).UpdateVertexColor(SelectedItem, MinValue, MaxValue);
                }
            }
        }

        public RenderObject Model { get; private set; }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
