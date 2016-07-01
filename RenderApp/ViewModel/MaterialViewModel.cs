using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using RenderApp.Assets;
using System.Windows.Controls;
using System.Windows;
using OpenTK;
using RenderApp.GLUtil;
namespace RenderApp.ViewModel
{
    class MaterialViewModel : AvalonWindowViewModel
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
            WindowPosition = AvalonWindow.RightUp;
        }
        public MaterialViewModel(Material model)
        {
            WindowPosition = AvalonWindow.RightUp;


            _items = new Dictionary<string, object>();

            model.ShaderItem.ActiveShader.ForEach(
                loop => Items.Add(loop.shaderType.ToString(),loop.FileName)
                );
            foreach (KeyValuePair<TextureKind,Texture> loop in model.TextureItem)
            {
                Items.Add(loop.Key.ToString(), loop.Value);
            }
            model.AnalyzeItem.ForEach(
                loop=> Items.Add(loop.ToString(), loop)
                );
            _property = new PropertyGridViewModel(Items);
        }

        public override string Title
        {
            get { return "Material"; }
        }
        public override void SizeChanged()
        {
        }
        public override void UpdateProperty()
        {

        }
    }
}
