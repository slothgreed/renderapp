using System;
using System.Collections.Generic;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.KIShader
{
    /// <summary>
    /// シェーダ
    /// </summary>
    public class Shader
    {
        #region [constructor]
        /// <summary>
        /// シェーダ変数
        /// </summary>
        private Dictionary<string, ShaderProgramInfo> shaderVariable = new Dictionary<string, ShaderProgramInfo>();

        /// <summary>
        /// アクティブテクスチャのカウンタ
        /// </summary>
        private int activeTextureCounter;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        public Shader(ShaderProgram vert, ShaderProgram frag)
        {
            if (vert.ShaderType == ShaderType.VertexShader && frag.ShaderType == ShaderType.FragmentShader)
            {
                VertexShader = vert;
                FragShader = frag;
                Program = CreateShaderProgram(vert.ShaderCode, frag.ShaderCode);
            }

            Initialize();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        /// <param name="geom">ジオメトリシェーダ</param>
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram geom)
        {
            if (vert.ShaderType == ShaderType.VertexShader &&
                frag.ShaderType == ShaderType.FragmentShader &&
                geom.ShaderType == ShaderType.GeometryShader)
            {
                VertexShader = vert;
                FragShader = frag;
                GeomShader = geom;
                Program = CreateShaderProgram(vert.ShaderCode, frag.ShaderCode, geom.ShaderCode, 3, 3);
            }

            Initialize();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        /// <param name="geom">ジオメトリシェーダ</param>
        /// <param name="tcs">テッセレーション制御シェーダ</param>
        /// <param name="tes">テッセレーション評価シェーダ</param>
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram geom, ShaderProgram tcs, ShaderProgram tes)
        {
            if (vert.ShaderType == ShaderType.VertexShader &&
                frag.ShaderType == ShaderType.FragmentShader &&
                geom.ShaderType == ShaderType.GeometryShader &&
                tcs.ShaderType == ShaderType.TessControlShader)
            {
                VertexShader = vert;
                FragShader = frag;
                GeomShader = geom;
                TcsShader = tcs;
                TesShader = tes;
                Program = CreateShaderProgram(vert.ShaderCode, frag.ShaderCode, geom.ShaderCode, tcs.ShaderCode, tes.ShaderCode);
            }

            Initialize();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        /// <param name="tcs">テッセレーション制御シェーダ</param>
        /// <param name="tes">テッセレーション評価シェーダ</param>
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram tcs, ShaderProgram tes)
        {
            if (vert.ShaderType == ShaderType.VertexShader &&
                frag.ShaderType == ShaderType.FragmentShader &&
                tcs.ShaderType == ShaderType.TessControlShader &&
                tes.ShaderType == ShaderType.TessEvaluationShader)
            {
                VertexShader = vert;
                FragShader = frag;
                TcsShader = tcs;
                TesShader = tes;
                Program = CreateShaderProgram(vert.ShaderCode, frag.ShaderCode, tcs.ShaderCode, tes.ShaderCode);
            }

            Initialize();
        }
        #endregion
        #region [member value]

        /// <summary>
        /// シェーダプログラム
        /// </summary>
        public int Program { get; private set; } = -1;

        /// <summary>
        /// 出力バッファ数
        /// </summary>
        public int OutputBufferNum { get; private set; }

        /// <summary>
        /// 頂点シェーダ
        /// </summary>
        public ShaderProgram VertexShader { get; private set; }

        /// <summary>
        /// フラグメントシェーダ
        /// </summary>
        public ShaderProgram FragShader { get; private set; }

        /// <summary>
        /// ジオメトリシェーダ
        /// </summary>
        public ShaderProgram GeomShader { get; private set; }

        /// <summary>
        /// テッセレーション制御シェーダ
        /// </summary>
        public ShaderProgram TcsShader { get; private set; }

        /// <summary>
        /// テッセレーション評価シェーダ
        /// </summary>
        public ShaderProgram TesShader { get; private set; }

        /// <summary>
        /// 利用しているシェーダ
        /// </summary>
        private List<ShaderProgram> ActiveShader { get; set; }

        #endregion

        public bool ExistShaderProgram(ShaderProgram prog, string path)
        {
            if (prog == null)
            {
                return false;
            }

            if (prog.FilePath == path)
            {
                return true;
            }

            return false;
        }

        public bool FindShaderCombi(string vert, string frag)
        {
            if (ExistShaderProgram(VertexShader, vert) &&
               ExistShaderProgram(FragShader, frag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FindShaderCombi(string vert, string frag, string geom)
        {
            if (ExistShaderProgram(VertexShader, vert) &&
               ExistShaderProgram(FragShader, frag) &&
               ExistShaderProgram(GeomShader, geom))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FindShaderCombi(string vert, string frag, string tes, string tcs)
        {
            if (ExistShaderProgram(VertexShader, vert) &&
               ExistShaderProgram(FragShader, frag) &&
               ExistShaderProgram(TesShader, tes) &&
               ExistShaderProgram(TcsShader, tcs))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FindShaderCombi(string vert, string frag, string geom, string tes, string tcs)
        {
            if (ExistShaderProgram(VertexShader, vert) &&
               ExistShaderProgram(FragShader, frag) &&
               ExistShaderProgram(GeomShader, geom) &&
               ExistShaderProgram(TesShader, tes) &&
               ExistShaderProgram(TcsShader, tcs))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region [bind buffer]

        /// <summary>
        /// シェーダの値をバインド
        /// </summary>
        public void BindBuffer()
        {
            GL.UseProgram(Program);

            activeTextureCounter = 0;
            foreach (ShaderProgramInfo loop in shaderVariable.Values)
            {
                if (loop.ShaderID == -1)
                {
                    continue;
                }

                if (loop.ShaderVariableType == ShaderVariableType.Uniform)
                {
                    BindUniformState(loop, ref activeTextureCounter);
                }

                if (loop.ShaderVariableType == ShaderVariableType.Attribute)
                {
                    BindAttributeState(loop);
                }
            }
        }

        /// <summary>
        /// AttributeのBinding
        /// </summary>
        /// <param name="attribute">アトリビュート変数情報</param>
        public void BindAttributeState(ShaderProgramInfo attribute)
        {
            if (attribute.ShaderID == -1)
            {
                return;
            }

            if (attribute.Variable is ArrayBuffer)
            {
                var array = attribute.Variable as ArrayBuffer;
                if (attribute.Name != "index")
                {
                    GL.EnableVertexAttribArray(attribute.ShaderID);
                    array.BindBuffer();

                    switch (array.ArrayType)
                    {
                        case EArrayType.None:
                        case EArrayType.IntArray:
                        case EArrayType.FloatArray:
                        case EArrayType.DoubleArra:
                            //GL.VertexAttribPointer(attribute.ShaderID, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
                            break;
                        case EArrayType.Vec2Array:
                            GL.VertexAttribPointer(attribute.ShaderID, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
                            break;
                        case EArrayType.Vec3Array:
                            GL.VertexAttribPointer(attribute.ShaderID, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
                            break;
                        case EArrayType.Vec4Array:
                            GL.VertexAttribPointer(attribute.ShaderID, 4, VertexAttribPointerType.Float, false, Vector4.SizeInBytes, 0);
                            break;
                        default:
                            break;
                    }

                    array.UnBindBuffer();
                }
                else
                {
                    array.BindBuffer();
                }
            }

            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// バインドの解除
        /// </summary>
        public void UnBindBuffer()
        {
            foreach (ShaderProgramInfo loop in shaderVariable.Values)
            {
                if (loop.ShaderVariableType == ShaderVariableType.Uniform)
                {
                    if (loop.ShaderID != -1)
                    {
                        GL.DisableVertexAttribArray(loop.ShaderID);
                    }
                }
            }

            for (int i = 0; i < activeTextureCounter; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="name">変数名</param>
        /// <param name="value">値</param>
        /// <returns>成功</returns>
        public bool SetValue(string name, object value)
        {
            if (shaderVariable.ContainsKey(name))
            {
                if (value is Texture)
                {
                    var texture = value as Texture;
                    shaderVariable[name].Variable = texture.DeviceID;
                    return true;
                }

                if (value is bool)
                {
                    var bValue = (bool)value;
                    if (bValue)
                    {
                        shaderVariable[name].Variable = 1;
                    }
                    else
                    {
                        shaderVariable[name].Variable = 0;
                    }

                    return true;
                }

                shaderVariable[name].Variable = value;
                return true;
            }

            return false;
        }

        #endregion

        /// <summary>
        /// シェーダ変数の取得
        /// </summary>
        /// <returns>シェーダ変数</returns>
        public IEnumerable<object> GetShaderVariable()
        {
            foreach (var loop in shaderVariable)
            {
                yield return loop.Value;
            }
        }

        #region [virtual process]
        /// <summary>
        /// シェーダの生成後に呼び出す。
        /// </summary>
        public void Initialize()
        {
            ActiveShader = new List<ShaderProgram>();

            if (VertexShader != null)
            {
                ActiveShader.Add(VertexShader);
            }

            if (FragShader != null)
            {
                ActiveShader.Add(FragShader);
            }

            if (GeomShader != null)
            {
                ActiveShader.Add(GeomShader);
            }

            if (TcsShader != null)
            {
                ActiveShader.Add(TcsShader);
            }

            if (TesShader != null)
            {
                ActiveShader.Add(TesShader);
            }

            AnalyzeShaderProgram();
        }

        /// <summary>
        /// オブジェクトを表す文字列
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            string name = string.Empty;
            if (VertexShader != null)
            {
                name += "[v]" + System.IO.Path.GetFileNameWithoutExtension(VertexShader.FileName);
            }

            if (FragShader != null)
            {
                name += "[f]" + System.IO.Path.GetFileNameWithoutExtension(FragShader.FileName);
            }

            if (GeomShader != null)
            {
                name += "[g]" + System.IO.Path.GetFileNameWithoutExtension(GeomShader.FileName);
            }

            if (TcsShader != null)
            {
                name += "[tc]" + System.IO.Path.GetFileNameWithoutExtension(TcsShader.FileName);
            }

            if (TesShader != null)
            {
                name += "[te]" + System.IO.Path.GetFileNameWithoutExtension(TesShader.FileName);
            }

            return name;
        }

        #endregion

        #region [analyze shader code]

        /// <summary>
        /// シェーダプログラムの解析
        /// </summary>
        public void AnalyzeShaderProgram()
        {
            shaderVariable.Clear();
            GL.UseProgram(Program);
            Logger.GLLog(Logger.LogLevel.Error);

            foreach (ShaderProgram loop in ActiveShader)
            {
                AnalyzeShaderProgram(loop);
            }

            SetShaderVariableID();

            // index buffer
            ShaderProgramInfo info = new ShaderProgramInfo();
            info.Name = "index";
            info.Variable = new List<int>();
            info.ShaderVariableType = ShaderVariableType.Attribute;
            shaderVariable.Add(info.Name, info);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            GL.DeleteProgram(Program);
            Logger.GLLog(Logger.LogLevel.Error);
        }

        #region [compiler]
        /// <summary>
        /// シェーダの作成
        /// </summary>
        /// <param name="vertexShaderCode">頂点シェーダ</param>
        /// <param name="fragmentShaderCode">フラグシェーダ</param>
        /// <returns>プログラムID</returns>
        protected int CreateShaderProgram(string vertexShaderCode, string fragmentShaderCode)
        {
            int vshader = CompileShader(ShaderType.VertexShader, vertexShaderCode);
            int fshader = CompileShader(ShaderType.FragmentShader, fragmentShaderCode);

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
                Logger.GLLog(Logger.LogLevel.Error, GL.GetProgramInfoLog(program) + VertexShader.FileName);
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
        protected int CreateShaderProgram(string vertexShaderCode, string geometryShaderCode, string fragmentShaderCode, int inType, int outType, int outVertexNum = -1)
        {
            int vshader = CompileShader(ShaderType.VertexShader, vertexShaderCode);
            int fshader = CompileShader(ShaderType.FragmentShader, fragmentShaderCode);
            int gshader = CompileShader(ShaderType.GeometryShader, geometryShaderCode);

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
        /// <param name="vertexShaderCode">頂点シェーダ</param>
        /// <param name="fragmentShaderCode">フラグシェーダ</param>
        /// <param name="geometryShaderCode">ジオメトリシェーダ</param>
        /// <param name="tcsShaderCode">テッセレーション制御シェーダ</param>
        /// <param name="tesShaderCode">テッセレーション評価シェーダ</param>
        /// <returns>プログラムID</returns>
        protected int CreateShaderProgram(string vertexShaderCode, string fragmentShaderCode, string geometryShaderCode, string tcsShaderCode, string tesShaderCode)
        {
            int vshader = CompileShader(ShaderType.VertexShader, vertexShaderCode);
            int fshader = CompileShader(ShaderType.FragmentShader, fragmentShaderCode);
            int gshader = CompileShader(ShaderType.GeometryShader, geometryShaderCode);
            int tesshader = CompileShader(ShaderType.TessEvaluationShader, tesShaderCode);
            int tcsshader = CompileShader(ShaderType.TessControlShader, tcsShaderCode);

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
        protected int CreateShaderProgram(string vertexShaderCode, string fragmentShaderCode, string tcsShaderCode, string tesShaderCode)
        {
            int vshader = CompileShader(ShaderType.VertexShader, vertexShaderCode);
            int fshader = CompileShader(ShaderType.FragmentShader, fragmentShaderCode);
            int tesshader = CompileShader(ShaderType.TessEvaluationShader, tesShaderCode);
            int tcsshader = CompileShader(ShaderType.TessControlShader, tcsShaderCode);

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
        #endregion

        /// <summary>
        /// Uniform変数のBinding
        /// </summary>
        /// <param name="uniform">バインドするuniform情報</param>
        /// <param name="activeCount">テクスチャのアクティブカウント</param>
        private void BindUniformState(ShaderProgramInfo uniform, ref int activeCount)
        {
            if (uniform.Variable == null)
            {
                Logger.Log(Logger.LogLevel.Warning, this.ToString() + " : Shader Binding Error : " + uniform.Name);
                return;
            }

            if (uniform.ShaderID == -1)
                return;

            if (uniform.VariableType == VariableType.Vec2)
            {
                GL.Uniform2(uniform.ShaderID, (Vector2)uniform.Variable);
            }
            else if (uniform.VariableType == VariableType.Vec3)
            {
                GL.Uniform3(uniform.ShaderID, (Vector3)uniform.Variable);
            }
            else if (uniform.VariableType == VariableType.Mat3)
            {
                Matrix3 tmp = (Matrix3)uniform.Variable;
                GL.UniformMatrix3(uniform.ShaderID, false, ref tmp);
            }
            else if (uniform.VariableType == VariableType.Mat4)
            {
                Matrix4 tmp = (Matrix4)uniform.Variable;
                GL.UniformMatrix4(uniform.ShaderID, false, ref tmp);
            }
            else if (uniform.VariableType == VariableType.Int)
            {
                GL.Uniform1(uniform.ShaderID, (int)uniform.Variable);
            }
            else if (uniform.VariableType == VariableType.Float)
            {
                GL.Uniform1(uniform.ShaderID, (float)uniform.Variable);
            }
            else if (uniform.VariableType == VariableType.Texture2D)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + activeCount);
                GL.BindTexture(TextureTarget.Texture2D, (int)uniform.Variable);
                GL.Uniform1(uniform.ShaderID, activeCount);
                activeCount++;
            }
            else if (uniform.VariableType == VariableType.Cubemap)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + activeCount);
                GL.BindTexture(TextureTarget.TextureCubeMap, (int)uniform.Variable);
                GL.Uniform1(uniform.ShaderID, activeCount);
                activeCount++;
            }
            else if (uniform.VariableType == VariableType.IntArray)
            {
                GL.Uniform1(uniform.ShaderID, uniform.ArrayNum, (int[])uniform.Variable);
            }
            else if (uniform.VariableType == VariableType.FloatArray)
            {
                GL.Uniform1(uniform.ShaderID, uniform.ArrayNum, (float[])uniform.Variable);
            }
            else if (uniform.VariableType == VariableType.DoubleArray)
            {
                GL.Uniform1(uniform.ShaderID, uniform.ArrayNum, (double[])uniform.Variable);
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "BindUniformState not identify");
            }

            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// Attribの設定
        /// </summary>
        private void SetShaderVariableID()
        {
            GL.UseProgram(Program);
            foreach (ShaderProgramInfo loop in shaderVariable.Values)
            {
                switch (loop.ShaderVariableType)
                {
                    case ShaderVariableType.Attribute:
                        loop.ShaderID = GL.GetAttribLocation(Program, loop.Name);
                        break;
                    case ShaderVariableType.Uniform:
                        loop.ShaderID = GL.GetUniformLocation(Program, loop.Name);
                        break;
                }
            }

            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// attributeをDictionaryに追加
        /// </summary>
        /// <param name="info">シェーダ情報</param>
        /// <param name="code">コードの1行</param>
        private void AttributeParameter(ShaderProgramInfo info, string[] code)
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
        private void UniformParameter(ShaderProgramInfo info, string[] code)
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
        private void GetAttributeVariableCode(ShaderProgramInfo info, string variable, string name)
        {
            switch (variable)
            {
                case "vec2":
                    info.VariableType = VariableType.Vec2Array;
                    break;
                case "vec3":
                    info.VariableType = VariableType.Vec3Array;
                    break;
                case "vec4":
                    info.VariableType = VariableType.Vec4Array;
                    break;
                case "int":
                    info.VariableType = VariableType.IntArray;
                    break;
                case "float":
                case "double":
                    info.VariableType = VariableType.FloatArray;
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
        private void GetUniformVariableCode(ShaderProgramInfo info, string variable, string name)
        {
            //[]を含んでいたら配列
            if (name.Contains("[") && name.Contains("]"))
            {
                switch (variable)
                {
                    case "vec2":
                        info.VariableType = VariableType.Vec2Array;
                        return;
                    case "vec3":
                        info.VariableType = VariableType.Vec3Array;
                        return;
                    case "vec4":
                        info.VariableType = VariableType.Vec4Array;
                        return;
                    case "int":
                        info.VariableType = VariableType.IntArray;
                        return;
                    case "float":
                        info.VariableType = VariableType.FloatArray;
                        return;
                    case "double":
                        info.VariableType = VariableType.DoubleArray;
                        return;
                }
            }

            switch (variable)
            {
                case "vec2":
                    info.VariableType = VariableType.Vec2;
                    return;
                case "vec3":
                    info.VariableType = VariableType.Vec3;
                    return;
                case "vec4":
                    info.VariableType = VariableType.Vec4;
                    return;
                case "int":
                    info.VariableType = VariableType.Int;
                    return;
                case "sampler2D":
                    info.VariableType = VariableType.Texture2D;
                    return;
                case "samplerCube":
                    info.VariableType = VariableType.Cubemap;
                    return;
                case "sampler3D":
                    info.VariableType = VariableType.Texture3D;
                    break;
                case "float":
                    info.VariableType = VariableType.Float;
                    return;
                case "double":
                    info.VariableType = VariableType.Double;
                    return;
                case "mat3":
                    info.VariableType = VariableType.Mat3;
                    return;
                case "mat4":
                    info.VariableType = VariableType.Mat4;
                    return;
                default:
                    Logger.Log(Logger.LogLevel.Error, "Shader ReadError" + name);
                    return;
            }

            return;
        }

        /// <summary>
        /// UniformとAttributeの指定
        /// </summary>
        /// <param name="shader">シェーダプログラム</param>
        private void AnalyzeShaderProgram(ShaderProgram shader)
        {
            if (shader == null)
            {
                return;
            }

            string shaderCode = shader.ShaderCode;
            string[] lines = shaderCode.Split(new[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                Logger.GLLog(Logger.LogLevel.Error);

                string line = lines[i];

                if (line.Contains("OutputColor") && line.Contains("out") == false)
                {
                    OutputBufferNum++;
                }

                //セミコロン以降削除
                int colon = line.IndexOf(";");
                if (colon == 0 || colon == -1)
                {
                    continue;
                }

                line = line.Remove(colon);

                string[] derim = line.Split(' ');
                if (derim.Length < 3)
                {
                    continue;
                }

                //後半3つが信頼性高いため
                string[] code = { derim[derim.Length - 3], derim[derim.Length - 2], derim[derim.Length - 1] };
                ShaderProgramInfo info = new ShaderProgramInfo();
                switch (code[0])
                {
                    case "attribute":
                    case "in":
                        AttributeParameter(info, code);
                        if (info.VariableType != VariableType.None && !shaderVariable.ContainsKey(code[2]))
                        {
                            info.Name = code[2];
                            info.ShaderVariableType = ShaderVariableType.Attribute;
                            shaderVariable.Add(info.Name, info);
                        }

                        Logger.GLLog(Logger.LogLevel.Error);

                        break;
                    case "uniform":
                        UniformParameter(info, code);
                        if (info.VariableType != VariableType.None && !shaderVariable.ContainsKey(code[2]))
                        {
                            if (code[2].Contains("["))
                            {
                                string arrName = code[2];
                                int arrNum = 0;
                                string arrNumStr = string.Empty;
                                int startIndex = arrName.IndexOf("[");
                                int endIndex = arrName.IndexOf("]");
                                arrNumStr = arrName.Substring(startIndex + 1, endIndex - (startIndex + 1));
                                arrNum = int.Parse(arrNumStr);
                                arrName = arrName.Substring(0, startIndex);
                                code[2] = arrName;
                                info.ArrayNum = arrNum;
                            }

                            info.Name = code[2];
                            info.ShaderVariableType = ShaderVariableType.Uniform;
                            shaderVariable.Add(info.Name, info);
                        }

                        Logger.GLLog(Logger.LogLevel.Error);
                        break;
                }
            }

            Logger.GLLog(Logger.LogLevel.Error);
        }
        #endregion

        /// <summary>
        /// シェーダのコンパイル
        /// </summary>
        /// <param name="shaderType">シェーダの種類</param>
        /// <param name="shaderCode">シェーダコード</param>
        /// <returns>シェーダID</returns>
        private int CompileShader(ShaderType shaderType, string shaderCode)
        {
            int shader = GL.CreateShader(shaderType);
            string info;
            int status_code;
            GL.ShaderSource(shader, shaderCode);
            GL.CompileShader(shader);
            GL.GetShaderInfoLog(shader, out info);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out status_code);
            if (status_code != 1)
            {
                switch (shaderType)
                {
                    case ShaderType.FragmentShader:
                        Logger.Log(Logger.LogLevel.Error, FragShader.FileName);
                        break;
                    case ShaderType.GeometryShader:
                        Logger.Log(Logger.LogLevel.Error, GeomShader.FileName);
                        break;
                    case ShaderType.TessControlShader:
                        Logger.Log(Logger.LogLevel.Error, TesShader.FileName);
                        break;
                    case ShaderType.TessEvaluationShader:
                        Logger.Log(Logger.LogLevel.Error, TcsShader.FileName);
                        break;
                    case ShaderType.VertexShader:
                        Logger.Log(Logger.LogLevel.Error, VertexShader.FileName);
                        break;
                    default:
                        break;
                }

                Logger.Log(Logger.LogLevel.Error, info);
            }

            Logger.GLLog(Logger.LogLevel.Error);
            return shader;
        }
    }
}
