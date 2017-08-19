using System.Collections.Generic;
using KI.Gfx.KITexture;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    class ImageViewModel : ViewModelBase
    {
        private string name;
        private List<string> items;
        private int selectIndex;

        public Texture Model
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                SetValue<string>(ref name, value);
                OnPropertyChanged("Items");
            }
        }

        //public string ImagePath
        //{
        //    get
        //    {
        //        if (Model.ImageInfo == null)
        //        {
        //            return @"C:\Users\ido\Desktop\RenderApp-clone\RenderApp\Thumbnail\noimage.png";
        //        }

        //        if (Model.ImageInfo.FilePath == null)
        //        {
        //            return @"C:\Users\ido\Desktop\RenderApp-clone\RenderApp\Thumbnail\noimage.png";
        //        }

        //        return Model.ImageInfo.FilePath;
        //    }
        //}

        //public string FileName
        //{
        //    get
        //    {
        //        if (Model.ImageInfo == null)
        //        {
        //            return string.Empty;
        //        }

        //        if (Model.ImageInfo.FilePath == null)
        //        {
        //            return string.Empty;
        //        }

        //        return Model.ImageInfo.FileName;
        //    }
        //}

        public List<string> Items
        {
            get
            {
                return items;
            }
        }

        public int SelectIndex
        {
            get
            {
                return selectIndex;
            }

            set
            {
                SetValue<int>(ref selectIndex, value);
            }
        }

        public ImageViewModel(string name, Texture model)
        {
            Model = model;
            Name = name;
            items = new List<string>();
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
