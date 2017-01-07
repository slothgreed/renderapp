using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using RenderApp.GLUtil;
using RenderApp.AssetModel;
namespace RenderApp.GLUtil.ShaderModel
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
        private string _code;
        public string Code
        {
            get
            {
                if(_code == null)
                {
                    _code = "";
                    foreach(var loop in _activeShader)
                    {
                        _code += _activeShader;
                    }
                }
                return _code;
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
        public void BindBuffer(Geometry geometry)
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
                    BindAttributeState(geometry, loop);
                }
            }
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
            Output.GLLog(Output.LogLevel.Error);
        }


        internal bool SetValue(string key, object value)
        {
            if (_shaderVariable.ContainsKey(key))
            {
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
                //Output.Error("Shader Binding Error" + uniform.Name);
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
            else if (uniform.variableType == EVariableType.Texture2D)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + activeCount);
                GL.BindTexture(TextureTarget.Texture2D, (int)uniform.variable);
                GL.Uniform1(uniform.ShaderID, activeCount);
                activeCount++;
            }
            else
            {
                if (uniform.variableType == EVariableType.Float)
                {
                    GL.Uniform1(uniform.ShaderID, (float)uniform.variable);
                }
                else
                {
                    GL.Uniform1(uniform.ShaderID, (int)uniform.variable);
                }
            }
            Output.GLLog(Output.LogLevel.Error);
        }
        /// <summary>
        /// AttributeのBinding
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="attribute"></param>
        private void BindAttributeState(Geometry geometry, ShaderProgramInfo attribute)
        {
            if (attribute.ShaderID == -1)
            {
                return;
            }

            if (attribute.Name == "position" && geometry.PositionBuffer != null)
            {
                GL.EnableVertexAttribArray(attribute.ShaderID);
                geometry.PositionBuffer.BindBuffer();
                GL.VertexAttribPointer(attribute.ShaderID, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
                geometry.PositionBuffer.UnBindBuffer();
            }
            if (attribute.Name == "normal" && geometry.NormalBuffer != null)
            {
                GL.EnableVertexAttribArray(attribute.ShaderID);
                geometry.NormalBuffer.SetData(geometry.Normal, Buffer.EArrayType.Vec3Array);
                geometry.NormalBuffer.BindBuffer();
                GL.VertexAttribPointer(attribute.ShaderID, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
                geometry.NormalBuffer.UnBindBuffer();
            }
            if (attribute.Name == "color" && geometry.ColorBuffer != null)
            {
                GL.EnableVertexAttribArray(attribute.ShaderID);
                geometry.NormalBuffer.SetData(geometry.Color, Buffer.EArrayType.Vec3Array);
                geometry.NormalBuffer.BindBuffer();
                GL.VertexAttribPointer(attribute.ShaderID, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
                geometry.NormalBuffer.UnBindBuffer();
            }
            if (attribute.Name == "texcoord" && geometry.TexBuffer != null)
            {
                GL.EnableVertexAttribArray(attribute.ShaderID);
                geometry.TexBuffer.SetData(geometry.TexCoord, Buffer.EArrayType.Vec2Array);
                geometry.TexBuffer.BindBuffer();
                GL.VertexAttribPointer(attribute.ShaderID, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
                geometry.TexBuffer.UnBindBuffer();
            }
            if(attribute.Name == "index" && geometry.IndexBuffer != null)
            {
                geometry.IndexBuffer.BindBuffer();
            }
            Output.GLLog(Output.LogLevel.Error);
        }
        #endregion
        /// <summary>
        /// 初期状態の設定
        /// </summary>
        public void InitializeState(Geometry geometry,Dictionary<TextureKind,Texture> TextureItem)
        {
            foreach (ShaderProgramInfo info in _shaderVariable.Values)
            {
                switch (info.Name)
                {
                    case "position":
                        info.variable = geometry.Position;
                        break;
                    case "normal":
                        info.variable = geometry.Normal;
                        break;
                    case "color":
                        info.variable = geometry.Color;
                        break;
                    case "texcoord":
                        info.variable = geometry.TexCoord;
                        break;
                    case "index":
                        info.variable = geometry.Index;
                        break;
                    case "uGeometryID":
                        info.variable = geometry.ID;
                        break;
                    case "uWidth":
                        info.variable = DeviceContext.Instance.Width;
                        break;
                    case "uHeight":
                        info.variable = DeviceContext.Instance.Height;
                        break;
                    case "uMVP":
                        Matrix4 vp = Scene.ActiveScene.MainCamera.CameraProjMatrix;
                        info.variable = geometry.ModelMatrix * vp;
                        break;
                    case "uModelMatrix":
                        info.variable = geometry.ModelMatrix;
                        break;
                    case "uNormalMatrix":
                        info.variable = geometry.NormalMatrix;
                        break;
                    case "uProjectMatrix":
                        info.variable = Scene.ActiveScene.MainCamera.ProjMatrix;
                        break;
                    case "uUnProjectMatrix":
                        info.variable = Scene.ActiveScene.MainCamera.UnProject;
                        break;
                    case "uCameraPosition":
                        info.variable = Scene.ActiveScene.MainCamera.Position;
                        break;
                    case "uCameraMatrix":
                        info.variable = Scene.ActiveScene.MainCamera.Matrix;
                        break;
                    case "uLightPosition":
                        info.variable = Scene.ActiveScene.SunLight.Position;
                        break;
                    case "uLightDirection":
                        info.variable = Scene.ActiveScene.SunLight.Direction;
                        break;
                    case "uLightMatrix":
                        info.variable = Scene.ActiveScene.SunLight.Matrix;
                        break;
                    case "uAlbedoMap":
                        if (TextureItem.ContainsKey(TextureKind.Albedo))
                        {
                            info.variable = TextureItem[TextureKind.Albedo].DeviceID;
                        }
                        break;
                    case "uSpecularMap":
                        if(TextureItem.ContainsKey(TextureKind.Specular))
                        {
                            info.variable = TextureItem[TextureKind.Specular].DeviceID;
                        }
                        break;
                    case "uWorldMap":
                        if (TextureItem.ContainsKey(TextureKind.World))
                        {
                            info.variable = TextureItem[TextureKind.World].DeviceID;
                        }
                        break;
                    case "uLightingMap":
                        if (TextureItem.ContainsKey(TextureKind.Lighting))
                        {
                            info.variable = TextureItem[TextureKind.Lighting].DeviceID;
                        }
                        break;
                    case "uNormalMap":
                        if (TextureItem.ContainsKey(TextureKind.Normal))
                        {
                            info.variable = TextureItem[TextureKind.Normal].DeviceID;
                        }
                        break;
                    case "uHeightMap":
                        if (TextureItem.ContainsKey(TextureKind.Height))
                        {
                            info.variable = TextureItem[TextureKind.Height].DeviceID;
                        }
                        break;
                    case "uEmissiveMap":
                        if (TextureItem.ContainsKey(TextureKind.Emissive))
                        {
                            info.variable = TextureItem[TextureKind.Emissive].DeviceID;
                        }
                        break;
                }
            }
        }

        private Dictionary<string, ShaderProgramInfo> _shaderVariable = new Dictionary<string, ShaderProgramInfo>();
        public IEnumerable<object> GetShaderVariable()
        {
            foreach (var loop in _shaderVariable)
            {
                yield return loop.Value;
            }
        }

        #region [analyze shader code]
        public void AnalyzeShaderProgram()
        {
            _shaderVariable.Clear();
            GL.UseProgram(_program);
            foreach(ShaderProgram loop in _activeShader)
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
        private void GetAttributeVariableCode(ShaderProgramInfo info,string variable, string name)
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
                    Output.Log(Output.LogLevel.Error,"Shader ReadError" + name);
                    break;
            }
            return;
        }

        /// <summary>
        /// シェーダ解析
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="name"></param>
        private void GetUniformVariableCode(ShaderProgramInfo info,string variable, string name)
        {
            switch (variable)
            {
                case "vec2":
                    info.variableType = EVariableType.Vec2;
                    break;
                case "vec3":
                    info.variableType = EVariableType.Vec3;
                    break;
                case "vec4":
                    info.variableType = EVariableType.Vec4;
                    break;
                case "int":
                    info.variableType = EVariableType.Int;
                    break;
                case "sampler2D":
                    info.variableType = EVariableType.Texture2D;
                    break;
                case "sampler3D":
                    info.variableType = EVariableType.Texture3D;
                    break;
                case "float":
                    info.variableType = EVariableType.Float;
                    break;
                case "double":
                    info.variableType = EVariableType.Double;
                    break;
                case "mat3":
                    info.variableType = EVariableType.Mat3;
                    break;
                case "mat4":
                    info.variableType = EVariableType.Mat4;
                    break;
                default:
                    Output.Log(Output.LogLevel.Error,"Shader ReadError" + name);
                    break;
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
                string line = lines[i];

                if(line.Contains("gl_FragData"))
                {
                    OutputBufferNum++;
                }
                if(line.Contains("gl_FragColor"))
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
                        break;
                    case "uniform":
                        UniformParameter(info,code);
                        if (info.variableType != EVariableType.None && !_shaderVariable.ContainsKey(code[2]))
                        {
                            info.Name = code[2];
                            info.shaderVariableType = EShaderVariableType.Uniform;
                            _shaderVariable.Add(info.Name, info);
                        }
                        break;
                }
            }
            Output.GLLog(Output.LogLevel.Error);
        }
        #endregion
        

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
                name += "\r\n[f]" + System.IO.Path.GetFileNameWithoutExtension(_frag.FileName);
            }
            if (_geom != null)
            {
                name += "\r\n[g]" + System.IO.Path.GetFileNameWithoutExtension(_geom.FileName);
            } if (_tcs != null)
            {
                name += "\r\n[tc]" + System.IO.Path.GetFileNameWithoutExtension(_tcs.FileName);
            } if (_tes != null)
            {
                name += "\r\n[te]" + System.IO.Path.GetFileNameWithoutExtension(_tes.FileName);
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
            Output.GLLog(Output.LogLevel.Error);
        }

        #endregion
        
        #region [fin process]
        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            GL.DeleteProgram(_program);
            Output.GLLog(Output.LogLevel.Error);
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
            Output.GLLog(Output.LogLevel.Error);
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
            Output.Log(Output.LogLevel.Debug, info);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                Output.Log(Output.LogLevel.Error, GL.GetProgramInfoLog(program));
            }
            Output.GLLog(Output.LogLevel.Error);
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
                        Output.Log(Output.LogLevel.Debug,FragShader.FileName);
                        break;
                    case ShaderType.GeometryShader:
                        Output.Log(Output.LogLevel.Debug, GeomShader.FileName);
                        break;
                    case ShaderType.TessControlShader:
                        Output.Log(Output.LogLevel.Debug, TesShader.FileName);
                        break;
                    case ShaderType.TessEvaluationShader:
                        Output.Log(Output.LogLevel.Debug, TcsShader.FileName);
                        break;
                    case ShaderType.VertexShader:
                        Output.Log(Output.LogLevel.Debug, VertexShader.FileName);
                        break;
                    default:
                        break;
                }
                Output.Log(Output.LogLevel.Debug, info);
            }
            Output.GLLog(Output.LogLevel.Error);
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
            Output.Log(Output.LogLevel.Debug, info);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                throw new Exception(GL.GetProgramInfoLog(program));
            }
            Output.GLLog(Output.LogLevel.Error);
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
            Output.Log(Output.LogLevel.Debug, info);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                throw new Exception(GL.GetProgramInfoLog(program));
            }
            Output.GLLog(Output.LogLevel.Error);
            return program;
        }
        #endregion


    }
}
