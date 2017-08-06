using System.Collections.Generic;
using KI.Foundation.Core;

namespace KI.Gfx.KIShader
{
    /// <summary>
    /// シェーダファクトリ
    /// </summary>
    public class ShaderFactory
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static ShaderFactory Instance { get; } = new ShaderFactory();

        public Dictionary<string, Shader> Shaders { get; } = new Dictionary<string, Shader>();

        /// <summary>
        /// 同一シェーダの確認
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        /// <returns>シェーダ</returns>
        public Shader FindShader(string vert, string frag)
        {
            foreach (var sh in Shaders.Values)
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
        /// <param name="path">ファイルパス</param>
        /// <returns>シェーダ</returns>
        public Shader CreateShaderVF(string path)
        {
            return CreateShaderVF(path + ".vert", path + ".frag");
        }

        /// <summary>
        /// vert,frag専用ファイル名は同一のもの
        /// </summary>
        /// <param name="vPath">頂点パス</param>
        /// <param name="fPath">フラグパス</param>
        /// <returns>シェーダ</returns>
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
                Shaders.Add(vname + fname, shader);
            }

            return shader;
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            foreach (var shader in Shaders.Values)
            {
                shader.Dispose();
            }
        }
    }
}
