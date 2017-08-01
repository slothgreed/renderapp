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
                SetValue(ref _FolderPath, value);
                CreateDirectoryTree(value);
            }
        }

        private DirectoryViewModel _root;
        public DirectoryViewModel Root
        {
            get
            {
                return _root;
            }
            set
            {
                SetValue(ref _root, value);
            }
        }
        private ICommand _OpenFolder;
        public ICommand OpenFolder
        {
            get
            {
                if (_OpenFolder == null)
                {
                    _OpenFolder = CreateCommand(OnOpenFolderCommand);
                }
                return _OpenFolder;
            }
        }
        private void OnOpenFolderCommand()
        {

            return;
        }
        private ICommand _SelectedItemChanged;
        public ICommand SelectedItemChanged
        {
            get
            {
                if (_SelectedItemChanged == null)
                {
                    _SelectedItemChanged = CreateCommand(OnSelectedFolderChanged);
                }
                return _SelectedItemChanged;
            }
        }
        private void OnSelectedFolderChanged(object sender)
        {

        }
        public FileTreeViewModel(string path)
        {
            FolderPath = path;
        }

        public void CreateDirectoryTree(string path)
        {
            Root = new DirectoryViewModel(path);
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
