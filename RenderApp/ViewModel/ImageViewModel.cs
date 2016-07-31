using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
using System.IO;
using System.Windows.Media;
namespace RenderApp.ViewModel
{
    class ImageViewModel : ViewModelBase
    {

        public Texture Model
        {
            get;
            private set;
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                SetValue<string>(ref _name, value);
                OnPropertyChanged("Items");
            }
        }
        public string ImagePath
        {
            get
            {
                if(Model.FilePath == null)
                {
                    return @"C:\Users\ido\Desktop\RenderApp-clone\RenderApp\Thumbnail\noimage.png";
                }
                return Model.FilePath;
            }
        }
        public string FileName
        {
            get
            {
                return Model.FileName;
            }
        }
        private List<string> _items;
        public List<string> Items
        {
            get
            {
                return _items;
            }
        }
        private int _selectIndex;
        public int SelectIndex
        {
            get
            {
                return _selectIndex;
            }
            set
            {
                SetValue<int>(ref _selectIndex, value);
            }
        }
        
        public ImageViewModel(string name, Texture model)
        {
            Model = model;
            Name = name;
            _items = new List<string>(RenderApp.Scene.ActiveScene.GetAssetListStr(EAssetType.Textures));
            SelectIndex = _items.IndexOf(model.FileName);
        }

        public ImageViewModel(Texture model)
        {
            Model = model;
            Name = "noname";
        }
        public override void UpdateProperty()
        {
        }
    }
}
