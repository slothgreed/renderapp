using KI.Asset.Attribute;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    class VertexParameterAttributeViewModel : AttributeViewModel
    {
        /// <summary>
        /// モデル
        /// </summary>
        private VertexParameterAttribute Model;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        /// <param name="model">モデル</param>
        public VertexParameterAttributeViewModel(ViewModelBase parent, VertexParameterAttribute model)
                : base(parent, model)
        {
            Model = model;
            lowValue = model.Min;
            minValue = model.Min;
            maxValue = model.Max;
            heightValue = model.Max;
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
            Model.UpdateVertexColor(LowValue, HeightValue);
        }
    }
}
