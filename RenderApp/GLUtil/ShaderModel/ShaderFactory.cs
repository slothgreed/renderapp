using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.AssetModel;
namespace RenderApp.GLUtil
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
        private Shader _defaultLightShader;
        public Shader DefaultLightShader
        {
            get
            {
                if(_defaultLightShader == null)
                {
                    string path = Project.ShaderDirectory;
                    ShaderProgram vert = new ShaderProgram(path + @"\Defferd.vert");
                    ShaderProgram frag = new ShaderProgram(path + @"\Defferd.frag");
                    Shader deffered = new Shader(vert, frag);
                    deffered.RenderMode = ERenderMode.Defferred;
                    _defaultLightShader = deffered;
                }
                return _defaultLightShader;
            }
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
        private Shader _defaultDefferredShader;
        public Shader DefaultDefferredShader
        {
            get
            {
                if (_defaultDefferredShader == null)
                {
                    string path = Project.ShaderDirectory;
                    ShaderProgram diffuseV = new ShaderProgram(path + @"\ConstantGeometry.vert");
                    ShaderProgram diffuseF = new ShaderProgram(path + @"\ConstantGeometry.frag");
                    Shader diffuse = new Shader(diffuseV, diffuseF);
                    diffuse.RenderMode = ERenderMode.Defferred;
                    _defaultDefferredShader = diffuse;
                }
                return _defaultDefferredShader;
            }
        }
        private Shader _outputShader;
        public Shader OutputShader
        {
            get
            {
                if(_outputShader == null)
                {
                    string path = Project.ShaderDirectory;
                    ShaderProgram diffuseV = new ShaderProgram(path + @"\Output.vert");
                    ShaderProgram diffuseF = new ShaderProgram(path + @"\Output.frag");
                    Shader output = new Shader(diffuseV, diffuseF);
                    output.RenderMode = ERenderMode.Defferred;
                    _outputShader = output;
                }
                return _outputShader;
            }
        }

    }
}
