using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
