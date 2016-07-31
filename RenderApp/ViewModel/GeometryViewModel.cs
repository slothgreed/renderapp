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
    class GeometryViewModel : AvalonWindowViewModel
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
        public Dictionary<string,object> Items
        {
            get;
            private set;
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
            WindowPosition = AvalonWindow.LeftDown;
            Model = model;
            Items = new Dictionary<string,object>();
            Items.Add("Translate",model.Translate);
            Items.Add("Scale",model.Scale);
            Items.Add("Rotate",model.Rotate);
            Items.Add("Material", model.MaterialItem.ToString());
            Property = new PropertyGridViewModel(Items);
        }

        public GeometryViewModel()
        {
            WindowPosition = AvalonWindow.LeftDown;
        }
        public override void SizeChanged()
        {
        }
        public override void UpdateProperty()
        {

        }
    }
}
