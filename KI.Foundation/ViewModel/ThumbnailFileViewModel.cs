using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace KI.Foundation.ViewModel
{
    public class ThumbnailFileViewModel : ViewModelBase
    {
        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }

            set
            {
                SetValue(ref _filePath, value);
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }

            set
            {
                SetValue(ref _imagePath, value);
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
