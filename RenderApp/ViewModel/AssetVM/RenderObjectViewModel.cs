using System;
using System.Windows.Input;
using KI.Analyzer;
using KI.Foundation.Parameter;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.Tool.Command;
using KI.UI.ViewModel;
using OpenTK;
using RenderApp.Globals;

namespace RenderApp.ViewModel
{
    public class RenderObjectViewModel : DockWindowViewModel
    {
        public RenderObjectViewModel(ViewModelBase parent)
            : base(parent,"No Geometry", Place.RightUp)
        {
        }

        public RenderObjectViewModel(ViewModelBase parent, RenderObject model)
            : base(parent, "No Geometry", Place.RightUp)
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

        public RenderMode SelectedRenderMode
        {
            get
            {
                return Model.RenderMode;
            }

            set
            {
                Model.RenderMode = value;
                OnPropertyChanged(nameof(SelectedRenderMode));
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
                SetValue(ref selectedItem, value);

                var halfEdgeDS = Model.Polygon as HalfEdgeDS;
                if (halfEdgeDS.Parameter.ContainsKey(SelectedItem.ToString()))
                {
                    var param = halfEdgeDS.Parameter[SelectedItem.ToString()] as ScalarParameter;
                    maxValue = param.Max;
                    minValue = param.Min;
                    OnPropertyChanged(nameof(MaxValue));
                    OnPropertyChanged(nameof(MinValue));
                }

                ParamSlider_ValueChanged();
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
                SetValue(ref minValue, value);
                ParamSlider_ValueChanged();
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
                SetValue(ref maxValue, value);
                ParamSlider_ValueChanged();
            }
        }

        private float lowValue;
        public float LowValue
        {
            get
            {
                return lowValue;
            }
            set
            {
                SetValue(ref lowValue, value);
                ParamSlider_ValueChanged();
            }
        }

        private float heightValue;
        public float HeightValue
        {
            get
            {
                return heightValue;
            }
            set
            {
                SetValue(ref heightValue, value);
                ParamSlider_ValueChanged();
            }
        }

        /// <summary>
        /// パラメータスライダの値変更イベント
        /// </summary>
        private void ParamSlider_ValueChanged()
        {
            if (Model.Polygon is HalfEdgeDS)
            {
                ((HalfEdgeDS)Model.Polygon).UpdateVertexColor(SelectedItem, LowValue, HeightValue);
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

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
