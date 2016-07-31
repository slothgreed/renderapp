using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RenderApp.Assets
{

    public abstract class Asset
    {
        public Asset()
        {

        }
        public Asset(string name)
        {
            Key = name;
        }
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
        /// テクスチャパス
        /// </summary>
        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            protected set
            {
                _filePath = value;
            }
        }
        /// <summary>
        /// テクスチャ名
        /// </summary>
        private string _fileName;
        public virtual string FileName
        {
            get
            {
                if (_fileName == null)
                {
                    _fileName = System.IO.Path.GetFileName(FilePath);
                }
                return _fileName;
            }
            set
            {
                if (_fileName == null)
                {
                    _fileName = System.IO.Path.GetFileName(FilePath);
                }
            }
        }
        private string _key = null;
        public virtual string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        public override string ToString()
        {
            if(Key != null)
            {
                return Key;
            }
            if(FileName != null)
            {
                return FileName;
            }
            return "Unknown";

        }
        public abstract void Dispose();
    }
}
