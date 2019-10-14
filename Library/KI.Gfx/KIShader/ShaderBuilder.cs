using KI.Foundation.Core;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Gfx.KIShader
{
    /// <summary>
    /// シェーダビルドクラス
    /// </summary>
    public static class ShaderBuilder
    {
        #region [build]
        /// <summary>
        /// シェーダの作成
        /// </summary>
        /// <param name="vertexShaderCode">頂点シェーダ</param>
        /// <param name="fragmentShaderCode">フラグシェーダ</param>
        /// <returns>プログラムID</returns>
        public static int CreateShaderProgram(ShaderProgram vertexShader, ShaderProgram fragmentShader)
        {
            int vshader = CompileShader(vertexShader);
            int fshader = CompileShader(fragmentShader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vshader);
            GL.AttachShader(program, fshader);

            GL.DeleteShader(vshader);
            GL.DeleteShader(fshader);

            GL.LinkProgram(program);
            Logger.GLLog(Logger.LogLevel.Error);

            int status;
            string info;
            GL.GetProgramInfoLog(program, out info);
            if (!string.IsNullOrWhiteSpace(info))
            {
                Logger.GLLog(Logger.LogLevel.Error, info);
            }

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                Logger.GLLog(Logger.LogLevel.Error, GL.GetProgramInfoLog(program) + vertexShader.FileName);
            }

            return program;
        }

        /// <summary>
        /// ジオメトリシェーダ用
        /// </summary>
        /// <param name="vertexShaderCode">頂点シェーダ</param>
        /// <param name="fragmentShaderCode">フラグシェーダ</param>
        /// <param name="geometryShaderCode">ジオメトリシェーダ</param>
        /// <param name="inType"></param>
        /// <param name="outType"></param>
        /// <param name="outVertexNum"></param>
        /// <returns>プログラムID</returns>
        public static int CreateShaderProgram(ShaderProgram vertexShader, ShaderProgram fragmentShader, ShaderProgram geometryShader, int inType, int outType, int outVertexNum = -1)
        {
            int vshader = CompileShader(vertexShader);
            int fshader = CompileShader(fragmentShader);
            int gshader = CompileShader(geometryShader);

            if (outVertexNum < 0)
            {
                GL.GetInteger(GetPName.MaxGeometryOutputVertices, out outVertexNum);
            }

            int program = GL.CreateProgram();
            GL.AttachShader(program, vshader);
            GL.AttachShader(program, fshader);
            GL.AttachShader(program, gshader);
            //GL.ProgramParameter(program, AssemblyProgramParameterArb.GeometryInputType, inType);
            //GL.ProgramParameter(program, AssemblyProgramParameterArb.GeometryOutputType, outType);
            //GL.ProgramParameter(program, AssemblyProgramParameterArb.GeometryVerticesOut, outVertexNum);

            GL.DeleteShader(vshader);
            GL.DeleteShader(fshader);
            GL.DeleteShader(gshader);

            GL.LinkProgram(program);
            int status;
            string info;
            GL.GetProgramInfoLog(program, out info);
            if (!string.IsNullOrWhiteSpace(info))
            {
                Logger.GLLog(Logger.LogLevel.Error, info);
            }

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                Logger.GLLog(Logger.LogLevel.Error, GL.GetProgramInfoLog(program));
            }

            Logger.GLLog(Logger.LogLevel.Error);
            return program;
        }

        /// <summary>
        /// テッセレーションシェーダ用
        /// </summary>
        /// <param name="vertexShader">頂点シェーダ</param>
        /// <param name="fragmentShader">フラグシェーダ</param>
        /// <param name="geometryShader">ジオメトリシェーダ</param>
        /// <param name="tcsShader">テッセレーション制御シェーダ</param>
        /// <param name="tesShader">テッセレーション評価シェーダ</param>
        /// <returns>プログラムID</returns>
        public static int CreateShaderProgram(ShaderProgram vertexShader, ShaderProgram fragmentShader, ShaderProgram geometryShader, ShaderProgram tcsShader, ShaderProgram tesShader)
        {
            int vshader = CompileShader(vertexShader);
            int fshader = CompileShader(fragmentShader);
            int gshader = CompileShader(geometryShader);
            int tesshader = CompileShader(tesShader);
            int tcsshader = CompileShader(tcsShader);

            int outVertexNum = -1;

            if (outVertexNum < 0)
            {
                GL.GetInteger(GetPName.MaxGeometryOutputVertices, out outVertexNum);
            }

            int program = GL.CreateProgram();
            GL.AttachShader(program, vshader);
            GL.AttachShader(program, fshader);
            GL.AttachShader(program, gshader);
            GL.AttachShader(program, tesshader);
            GL.AttachShader(program, tcsshader);
            //int inType = 3;
            //int outType = 3;
            //GL.ProgramParameter(program, AssemblyProgramParameterArb.GeometryInputType, inType);
            //GL.ProgramParameter(program, AssemblyProgramParameterArb.GeometryOutputType, outType);
            //GL.ProgramParameter(program, AssemblyProgramParameterArb.GeometryVerticesOut, outVertexNum);

            GL.DeleteShader(vshader);
            GL.DeleteShader(fshader);
            GL.DeleteShader(gshader);
            GL.DeleteShader(tesshader);
            GL.DeleteShader(tcsshader);

            GL.LinkProgram(program);

            int status;
            string info;
            GL.GetProgramInfoLog(program, out info);
            if (!string.IsNullOrWhiteSpace(info))
            {
                Logger.GLLog(Logger.LogLevel.Error, info);
            }

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                Logger.GLLog(Logger.LogLevel.Error, GL.GetProgramInfoLog(program));
            }

            Logger.GLLog(Logger.LogLevel.Error);
            return program;
        }

        /// <summary>
        /// テッセレーションシェーダ用
        /// </summary>
        /// <param name="vertexShaderCode">頂点シェーダ</param>
        /// <param name="fragmentShaderCode">フラグシェーダ</param>
        /// <param name="tcsShaderCode">テッセレーション制御シェーダ</param>
        /// <param name="tesShaderCode">テッセレーション評価シェーダ</param>
        /// <returns>プログラムID</returns>
        public static int CreateShaderProgram(ShaderProgram vertexShader, ShaderProgram fragmentShader, ShaderProgram tcsShader, ShaderProgram tesShader)
        {
            int vshader = CompileShader(vertexShader);
            int fshader = CompileShader(fragmentShader);
            int tcsshader = CompileShader(tcsShader);
            int tesshader = CompileShader(tesShader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vshader);
            GL.AttachShader(program, fshader);
            GL.AttachShader(program, tesshader);
            GL.AttachShader(program, tcsshader);

            GL.DeleteShader(vshader);
            GL.DeleteShader(fshader);
            GL.DeleteShader(tesshader);
            GL.DeleteShader(tcsshader);

            GL.LinkProgram(program);

            int status;
            string info;
            GL.GetProgramInfoLog(program, out info);
            if (!string.IsNullOrWhiteSpace(info))
            {
                Logger.GLLog(Logger.LogLevel.Error, info);
            }

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                Logger.GLLog(Logger.LogLevel.Error, GL.GetProgramInfoLog(program));
            }

            Logger.GLLog(Logger.LogLevel.Error);
            return program;
        }


        /// <summary>
        /// シェーダのコンパイル
        /// </summary>
        /// <param name="shaderType">シェーダの種類</param>
        /// <param name="shaderCode">シェーダコード</param>
        /// <returns>シェーダID</returns>
        private static int CompileShader(ShaderProgram shaderProgram)
        {
            var shaderType = TypeUtility.ConvertShaderType(shaderProgram.ShaderKind);
            int shader = GL.CreateShader(shaderType);
            string info;
            int status_code;
            GL.ShaderSource(shader, shaderProgram.ShaderCode);
            GL.CompileShader(shader);
            GL.GetShaderInfoLog(shader, out info);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out status_code);
            if (status_code != 1)
            {
                Logger.Log(Logger.LogLevel.Error, shaderProgram.FileName);
                Logger.Log(Logger.LogLevel.Error, info);
            }

            Logger.GLLog(Logger.LogLevel.Error);
            return shader;
        }
        #endregion
    }
}
