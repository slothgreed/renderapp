using System;

namespace KI.UI.ViewModel
{
    public class ThumbnailFileViewModel : ViewModelBase
    {
        private string filePath;

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                SetValue(ref filePath, value);
            }
        }

        private string imagePath;

        public string ImagePath
        {
            get
            {
                return imagePath;
            }

            set
            {
                SetValue(ref imagePath, value);
            }
        }

        /// <summary>
        /// サムネイルファイルのパス
        /// </summary>
        /// <param name="path"></param>
        public ThumbnailFileViewModel(string filePath, string imagePath)
        {
            FilePath = filePath;
            ImagePath = imagePath;
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
