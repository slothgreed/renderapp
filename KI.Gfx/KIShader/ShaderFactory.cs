using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.KIShader;
namespace KI.Gfx.KIShader
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

        public Shader FindShader(string vert, string frag)
        {
            foreach (var sh in ShaderList.Values)
            {
                if (sh.FindShaderCombi(vert, frag))
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
            return CreateShaderVF(path + ".vert", path + ".frag");
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
                return null;
            }
            Shader shader = FindShader(vPath, fPath);
            if (shader == null)
            {
                string vname = KIFile.GetNameFromPath(vPath);
                string fname = KIFile.GetNameFromPath(fPath);
                ShaderProgram vert = ShaderProgramFactory.Instance.CreateShaderProgram(vPath, vPath);
                ShaderProgram frag = ShaderProgramFactory.Instance.CreateShaderProgram(fPath, fPath);
                shader = new Shader(vert, frag);
                ShaderList.Add(vname + fname, shader);
            }
            return shader;
        }

        public void Dispose()
        {
            foreach (var shader in ShaderList.Values)
            {
                shader.Dispose();
            }
        }
    }
}
