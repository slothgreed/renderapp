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
    public class MaterialViewModel : TabItemViewModel
    {

        private Dictionary<string,object> _items;
        public Dictionary<string,object> Items
        {
            get
            {
                return _items;
            }
        }
        private PropertyGridViewModel _property;
        public PropertyGridViewModel Property
        {
            get
            {
                return _property;
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
            _items = new Dictionary<string, object>();

            model.CurrentShader.ActiveShader.ForEach(
                loop => Items.Add(loop.shaderType.ToString(),loop.FileName)
                );
            foreach (KeyValuePair<TextureKind,Texture> loop in model.TextureItem)
            {
                Items.Add(loop.Key.ToString(), loop.Value);
            }
            _property = new PropertyGridViewModel(Items);
        }

        public override string Title
        {
            get { return "Material"; }
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
