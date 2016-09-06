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
                    string vPath = @"\Defferd.vert";
                    string fPath = @"\Defferd.frag";
                    ShaderProgram vert = new ShaderProgram(vPath, path + vPath);
                    ShaderProgram frag = new ShaderProgram(fPath, path + fPath);
                    Shader deffered = new Shader(vert, frag);
                    _defaultLightShader = deffered;
                }
                return _defaultLightShader;
            }
        }
        private Shader _defaultAnalyzeShader;
        public Shader DefaultAnalyzeShader
        {
            get
            {
                if (_defaultAnalyzeShader == null)
                {
                    string path = Project.ShaderDirectory;
                    string vPath = @"\ConstantGeometry.vert";
                    string fPath = @"\ConstantGeometry.frag";
                    ShaderProgram vert = new ShaderProgram(vPath, path + vPath);
                    ShaderProgram frag = new ShaderProgram(fPath, path + fPath);
                    Shader deffered = new Shader(vert, frag);
                    _defaultAnalyzeShader = deffered;
                }
                return _defaultAnalyzeShader;
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
                    string vPath = @"\MaterialGeometry.vert";
                    string fPath = @"\MaterialGeometry.frag";
                    ShaderProgram vert = new ShaderProgram(vPath, path + vPath);
                    ShaderProgram frag = new ShaderProgram(fPath, path + fPath);
                    Shader diffuse = new Shader(vert, frag);

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
                    string vPath = @"\Output.vert";
                    string fPath = @"\Output.frag";
                    ShaderProgram vert = new ShaderProgram(vPath, path + vPath);
                    ShaderProgram frag = new ShaderProgram(fPath, path + fPath);
                    Shader output = new Shader(vert, frag);
                    _outputShader = output;
                }
                return _outputShader;
            }
        }

    }
}
