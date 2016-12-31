using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.AssetModel;
using RenderApp.Globals;
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
        public Dictionary<string, Shader> ShaderList = new Dictionary<string, Shader>();
        private ShaderProgram CreateShaderProgram(string key,string path)
        {
            ShaderProgram program = new ShaderProgram(key, path);
            Project.ActiveProject.AddChild(program);
            return program;
        }
        /// <summary>
        /// vert,frag専用ファイル名は同一のもの
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Shader CreateShaderVF(string path)
        {
            string name = RAFile.GetNameFromPath(path);
            if(ShaderList.Keys.Contains(name))
            {
                return ShaderList[name];
            }
            else
            {
                ShaderProgram vert = CreateShaderProgram(path, path + ".vert");
                ShaderProgram frag = CreateShaderProgram(path, path + ".frag");
                Shader shader = new Shader(vert, frag);
                ShaderList.Add(name, shader);
                return shader;
            }
        }
        private Shader _defaultLightShader;
        public Shader DefaultLightShader
        {
            get
            {
                if(_defaultLightShader == null)
                {
                    _defaultLightShader = CreateShaderVF(ProjectInfo.ShaderDirectory + @"\Defferd");
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
                    _defaultAnalyzeShader = CreateShaderVF(ProjectInfo.ShaderDirectory + @"\ConstantGeometry");
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
                    _defaultDefferredShader = CreateShaderVF(ProjectInfo.ShaderDirectory + @"\MaterialGeometry");
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
                    _outputShader = CreateShaderVF(ProjectInfo.ShaderDirectory + @"\Output");
                }
                return _outputShader;
            }
        }

        public void Dispose()
        {
            foreach(var shadre in ShaderList.Values)
            {
                shadre.Dispose();
            }
        }

        private Shader _selectionShader;
        public Shader DefaultSelectionShader
        {
            get
            {
                if (_selectionShader == null)
                {
                    _selectionShader = CreateShaderVF(ProjectInfo.ShaderDirectory + @"\Selection");

                }
                return _selectionShader;
            }
        }
    }
}
