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

        private Shader _defaultForwardShader;
        public Shader DefaultForwardShader
        {
            get
            {
                if (_defaultForwardShader == null)
                {
                    string path = Project.ShaderDirectory;
                    ShaderProgram diffuseV = new ShaderProgram(path + @"\Diffuse.vert");
                    ShaderProgram diffuseF = new ShaderProgram(path + @"\Diffuse.frag");
                    Shader diffuse = new Shader(diffuseV, diffuseF);
                    diffuse.RenderMode = ERenderMode.Forward;
                    _defaultForwardShader = diffuse;
                }
                return _defaultForwardShader;
            }
        }
        private Shader _defaultDefferedShader;
        public Shader DefaultDefferedShader
        {
            get
            {
                if (_defaultDefferedShader == null)
                {
                    string path = Project.ShaderDirectory;
                    ShaderProgram diffuseV = new ShaderProgram(path + @"\defferd.vert");
                    ShaderProgram diffuseF = new ShaderProgram(path + @"\defferd.frag");
                    Shader diffuse = new Shader(diffuseV, diffuseF);
                    diffuse.RenderMode = ERenderMode.Deffered;
                    _defaultDefferedShader = diffuse;
                }
                return _defaultDefferedShader;
            }
        }

    }
}
