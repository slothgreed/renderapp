using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class RenderObjectViewModel : TabItemViewModel, IPropertyGridViewModel
    {
        private Dictionary<string, object> item;

        public RenderObjectViewModel()
        {
        }

        public RenderObjectViewModel(RenderObject model)
        {
            Model = model;
            PropertyItem = new Dictionary<string, object>();
            PropertyItem.Add("Translate", new Vector3ViewModel(Model, "Translate", model.Translate));
            PropertyItem.Add("Scale", new Vector3ViewModel(Model, "Scale", model.Scale));
            PropertyItem.Add("Rotate", new Vector3ViewModel(Model, "Rotate", model.Rotate));
            PropertyItem.Add("Shader", new ComboItemViewModel(Model, "Shader", Globals.Project.ActiveProject.GetObject(RAAsset.Shader), 0));
            PropertyItem.Add("Visible", new CheckBoxViewModel(Model, "Visible", model.Visible));
            PropertyItem.Add("Mode", new ComboItemViewModel(Model, "Mode", Enum.GetValues(typeof(RenderMode)).Cast<object>()));
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

        public Dictionary<string, object> PropertyItem
        {
            get
            {
                return item;
            }

            set
            {
                SetValue(ref item, value);
            }
        }

        public RenderObject Model { get; private set; }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
