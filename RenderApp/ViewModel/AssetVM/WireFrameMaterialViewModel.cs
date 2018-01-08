using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer.Material;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    class WireFrameMaterialViewModel : MaterialViewModel
    {
        /// <summary>
        /// モデル
        /// </summary>
        private WireFrameMaterial Model;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        /// <param name="model">モデル</param>
        public WireFrameMaterialViewModel(ViewModelBase parent, WireFrameMaterial model)
                : base(parent, model)
        {
            Model = model;
        }

        private System.Windows.Media.Color selectColor;

        public System.Windows.Media.Color SelectColor
        {
            get
            {
                return selectColor;
            }
            set
            {
                selectColor = value;
                var color = new OpenTK.Vector3(selectColor.R / 255.0f, selectColor.G / 255.0f, selectColor.B / 255.0f);
                Model.UpdateColor(color);
            }
        }
    }
}
