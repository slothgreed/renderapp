using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.AssetModel;
using RenderApp.Globals;
using KI.Foundation.Core;
using KI.Gfx;
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
        private Dictionary<string, ShaderProgram> ShaderProgramList = new Dictionary<string, ShaderProgram>();

        private ShaderProgram CreateShaderProgram(string key,string path)
        {
            if(ShaderProgramList.ContainsKey(key))
            {
                return ShaderProgramList[key];
            }
            else
            {
                ShaderProgram program = new ShaderProgram(key, path);
                Project.ActiveProject.AddChild(program);
                return program;
            }
        }

        public Shader FindShader(string vert,string frag)
        {
            foreach (var sh in ShaderList.Values)
            {
                if (sh.FindShaderCombi(vert,frag))
                {
                    return sh;
                }
            }
            return null;

        }
        /// <summary>
        /// vert,frag専用ファイル名は同一のもの
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Shader CreateShaderVF(string path)
        {
            Shader shader = FindShader(path + ".vert", path + ".frag");
            if (shader != null)
            {
                return shader;
            }
            string name = KIFile.GetNameFromPath(path);
            ShaderProgram vert = CreateShaderProgram(path, path + ".vert");
            ShaderProgram frag = CreateShaderProgram(path, path + ".frag");
            shader = new Shader(vert, frag);
            ShaderList.Add(name, shader);
            return shader;

        }
        /// <summary>
        /// vert,frag専用ファイル名は同一のもの
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Shader CreateShaderVF(string vPath, string fPath)
        {
            if (vPath == null || fPath == null)
            {
                return DefaultDefferredShader;
            }
            Shader shader = FindShader(vPath, fPath);
            if (shader == null)
            {
                string vname = KIFile.GetNameFromPath(vPath);
                string fname = KIFile.GetNameFromPath(fPath);
                ShaderProgram vert = CreateShaderProgram(vPath, vPath);
                ShaderProgram frag = CreateShaderProgram(fPath, fPath);
                shader = new Shader(vert, frag);
                ShaderList.Add(vname+fname, shader);
            }
            return shader;
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
