using System;
using System.IO;
namespace RenderApp
{
    class ProjectInfo
    {
        public static bool IsOpen
        {
            get;
            set;
        }

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
                //return _projectPath;
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
        public ProjectInfo()
        {
        }
        public bool Open(string filePath)
        {

            if (Path.GetExtension(filePath) != "proj")
            {
                throw new FileFormatException("extention error");
            }

            return true;
        }
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
        public bool SaveAs(string filePath)
        {
            return Save();
        }

    }
}
