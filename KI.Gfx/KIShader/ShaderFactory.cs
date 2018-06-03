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
        public static object RenderTargetFactory { get; set; }

        /// <summary>
        /// 同一シェーダの探索
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        /// <param name="geom">ジオメトリシェーダ</param>
        /// <param name="tcs">テッセレーション制御シェーダ</param>
        /// <param name="tes">テッセレーション評価シェーダ</param>
        /// <returns>シェーダ</returns>
        public Shader FindShader(string vert, string frag, string geom, string tcs, string tes)
        {
            foreach (var sh in Shaders.Values)
            {
                if (sh.FindShaderCombi(vert, frag, null, null, null))
                {
                    return sh;
                }
            }

            return null;
        }

        /// <summary>
        /// 頂点シェーダとフラグメントシェーダのみのシェーダの作成
        /// </summary>
        /// <param name="vPath">頂点パス</param>
        /// <param name="fPath">フラグパス</param>
        /// <returns>シェーダ</returns>
        public Shader CreateShaderVF(string vPath, string fPath, ShaderStage stage)
        {
            Shader shader = FindShader(vPath, fPath, null, null, null);
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
        /// 頂点シェーダとフラグメントシェーダとジオメトリシェーダのみのシェーダの作成
        /// </summary>
        /// <param name="vPath">頂点パス</param>
        /// <param name="fPath">フラグパス</param>
        /// <param name="gPath">ジオメトリシェーダ</param>
        /// <param name="stage">シェーダステージ</param>
        /// <returns>シェーダ</returns>
        public Shader CreateGeometryShader(string vPath, string fPath, string gPath, ShaderStage stage)
        {
            Shader shader = FindShader(vPath, fPath, gPath, null, null);
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
        /// 頂点シェーダとフラグメントシェーダのみのシェーダの作成
        /// </summary>
        /// <param name="vPath">頂点シェーダ</param>
        /// <param name="fPath">フラグシェーダ</param>
        /// <param name="gPath">ジオメトリシェーダ</param>
        /// <param name="tcPath">テッセレーション制御シェーダ</param>
        /// <param name="tePath">テッセレーション評価シェーダ</param>
        /// <returns>シェーダ</returns>
        public Shader CreateTesselation(string vPath, string fPath, string gPath, string tcPath, string tePath, ShaderStage stage)
        {
            Shader shader = FindShader(vPath, fPath, gPath, tcPath, tePath);
            if (shader == null)
            {
                string vname = Path.GetFileName(vPath);
                string fname = Path.GetFileName(fPath);
                string gname = Path.GetFileName(gPath);
                string tcname = Path.GetFileName(tcPath);
                string tename = Path.GetFileName(tePath);

                ShaderProgram vert = ShaderProgramFactory.Instance.CreateShaderProgram(vPath, vPath);
                ShaderProgram frag = ShaderProgramFactory.Instance.CreateShaderProgram(fPath, fPath);
                ShaderProgram geom = ShaderProgramFactory.Instance.CreateShaderProgram(gPath, gPath);
                ShaderProgram tcs = ShaderProgramFactory.Instance.CreateShaderProgram(tcPath, tcPath);
                ShaderProgram tes = ShaderProgramFactory.Instance.CreateShaderProgram(tePath, tePath);
                shader = new Shader(vert, frag, geom, tcs, tes, stage);
                Shaders.Add(vname + fname + tcs + tes, shader);
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
