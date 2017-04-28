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
        private static string _projectDirectory;
        public static string ProjectDirectory
        {
            get
            {
                //return _projectPath;
                return @"C:\Users\ido\Documents\KIProject\renderapp\RenderApp\Resource";
            }
            set
            {
                _projectDirectory = value;
            }
        }
        public static string ShaderDirectory
        {
            get
            {
                return ProjectDirectory + @"\Shader";
            }
        }
        public static string TextureDirectory
        {
            get
            {
                return ProjectDirectory + @"\Texture";
            }
        }
        public static string ModelDirectory
        {
            get
            {
                return ProjectDirectory + @"\Model";
            }
        }
        public ProjectInfo()
        {
        }
        public bool Open(string filePath)
        {

            if(Path.GetExtension(filePath) != "proj")
            {
                throw new FileFormatException("extention error");
            }

            _projectDirectory = filePath;
            return true;
        }
        public bool Save()
        {
            if (!Directory.Exists(ProjectDirectory))
            {
                Directory.CreateDirectory(ProjectDirectory);
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
            _projectDirectory = filePath;
            return Save();
        }
        
    }
}
