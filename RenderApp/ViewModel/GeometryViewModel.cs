using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using System.Collections.ObjectModel;
using RenderApp.GLUtil;
using OpenTK;
namespace RenderApp.ViewModel
{
    public class GeometryViewModel : TabItemViewModel
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
        public Dictionary<string, object> Item
        {
            get
            {
                return _item;
            }
            private set
            {
                SetValue(ref _item, value);
            }
        }
        public PropertyGridViewModel Property
        {
            get;
            private set;
        }
        public Geometry Model
        {
            get;
            private set;
        }
        public GeometryViewModel(Geometry model)
        {
            Model = model;
            _item = new Dictionary<string, object>();
            _item.Add("Translate", model.Translate);
            _item.Add("Scale", model.Scale);
            _item.Add("Rotate", model.Rotate);
            _item.Add("Material", model.MaterialItem.ToString());
            //Property = new PropertyGridViewModel(PropertyItem);
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
