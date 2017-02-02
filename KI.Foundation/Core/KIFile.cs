using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KI.Foundation.Utility;

namespace KI.Foundation.Core
{
    public abstract class KIFile : KIObject
    {
        private string _directoryPath;
        public string DirectoryPath
        {
            get
            {
                if (FilePath == null)
                {
                    return null;
                }
                else if(_directoryPath == null)
                {
                    _directoryPath = System.IO.Path.GetDirectoryName(FilePath);
                }
                return _directoryPath;
            }
        }
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get;
            private set;
        }
        /// <summary>
        /// テクスチャ名
        /// </summary>
        public string FileName
        {
            get;
            private set;
        }

        public KIFile(string filePath)
            :base(filePath)
        {
            FileName = Path.GetFileName(filePath);
            FilePath = filePath;
            Name = FileName;
        }
        
        public static string GetNameFromPath(string path)
        {
           return Path.GetFileName(path);
        }
    }
}
