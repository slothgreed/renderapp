using System.IO;

namespace STLBrowser
{
    public class FileWatcher
    {
        private FileSystemWatcher watcher;
        
        public FileWatcher()
        {
        }

        public bool CreateFileWatcher(string path, string filter)
        {
            watcher = new FileSystemWatcher(path, filter);
            return true;
        }
    }
}
