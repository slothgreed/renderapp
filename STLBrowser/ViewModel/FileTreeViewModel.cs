using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using KI.Foundation.ViewModel;


namespace STLBrowser.ViewModel
{
    public class FileTreeViewModel : ViewModelBase
    {
        private string _FolderPath;
        public string FolderPath
        {
            get
            {
                return _FolderPath;
            }
            set
            {
                SetValue(ref _FolderPath,value);
            }
        }
        public DirectoryViewModel Root
        {
            get;
            set;
        }

        private ICommand _SelectedFolderChanged;
        public ICommand SelectedFolderChangedCommand
        {
            get
            {
                if(_SelectedFolderChanged == null)
                {
                    _SelectedFolderChanged = CreateCommand(OnSelectedFolderChanged);
                }
                return _SelectedFolderChanged;
            }
        }
        private void OnSelectedFolderChanged()
        {

        }
        public FileTreeViewModel(string path)
        {
            Root = new DirectoryViewModel(path);
            FolderPath = path;
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
