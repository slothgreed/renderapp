using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KI.Foundation.ViewModel;
using STLBrowser.Model;
namespace STLBrowser.ViewModel
{
    public class ThumbnailFileViewModel : ViewModelBase
    {
        private STLFile _model;
        public STLFile Model
        {
            get
            {
                return _model;
            }
            set
            {
                SetValue(ref _model, value);
            }
        }


        /// <summary>
        /// サムネイルファイルのパス
        /// </summary>
        /// <param name="path"></param>
        public ThumbnailFileViewModel(STLFile stlFile)
        {
            Model = stlFile;
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
