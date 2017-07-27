using System.Collections.Generic;
using KI.Foundation.ViewModel;
using KI.Gfx.KITexture;

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
                if (Model.ImageInfo == null)
                {
                    return @"C:\Users\ido\Desktop\RenderApp-clone\RenderApp\Thumbnail\noimage.png";
                }
                if (Model.ImageInfo.FilePath == null)
                {
                    return @"C:\Users\ido\Desktop\RenderApp-clone\RenderApp\Thumbnail\noimage.png";
                }
                return Model.ImageInfo.FilePath;
            }
        }
        public string FileName
        {
            get
            {
                if (Model.ImageInfo == null)
                {
                    return "";
                }
                if (Model.ImageInfo.FilePath == null)
                {
                    return "";
                }
                return Model.ImageInfo.FileName;
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
            _items = new List<string>();
            //_items = new List<string>(RenderApp.Workspace.SceneManager.ActiveScene.GetAssetListStr(EAssetType.Textures));
            //SelectIndex = _items.IndexOf(model.ImageInfo.FileName);
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
