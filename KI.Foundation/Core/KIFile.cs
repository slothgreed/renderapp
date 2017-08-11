using System.IO;

namespace KI.Foundation.Core
{
    /// <summary>
    /// ファイルオブジェクト
    /// </summary>
    public abstract class KIFile : KIObject
    {
        /// <summary>
        /// ディレクトリパス
        /// </summary>
        private string directoryPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public KIFile(string filePath)
            : base(filePath)
        {
            FileName = Path.GetFileName(filePath);
            FilePath = filePath;
            Name = FileName;
        }

        /// <summary>
        /// ディレクトリパス
        /// </summary>
        public string DirectoryPath
        {
            get
            {
                if (FilePath == null)
                {
                    return null;
                }
                else if (directoryPath == null)
                {
                    directoryPath = System.IO.Path.GetDirectoryName(FilePath);
                }

                return directoryPath;
            }
        }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; private set; }
    }
}
