using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx;
using KI.Gfx.KIAsset;
namespace KI.Gfx.KIShader
{
    public class Shader
    {
       
        #region [member value]
        /// <summary>
        /// シェーダプログラム
        /// </summary>
        private int _program = -1;
        public int Program
        {
            get
            {
                return _program;
            }
        }
        
        /// <summary>
        /// 出力バッファ数
        /// </summary>
        public int OutputBufferNum
        {
            get;
            private set;
        }
        ShaderProgram _vert;
        public ShaderProgram VertexShader
        {
            get { return _vert; }
        }
        ShaderProgram _frag;
        public ShaderProgram FragShader
        {
            get { return _frag; }
        }
        ShaderProgram _geom;
        public ShaderProgram GeomShader
        {
            get { return _geom; }
        }
        ShaderProgram _tcs;
        public ShaderProgram TcsShader
        {
            get { return _tcs; }
        }
        ShaderProgram _tes;
        public ShaderProgram TesShader
        {
            get { return _tes; }
        }
        List<ShaderProgram> _activeShader = new List<ShaderProgram>();
        public List<ShaderProgram> ActiveShader
        {
            get
            {
                return _activeShader;
            }
        }
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

        #region [constructor]
        public Shader(ShaderProgram vert, ShaderProgram frag)
        {
            if(vert.shaderType == ShaderType.VertexShader && frag.shaderType == ShaderType.FragmentShader)
            {
                _vert = vert;
                _frag = frag;
                _program = CreateShaderProgram(vert.ShaderCode, frag.ShaderCode);
            }
            Initialize();
        }
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram geom)
        {
            if (vert.shaderType == ShaderType.VertexShader &&
                frag.shaderType == ShaderType.FragmentShader &&
                geom.shaderType == ShaderType.GeometryShader)
            {
                _vert = vert;
                _frag = frag;
                _geom = geom;
                _program = CreateShaderProgram(vert.ShaderCode, frag.ShaderCode, geom.ShaderCode, 3, 3);
            }
            Initialize();
        }
        public Shader( ShaderProgram vert, ShaderProgram frag, ShaderProgram geom, ShaderProgram tcs, ShaderProgram tes)
        {
            if (vert.shaderType == ShaderType.VertexShader &&
                frag.shaderType == ShaderType.FragmentShader &&
                geom.shaderType == ShaderType.GeometryShader &&
                tcs.shaderType == ShaderType.TessControlShader)
            {
                _vert = vert;
                _frag = frag;
                _geom = geom;
                _tcs = tcs;
                _tes = tes;
                _program = CreateShaderProgram(vert.ShaderCode, frag.ShaderCode, geom.ShaderCode, tcs.ShaderCode, tes.ShaderCode);
            }
            Initialize();
        }
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram tcs, ShaderProgram tes)
        {
            if (vert.shaderType == ShaderType.VertexShader &&
                frag.shaderType == ShaderType.FragmentShader &&
                tcs.shaderType == ShaderType.TessControlShader && 
                tes.shaderType == ShaderType.TessEvaluationShader)
            {
                _vert = vert;
                _frag = frag;
                _tcs = tcs;
                _tes = tes;
                _program = CreateShaderProgram(vert.ShaderCode, frag.ShaderCode, tcs.ShaderCode, tes.ShaderCode);
            }
            Initialize();
        }
        #endregion
        #region [bind buffer]
        private int ActiveTextureCounter;
        public void BindBuffer()
        {
            GL.UseProgram(Program);

            ActiveTextureCounter = 0;
            foreach (ShaderProgramInfo loop in _shaderVariable.Values)
            {
                if (loop.ShaderID == -1)
                {
                    continue;
                }
                if (loop.shaderVariableType == EShaderVariableType.Uniform)
                {
                    BindUniformState(loop, ref ActiveTextureCounter);
                }
                if (loop.shaderVariableType == EShaderVariableType.Attribute)
                {
                    BindAttributeState(loop);
                }
            }
        }
        /// <summary>
        /// AttributeのBinding
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="attribute"></param>
        public static void BindAttributeState(ShaderProgramInfo attribute)
        {
            if (attribute.ShaderID == -1)
            {
                return;
            }
            if (attribute.variable is ArrayBuffer)
            {
                var array = attribute.variable as ArrayBuffer;
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
        public void UnBindBuffer()
        {
            foreach (ShaderProgramInfo loop in _shaderVariable.Values)
            {
                if (loop.shaderVariableType == EShaderVariableType.Uniform)
                {
                    if (loop.ShaderID != -1)
                    {
                        GL.DisableVertexAttribArray(loop.ShaderID);
                    }
                }
            }
            for (int i = 0; i < ActiveTextureCounter; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            Logger.GLLog(Logger.LogLevel.Error);
        }


        public bool SetValue(string key, object value)
        {
            if (_shaderVariable.ContainsKey(key))
            {
                if (value is Texture)
                {
                    var texture = value as Texture;
                    _shaderVariable[key].variable = texture.DeviceID;
                    return true;
                }
                if(value is bool)
                {
                    var bValue = (bool)value;
                    if(bValue)
                    {
                        _shaderVariable[key].variable = 1;
                    }
                    else
                    {
                        _shaderVariable[key].variable = 0;
                    }
                    return true;
                }
                _shaderVariable[key].variable = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Uniform変数のBinding
        /// </summary>
        /// <param name="uniform"></param>
        private void BindUniformState(ShaderProgramInfo uniform, ref int activeCount)
        {
            if(uniform.variable == null)
            {
                Logger.Log(Logger.LogLevel.Warning,this.ToString() +  " : Shader Binding Error : " + uniform.Name);
                return;
            }
            if (uniform.ShaderID == -1)
            {
                return;
            }
            if (uniform.variableType == EVariableType.Vec2)
            {
                GL.Uniform2(uniform.ShaderID, (Vector2)uniform.variable);
            }
            else if (uniform.variableType == EVariableType.Vec3)
            {
                GL.Uniform3(uniform.ShaderID, (Vector3)uniform.variable);
            }
            else if (uniform.variableType == EVariableType.Mat3)
            {
                Matrix3 tmp = (Matrix3)uniform.variable;
                GL.UniformMatrix3(uniform.ShaderID, false, ref tmp);
            }
            else if (uniform.variableType == EVariableType.Mat4)
            {
                Matrix4 tmp = (Matrix4)uniform.variable;
                GL.UniformMatrix4(uniform.ShaderID, false, ref tmp);
            }
            else if (uniform.variableType == EVariableType.Int)
            {
                GL.Uniform1(uniform.ShaderID, (int)uniform.variable);
            }
            else if (uniform.variableType == EVariableType.Float)
            {
                GL.Uniform1(uniform.ShaderID, (float)uniform.variable);
            }
            else if (uniform.variableType == EVariableType.Texture2D)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + activeCount);
                GL.BindTexture(TextureTarget.Texture2D, (int)uniform.variable);
                GL.Uniform1(uniform.ShaderID, activeCount);
                activeCount++;
            }
            else if(uniform.variableType == EVariableType.Cubemap)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + activeCount);
                GL.BindTexture(TextureTarget.TextureCubeMap, (int)uniform.variable);
                GL.Uniform1(uniform.ShaderID, activeCount);
                activeCount++;
            }
            else if (uniform.variableType == EVariableType.IntArray)
            {
                GL.Uniform1(uniform.ShaderID, uniform.arrayNum, (int[])uniform.variable);
            }
            else if (uniform.variableType == EVariableType.FloatArray)
            {
                GL.Uniform1(uniform.ShaderID, uniform.arrayNum, (float[])uniform.variable);
            }
            else if (uniform.variableType == EVariableType.DoubleArray)
            {
                GL.Uniform1(uniform.ShaderID, uniform.arrayNum, (double[])uniform.variable);
            }else
            {
                Logger.Log(Logger.LogLevel.Warning, "BindUniformState not identify");
            }
            Logger.GLLog(Logger.LogLevel.Error);
        }
        #endregion



        private Dictionary<string, ShaderProgramInfo> _shaderVariable = new Dictionary<string, ShaderProgramInfo>();

        public IEnumerable<object> GetShaderVariable()
        {
            foreach (var loop in _shaderVariable)
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
            if (_vert != null)
            {
                _activeShader.Add(_vert);
            }
            if (_frag != null)
            {
                _activeShader.Add(_frag);
            }
            if (_geom != null)
            {
                _activeShader.Add(_geom);
            }
            if (_tcs != null)
            {
                _activeShader.Add(_tcs);
            }
            if (_tes != null)
            {
                _activeShader.Add(_tes);
            }
            AnalyzeShaderProgram();
        }
        public override string ToString()
        {
            string name = "";
            if (_vert != null)
            {
                name += "[v]" + System.IO.Path.GetFileNameWithoutExtension(_vert.FileName);
            }
            if (_frag != null)
            {
                name += "[f]" + System.IO.Path.GetFileNameWithoutExtension(_frag.FileName);
            }
            if (_geom != null)
            {
                name += "[g]" + System.IO.Path.GetFileNameWithoutExtension(_geom.FileName);
            } if (_tcs != null)
            {
                name += "[tc]" + System.IO.Path.GetFileNameWithoutExtension(_tcs.FileName);
            } if (_tes != null)
            {
                name += "[te]" + System.IO.Path.GetFileNameWithoutExtension(_tes.FileName);
            }
            return name;
        }
        
        /// <summary>
        /// Attribの設定
        /// </summary>
        private void SetShaderVariableID()
        {
            GL.UseProgram(_program);
            foreach (ShaderProgramInfo loop in _shaderVariable.Values)
            {
                switch(loop.shaderVariableType)
                {
                    case EShaderVariableType.Attribute:
                        loop.ShaderID = GL.GetAttribLocation(_program, loop.Name);
                        break;
                    case EShaderVariableType.Uniform:
                        loop.ShaderID = GL.GetUniformLocation(_program, loop.Name);
                        break;
                }
            }
            Logger.GLLog(Logger.LogLevel.Error);
        }

        #endregion

        #region [analyze shader code]
        public void AnalyzeShaderProgram()
        {
            _shaderVariable.Clear();
            GL.UseProgram(_program);

            foreach (ShaderProgram loop in _activeShader)
            {
                AnalyzeShaderProgram(loop);
            }
            SetShaderVariableID();

            // index buffer
            ShaderProgramInfo info = new ShaderProgramInfo();
            info.Name = "index";
            info.variable = new List<int>();
            info.shaderVariableType = EShaderVariableType.Attribute;
            _shaderVariable.Add(info.Name, info);

        }
        /// <summary>
        /// attributeをDictionaryに追加
        /// </summary>
        /// <param name="code"></param>
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
        /// <param name="code"></param>
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
        /// <param name="variable"></param>
        /// <param name="name"></param>
        private void GetAttributeVariableCode(ShaderProgramInfo info, string variable, string name)
        {
            switch (variable)
            {
                case "vec2":
                    info.variableType = EVariableType.Vec2Array;
                    break;
                case "vec3":
                    info.variableType = EVariableType.Vec3Array;
                    break;
                case "vec4":
                    info.variableType = EVariableType.Vec4Array;
                    break;
                case "int":
                    info.variableType = EVariableType.IntArray;
                    break;
                case "float":
                case "double":
                    info.variableType = EVariableType.FloatArray;
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
        /// <param name="variable"></param>
        /// <param name="name"></param>
        private void GetUniformVariableCode(ShaderProgramInfo info, string variable, string name)
        {
            //[]を含んでいたら配列
            if (name.Contains("[") && name.Contains("]"))
            {
                switch (variable)
                {
                    case "vec2":
                        info.variableType = EVariableType.Vec2Array;
                        return;
                    case "vec3":
                        info.variableType = EVariableType.Vec3Array;
                        return;
                    case "vec4":
                        info.variableType = EVariableType.Vec4Array;
                        return;
                    case "int":
                        info.variableType = EVariableType.IntArray;
                        return;
                    case "float":
                        info.variableType = EVariableType.FloatArray;
                        return;
                    case "double":
                        info.variableType = EVariableType.DoubleArray;
                        return;
                }
            }
            switch (variable)
            {
                case "vec2":
                    info.variableType = EVariableType.Vec2;
                    return;
                case "vec3":
                    info.variableType = EVariableType.Vec3;
                    return;
                case "vec4":
                    info.variableType = EVariableType.Vec4;
                    return;
                case "int":
                    info.variableType = EVariableType.Int;
                    return;
                case "sampler2D":
                    info.variableType = EVariableType.Texture2D;
                    return;
                case "samplerCube":
                    info.variableType = EVariableType.Cubemap;
                    return;
                case "sampler3D":
                    info.variableType = EVariableType.Texture3D;
                    break;
                case "float":
                    info.variableType = EVariableType.Float;
                    return;
                case "double":
                    info.variableType = EVariableType.Double;
                    return;
                case "mat3":
                    info.variableType = EVariableType.Mat3;
                    return;
                case "mat4":
                    info.variableType = EVariableType.Mat4;
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

                if (line.Contains("gl_FragData"))
                {
                    OutputBufferNum++;
                }
                if (line.Contains("gl_FragColor"))
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
                        if (info.variableType != EVariableType.None && !_shaderVariable.ContainsKey(code[2]))
                        {
                            info.Name = code[2];
                            info.shaderVariableType = EShaderVariableType.Attribute;
                            _shaderVariable.Add(info.Name, info);
                        }
                        Logger.GLLog(Logger.LogLevel.Error);

                        break;
                    case "uniform":
                        UniformParameter(info, code);
                        if (info.variableType != EVariableType.None && !_shaderVariable.ContainsKey(code[2]))
                        {
                            if (code[2].Contains("["))
                            {
                                string arrName = code[2];
                                int arrNum = 0;
                                string arrNumStr = "";
                                int startIndex = arrName.IndexOf("[");
                                int endIndex = arrName.IndexOf("]");
                                arrNumStr = arrName.Substring(startIndex + 1, endIndex - (startIndex + 1));
                                arrNum = int.Parse(arrNumStr);
                                arrName = arrName.Substring(0,startIndex);
                                code[2] = arrName;
                                info.arrayNum = arrNum;
                            }
                            info.Name = code[2];
                            info.shaderVariableType = EShaderVariableType.Uniform;
                            _shaderVariable.Add(info.Name, info);
                        }
                        Logger.GLLog(Logger.LogLevel.Error);

                        break;
                }
            }
            Logger.GLLog(Logger.LogLevel.Error);
        }
        #endregion

        #region [fin process]
        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            GL.DeleteProgram(_program);
            Logger.GLLog(Logger.LogLevel.Error);
        }
        #endregion

        #region [compiler]
        /// <summary>
        /// シェーダの作成
        /// </summary>
        protected int CreateShaderProgram(string vertexShaderCode, string fragmentShaderCode)
        {
            int vshader = CompailShader(ShaderType.VertexShader, vertexShaderCode);
            int fshader = CompailShader(ShaderType.FragmentShader, fragmentShaderCode);

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
            if(!string.IsNullOrWhiteSpace(info))
            {
                Logger.Log(Logger.LogLevel.Error, info);
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
        protected int CreateShaderProgram(string vertexShaderCode, string geometryShaderCode, string fragmentShaderCode, int inType, int outType, int outVertexNum = -1)
        {
            int vshader = CompailShader(ShaderType.VertexShader, vertexShaderCode);
            int fshader = CompailShader(ShaderType.FragmentShader, fragmentShaderCode);
            int gshader = CompailShader(ShaderType.GeometryShader, geometryShaderCode);

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
                Logger.Log(Logger.LogLevel.Error, info);
            } 
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                Logger.GLLog(Logger.LogLevel.Error, GL.GetProgramInfoLog(program));
            }
            Logger.GLLog(Logger.LogLevel.Error);
            return program;
        }
        private int CompailShader(ShaderType shaderType, string shaderCode)
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
                        Logger.Log(Logger.LogLevel.Error,FragShader.FileName);
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

        /// <summary>
        /// テッセレーションシェーダ用
        /// </summary>
        protected int CreateShaderProgram(string vertexShaderCode, string fragmentShaderCode, string geometryShaderCode, string tcsShaderCode, string tesShaderCode)
        {
            int vshader = CompailShader(ShaderType.VertexShader, vertexShaderCode);
            int fshader = CompailShader(ShaderType.FragmentShader, fragmentShaderCode);
            int gshader = CompailShader(ShaderType.GeometryShader, geometryShaderCode);
            int tesshader = CompailShader(ShaderType.TessEvaluationShader, tesShaderCode);
            int tcsshader = CompailShader(ShaderType.TessControlShader, tcsShaderCode);

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
                Logger.Log(Logger.LogLevel.Error, info);
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
        protected int CreateShaderProgram(string vertexShaderCode, string fragmentShaderCode, string tcsShaderCode, string tesShaderCode)
        {
            int vshader = CompailShader(ShaderType.VertexShader, vertexShaderCode);
            int fshader = CompailShader(ShaderType.FragmentShader, fragmentShaderCode);
            int tesshader = CompailShader(ShaderType.TessEvaluationShader, tesShaderCode);
            int tcsshader = CompailShader(ShaderType.TessControlShader, tcsShaderCode);


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
                Logger.Log(Logger.LogLevel.Error, info);
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


    }
}
