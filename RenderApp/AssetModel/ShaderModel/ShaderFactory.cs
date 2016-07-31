using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel.ShaderModel;
namespace RenderApp.AssetModel
{
    public class ShaderFactory
    {
        private static ShaderFactory _instance = new ShaderFactory();
        public static ShaderFactory Instance
        {
            get
            {
                return _instance;
            }
        }
        public Shader CreateDefaultLightShader()
        {
            ShaderProgram vert = new ShaderProgram("todo");
            ShaderProgram frag = new ShaderProgram("todo");
            return null;
        }
    }
}
