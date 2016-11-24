using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RenderApp
{

    public abstract class RAFile : RAObject
    {
        public RAFile(string filePath)
        {
            FileName = Path.GetFileName(filePath);
            FilePath = filePath;
            Key = FileName;

            System.Diagnostics.Debug.WriteLine("create asset " + filePath + ":");
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
        public static Dictionary<EAssetType, int> AssetNum = new Dictionary<EAssetType,int>();

        public static string GetNameFromPath(string path)
        {
            return Path.GetFileName(path);
        }
        public static string GetNameFromType(EAssetType type)
        {
            if(!AssetNum.ContainsKey(type))
            {
                AssetNum.Add(type, 0);
            }
            else
            {
                AssetNum[type]++;
            }
            return type.ToString() + AssetNum[type];
        }
    }
}
