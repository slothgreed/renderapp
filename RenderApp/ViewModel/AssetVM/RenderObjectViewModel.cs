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
    public class RenderObjectViewModel : TabItemViewModel
    {
        private ICommand _CalculateVertexCurvature;
        public ICommand CalculateVertexCurvature
        {
            get
            {
                if (_CalculateVertexCurvature == null)
                {
                    return _CalculateVertexCurvature = CreateCommand(CalculateVertexCurvatureCommand);
                }

                return _CalculateVertexCurvature;
            }
        }

        private void CalculateVertexCurvatureCommand()
        {
            var command = new CalculateVertexCurvature(Workspace.MainScene.SelectNode);
            KI.Foundation.Command.CommandManager.Instance.Execute(command, null, true);
        }

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
                if (Model.Polygon is HalfEdgeDS)
                {
                    ((HalfEdgeDS)Model.Polygon).UpdateVertexColor(SelectedItem, MinValue, MaxValue);
                }
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
            ((HalfEdgeDS)Model.Polygon).UpdateVertexColor(SelectedItem, LowValue, HeightValue);
        }

        public RenderObject Model { get; private set; }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
