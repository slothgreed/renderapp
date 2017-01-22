using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using RenderApp.AssetModel;
using System.Windows.Controls;
using System.Windows;
using OpenTK;
using RenderApp.GLUtil;
using RenderApp.ViewModel.DockTabVM;
using RenderApp.ViewModel.PropertyVM;
namespace RenderApp.ViewModel.AssetVM
{
    public class MaterialViewModel : TabItemViewModel,IPropertyGridViewModel
    {

        private Dictionary<string,object> _item;
        public Dictionary<string,object> PropertyItem
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

        private Material _model;
        public Material Model
        {
            get { return _model; }
            set { SetValue<Material>(ref _model, value); }
        }
        public MaterialViewModel()
        {
        }
        public MaterialViewModel(Material model)
        {
            Title = model.Key;
            _item = new Dictionary<string, object>();

            model.CurrentShader.ActiveShader.ForEach(
                loop => PropertyItem.Add(loop.shaderType.ToString(),loop.FileName)
                );
            foreach (KeyValuePair<TextureKind,Texture> loop in model.TextureItem)
            {
                PropertyItem.Add(loop.Key.ToString(), loop.Value);
            }
        }
        private string _title = "";
        public override string Title
        {
            get
            {
                return "Material : " + _title;
            }
            set
            {
                SetValue(ref _title, value);
            }
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
