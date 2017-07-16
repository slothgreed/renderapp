using System;
using System.Collections.Generic;
using KI.Foundation.ViewModel;
using KI.Gfx.KIAsset;

namespace RenderApp.ViewModel
{
    public class GeometryViewModel : TabItemViewModel,IPropertyGridViewModel
    {
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
        private Dictionary<string, object> _item;
        public Dictionary<string, object> PropertyItem
        {
            get
            {
                return _item;
            }
            set
            {
                SetValue(ref _item, value);
            }
        }
        public Geometry Model
        {
            get;
            private set;
        }
        public GeometryViewModel(Geometry model)
        {
            Model = model;
            PropertyItem = new Dictionary<string, object>();
            PropertyItem.Add("Translate", new Vector3ViewModel(Model, "Translate", model.Translate));
            PropertyItem.Add("Scale", new Vector3ViewModel(Model, "Scale", model.Scale));
            PropertyItem.Add("Rotate", new Vector3ViewModel(Model, "Rotate", model.Rotate));
            PropertyItem.Add("Shader", new ComboItemViewModel(Model, "Shader", RenderApp.Globals.Project.ActiveProject.GetObject(RAAsset.Shader), 0));
            PropertyItem.Add("Visible", new CheckBoxViewModel(Model, "Visible", model.Visible));
        }

        public GeometryViewModel()
        {
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
