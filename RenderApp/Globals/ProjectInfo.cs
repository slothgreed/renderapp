using System.IO;

namespace RenderApp
{
    /// <summary>
    /// プロジェクト情報
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProjectInfo()
        {
        }

        /// <summary>
        /// 開いたか
        /// </summary>
        public static bool IsOpen { get; set; }

        public static string ProjectDirectory
        {
            get
            {
                return AppSystem.KIDirectory;
            }
        }

        public static string ResourceDirectory
        {
            get
            {
                return ProjectDirectory + @"\RenderApp\Resource";
            }
        }

        public static string ShaderDirectory
        {
            get
            {
                return ResourceDirectory + @"\Shader";
            }
        }

        public static string TextureDirectory
        {
            get
            {
                return ResourceDirectory + @"\Texture";
            }
        }

        public static string ModelDirectory
        {
            get
            {
                return ResourceDirectory + @"\Model";
            }
        }

        /// <summary>
        /// プロジェクトを開く
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>成功</returns>
        public bool Open(string filePath)
        {
            if (Path.GetExtension(filePath) != "proj")
            {
                throw new FileFormatException("extention error");
            }

            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>成功</returns>
        public bool Save()
        {
            if (!Directory.Exists(ResourceDirectory))
            {
                Directory.CreateDirectory(ResourceDirectory);
            }

            //ToDo: models;
            //List<AssetModel.Asset> models = new List<CModel>();
            //foreach(var loop in models)
            //{
            //    File.Copy(loop.FilePath, ProjectDirectory + loop.FileName);
            //}
            //List<Texture> textures = new List<Texture>();
            //foreach(var loop in textures)
            //{
            //    File.Copy(loop.FilePath, ProjectDirectory + loop.FileName);
            //}

            return true;
        }

        /// <summary>
        /// 別名保存
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>成功</returns>
        public bool SaveAs(string filePath)
        {
            return Save();
        }
    }
}
