﻿namespace KI.Presenter.ViewModel
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
        public ThumbnailFileViewModel(ViewModelBase parent, string filePath, string imagePath)
            : base(parent, null)
        {
            FilePath = filePath;
            ImagePath = imagePath;
        }
    }
}
