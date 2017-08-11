using System;
using System.Collections.Generic;
using KI.Asset;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class GeometryViewModel : TabItemViewModel, IPropertyGridViewModel
    {
        private Dictionary<string, object> item;

        public GeometryViewModel()
        {
        }

        public GeometryViewModel(Geometry model)
        {
            Model = model;
            PropertyItem = new Dictionary<string, object>();
            //PropertyItem.Add("Translate", new Vector3ViewModel(Model, "Translate", model.Translate));
            //PropertyItem.Add("Scale", new Vector3ViewModel(Model, "Scale", model.Scale));
            //PropertyItem.Add("Rotate", new Vector3ViewModel(Model, "Rotate", model.Rotate));
            //PropertyItem.Add("Shader", new ComboItemViewModel(Model, "Shader", RenderApp.Globals.Project.ActiveProject.GetObject(RAAsset.Shader), 0));
            //PropertyItem.Add("Visible", new CheckBoxViewModel(Model, "Visible", model.Visible));
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

        public Geometry Model { get; private set; }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
