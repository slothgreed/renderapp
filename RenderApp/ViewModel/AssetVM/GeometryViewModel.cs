using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using System.Collections.ObjectModel;
using RenderApp.GLUtil;
using OpenTK;
using RenderApp.ViewModel.DockTabVM;
using RenderApp.ViewModel.PropertyVM;
using KI.Foundation.ViewModel;

namespace RenderApp.ViewModel.AssetVM
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
            PropertyItem.Add("Material", new ComboItemViewModel(Model,"MaterialItem", RenderApp.Globals.Project.ActiveProject.GetObject(RAAsset.Material), 0));
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
