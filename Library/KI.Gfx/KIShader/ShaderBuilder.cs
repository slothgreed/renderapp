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
        /// シェーダプログラムの作成
        /// </summary>
        /// <param name="shdaers">シェーダの入ったリスト</param>
        /// <returns>シェーダプログラムID</returns>
        public static int CreateShaderProgram(List<ShaderProgram> shaders)
        {
            var vertexShader = shaders.FirstOrDefault(p => p.ShaderKind == ShaderKind.VertexShader);
            var fragShader = shaders.FirstOrDefault(p => p.ShaderKind == ShaderKind.FragmentShader);
            var geomShader = shaders.FirstOrDefault(p => p.ShaderKind == ShaderKind.GeometryShader);
            var tcsShader = shaders.FirstOrDefault(p => p.ShaderKind == ShaderKind.TessControlShader);
            var tessShader = shaders.FirstOrDefault(p => p.ShaderKind == ShaderKind.TessEvaluationShader);

            if (geomShader != null && tcsShader != null && tessShader != null)
            {
                return CreateShaderProgram(vertexShader, fragShader, geomShader, tcsShader, tessShader);
            }
            else if (geomShader == null && tcsShader != null && tessShader != null)
            {
                return CreateShaderProgram(vertexShader, fragShader, tcsShader, tessShader);
            }
            else if (geomShader != null)
            {
                return CreateShaderProgram(vertexShader, fragShader, geomShader, 3, 3);
            }
            else if(vertexShader != null && fragShader != null)
            {
                return CreateShaderProgram(vertexShader, fragShader);
            }

            return -1;
        }

        /// <summary>
        /// シェーダの作成
        /// </summary>
        /// <param name="vertexShaderCode">頂点シェーダ</param>
        /// <param name="fragmentShaderCode">フラグシェーダ</param>
        /// <returns>プログラムID</returns>
        private static int CreateShaderProgram(ShaderProgram vertexShader, ShaderProgram fragmentShader)
        {
            int vshader = CompileShader(vertexShader);
            int fshader = CompileShader(fragmentShader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vshader);
            GL.AttachShader(program, fshader);

            GL.DeleteShader(vshader);
            GL.DeleteShader(fshader);

            GL.LinkProgram(program);

            GL.DetachShader(program, vshader);
            GL.DetachShader(program, fshader);
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
        private static int CreateShaderProgram(ShaderProgram vertexShader, ShaderProgram fragmentShader, ShaderProgram geometryShader, int inType, int outType, int outVertexNum = -1)
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


            GL.DetachShader(program, vshader);
            GL.DetachShader(program, fshader);
            GL.DetachShader(program, gshader);


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
        private static int CreateShaderProgram(ShaderProgram vertexShader, ShaderProgram fragmentShader, ShaderProgram geometryShader, ShaderProgram tcsShader, ShaderProgram tesShader)
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


            GL.DetachShader(program, vshader);
            GL.DetachShader(program, fshader);
            GL.DetachShader(program, gshader);
            GL.DetachShader(program, tesshader);
            GL.DetachShader(program, tcsshader);
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
        private static int CreateShaderProgram(ShaderProgram vertexShader, ShaderProgram fragmentShader, ShaderProgram tcsShader, ShaderProgram tesShader)
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

            GL.DetachShader(program, vshader);
            GL.DetachShader(program, fshader);
            GL.DetachShader(program, tesshader);
            GL.DetachShader(program, tcsshader);

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
            var code = shaderProgram.ShaderCode;
            int index = shaderProgram.Version.Length;
            code = code.Insert(index, shaderProgram.Header);

            GL.ShaderSource(shader, code);
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


        /// <summary>
        /// attributeをDictionaryに追加
        /// </summary>
        /// <param name="info">シェーダ情報</param>
        /// <param name="code">コードの1行</param>
        private static void AttributeParameter(ShaderProgramInfo info, string[] code)
        {
            if (code[0] == "attribute" || code[0] == "in")
            {
            }
            else
            {
                return;
            }

            GetAttributeVariableCode(info, code[1], code[2]);
        }

        /// <summary>
        /// uniformをDictionaryに追加
        /// </summary>
        /// <param name="info">シェーダ情報</param>
        /// <param name="code">コードの1行</param>
        private static void UniformParameter(ShaderProgramInfo info, string[] code)
        {
            if (code[0] != "uniform")
            {
                return;
            }

            GetUniformVariableCode(info, code[1], code[2]);
        }

        /// <summary>
        /// シェーダ解析
        /// </summary>
        /// <param name="info">シェーダ情報</param>
        /// <param name="variable">型</param>
        /// <param name="name">名前</param>
        private static void GetAttributeVariableCode(ShaderProgramInfo info, string variable, string name)
        {
            switch (variable)
            {
                case "vec2":
                    info.VauleType = ValueType.Vec2Array;
                    break;
                case "vec3":
                    info.VauleType = ValueType.Vec3Array;
                    break;
                case "vec4":
                    info.VauleType = ValueType.Vec4Array;
                    break;
                case "int":
                    info.VauleType = ValueType.IntArray;
                    break;
                case "float":
                case "double":
                    info.VauleType = ValueType.FloatArray;
                    break;
                default:
                    Logger.Log(Logger.LogLevel.Error, "Shader ReadError" + name);
                    break;
            }

            return;
        }

        /// <summary>
        /// シェーダ解析
        /// </summary>
        /// <param name="info">シェーダ情報</param>
        /// <param name="variable">型</param>
        /// <param name="name">名前</param>
        private static void GetUniformVariableCode(ShaderProgramInfo info, string variable, string name)
        {
            //[]を含んでいたら配列
            if (name.Contains("[") && name.Contains("]"))
            {
                switch (variable)
                {
                    case "vec2":
                        info.VauleType = ValueType.Vec2Array;
                        return;
                    case "vec3":
                        info.VauleType = ValueType.Vec3Array;
                        return;
                    case "vec4":
                        info.VauleType = ValueType.Vec4Array;
                        return;
                    case "int":
                        info.VauleType = ValueType.IntArray;
                        return;
                    case "float":
                        info.VauleType = ValueType.FloatArray;
                        return;
                    case "double":
                        info.VauleType = ValueType.DoubleArray;
                        return;
                }
            }

            switch (variable)
            {
                case "vec2":
                    info.VauleType = ValueType.Vec2;
                    return;
                case "vec3":
                    info.VauleType = ValueType.Vec3;
                    return;
                case "vec4":
                    info.VauleType = ValueType.Vec4;
                    return;
                case "int":
                    info.VauleType = ValueType.Int;
                    return;
                case "sampler2D":
                    info.VauleType = ValueType.Texture2D;
                    return;
                case "samplerCube":
                    info.VauleType = ValueType.Cubemap;
                    return;
                case "sampler3D":
                    info.VauleType = ValueType.Texture3D;
                    break;
                case "float":
                    info.VauleType = ValueType.Float;
                    return;
                case "double":
                    info.VauleType = ValueType.Double;
                    return;
                case "mat3":
                    info.VauleType = ValueType.Mat3;
                    return;
                case "mat4":
                    info.VauleType = ValueType.Mat4;
                    return;
                default:
                    Logger.Log(Logger.LogLevel.Error, "Shader ReadError" + name);
                    return;
            }

            return;
        }
    }
}
