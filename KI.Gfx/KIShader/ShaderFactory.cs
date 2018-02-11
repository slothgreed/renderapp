using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// シェーダ
        /// </summary>
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
        /// 同一シェーダの確認
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        /// <returns>シェーダ</returns>
        public Shader FindShader(string vert, string frag, string geom)
        {
            foreach (var sh in Shaders.Values)
            {
                if (sh.FindShaderCombi(vert, frag, geom))
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
        /// <param name="stage">シェーダステージ</param>
        /// <returns>シェーダ</returns>
        public Shader CreateShaderVF(string path, ShaderStage stage)
        {
            return CreateShaderVF(path + ".vert", path + ".frag", stage);
        }

        /// <summary>
        /// 頂点シェーダとフラグメントシェーダのみのシェーダの作成
        /// </summary>
        /// <param name="vPath">頂点パス</param>
        /// <param name="fPath">フラグパス</param>
        /// <returns>シェーダ</returns>
        public Shader CreateShaderVF(string vPath, string fPath, ShaderStage stage)
        {
            Shader shader = FindShader(vPath, fPath);
            if (shader == null)
            {
                string vname = Path.GetFileName(vPath);
                string fname = Path.GetFileName(fPath);
                ShaderProgram vert = ShaderProgramFactory.Instance.CreateShaderProgram(vPath, vPath);
                ShaderProgram frag = ShaderProgramFactory.Instance.CreateShaderProgram(fPath, fPath);
                shader = new Shader(vert, frag, stage);
                Shaders.Add(vname + fname, shader);
            }

            return shader;
        }

        /// <summary>
        /// vert,frag専用ファイル名は同一のもの
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>シェーダ</returns>
        public Shader CreateGeometryShader(string path, ShaderStage stage)
        {
            return CreateGeometryShader(path + ".vert", path + ".frag", path + ".geom", stage);
        }

        /// <summary>
        /// 頂点シェーダとフラグメントシェーダとジオメトリシェーダのみのシェーダの作成
        /// </summary>
        /// <param name="vPath">頂点パス</param>
        /// <param name="fPath">フラグパス</param>
        /// <param name="gPath">ジオメトリシェーダ</param>
        /// <param name="stage">シェーダステージ</param>
        /// <returns>シェーダ</returns>
        public Shader CreateGeometryShader(string vPath, string fPath, string gPath, ShaderStage stage)
        {
            Shader shader = FindShader(vPath, fPath, gPath);
            if (shader == null)
            {
                string vname = Path.GetFileName(vPath);
                string fname = Path.GetFileName(fPath);
                string gname = Path.GetFileName(gPath);
                ShaderProgram vert = ShaderProgramFactory.Instance.CreateShaderProgram(vPath, vPath);
                ShaderProgram frag = ShaderProgramFactory.Instance.CreateShaderProgram(fPath, fPath);
                ShaderProgram geom = ShaderProgramFactory.Instance.CreateShaderProgram(gPath, gPath);
                shader = new Shader(vert, frag, geom, stage);
                Shaders.Add(vname + fname + geom, shader);
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
