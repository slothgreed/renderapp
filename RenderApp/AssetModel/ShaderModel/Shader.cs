using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using RenderApp.GLUtil;
namespace RenderApp.AssetModel.ShaderModel
{
    public class Shader : Asset
    {
       
        #region [member value]
        public Shader()
        {

        }
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
        /// Deffard or MultiBuffer;
        /// </summary>
        private ERenderMode _renderMode = ERenderMode.Forward;
        public ERenderMode RenderMode
        {
            get
            {
                return _renderMode;
            }
            private set
            {
                _renderMode = value;
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
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram geom, ShaderProgram tcs, ShaderProgram tes)
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

        public void BindBuffer(Geometry geometry)
        {
            GL.UseProgram(Program);

            int activeCount = 0;
            foreach (ShaderProgramInfo loop in _shaderVariable.Values)
            {
                if (loop.ID == -1)
                {
                    continue;
                }
                if (loop.variableType == EVariableType.Uniform)
                {
                    BindUniformState(loop, ref activeCount);
                }
                if (loop.variableType == EVariableType.Attribute)
                {
                    BindAttributeState(geometry, loop);
                }
            }
        }
        public void UnBindBuffer()
        {
            foreach (ShaderProgramInfo loop in _shaderVariable.Values)
            {
                if (loop.variableType == EVariableType.Uniform)
                {
                    if (loop.ID != -1)
                    {
                        GL.DisableVertexAttribArray(loop.ID);
                    }
                }
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            Output.GLError();
        }

        /// <summary>
        /// Uniform変数のBinding
        /// </summary>
        /// <param name="uniform"></param>
        private void BindUniformState(ShaderProgramInfo uniform, ref int activeCount)
        {
            if (uniform.ID == -1)
            {
                return;
            }
            if (uniform.variable.GetType() == typeof(Vector2))
            {
                GL.Uniform2(uniform.ID, (Vector2)uniform.variable);
            }
            else if (uniform.variable.GetType() == typeof(Vector3))
            {
                GL.Uniform3(uniform.ID, (Vector3)uniform.variable);
            }
            else if (uniform.variable.GetType() == typeof(Matrix3))
            {
                Matrix3 tmp = (Matrix3)uniform.variable;
                GL.UniformMatrix3(uniform.ID, false, ref tmp);
            }
            else if (uniform.variable.GetType() == typeof(Matrix4))
            {
                Matrix4 tmp = (Matrix4)uniform.variable;
                GL.UniformMatrix4(uniform.ID, false, ref tmp);
            }
            else if (uniform.variable.GetType() == typeof(Texture))
            {
                GL.ActiveTexture(TextureUnit.Texture0 + activeCount);
                GL.BindTexture(TextureTarget.Texture2D, ((Texture)uniform.variable).ID);
                GL.Uniform1(uniform.ID, activeCount);
                activeCount++;
            }
            else
            {
                GL.Uniform1(uniform.ID, (float)uniform.variable);
            }
            Output.GLError();
        }
        /// <summary>
        /// AttributeのBinding
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="attribute"></param>
        private void BindAttributeState(Geometry geometry, ShaderProgramInfo attribute)
        {
            if (attribute.ID == -1)
            {
                return;
            }
            if (attribute.Name == "index")
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, attribute.VertexBufferId);
                List<int> tmp = (List<int>)attribute.variable;
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(tmp.Count * sizeof(int)), tmp.ToArray(), BufferUsageHint.StaticDraw);
            }
            else
            {
                GL.EnableVertexAttribArray(attribute.ID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, attribute.VertexBufferId);
                if (attribute.variable is List<Vector2>)
                {
                    List<Vector2> tmp = (List<Vector2>)attribute.variable;
                    GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, new IntPtr(tmp.Count * Vector2.SizeInBytes), tmp.ToArray(), BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(attribute.ID, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
                }
                if (attribute.variable is List<Vector3>)
                {
                    List<Vector3> tmp = (List<Vector3>)attribute.variable;
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(tmp.Count * Vector3.SizeInBytes), tmp.ToArray(), BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(attribute.ID, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
                }
                if (attribute.variable is List<Vector4>)
                {
                    List<Vector4> tmp = (List<Vector4>)attribute.variable;
                    GL.BufferData<Vector4>(BufferTarget.ArrayBuffer, new IntPtr(tmp.Count * Vector4.SizeInBytes), tmp.ToArray(), BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(attribute.ID, 4, VertexAttribPointerType.Float, false, Vector4.SizeInBytes, 0);
                }
                if (attribute.variable is List<int>)
                {
                    List<int> tmp = (List<int>)attribute.variable;
                    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(tmp.Count * sizeof(int)), tmp.ToArray(), BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                }

            }
            Output.GLError();
        }
        #endregion

        /// <summary>
        /// 初期状態の設定
        /// </summary>
        public void InitializeState(Geometry geometry,Dictionary<TextureKind,Texture> TextureItem)
        {
            foreach (ShaderProgramInfo variable in _shaderVariable.Values)
            {
                switch (variable.Name)
                {
                    case "position":
                        variable.variable = geometry.Position;
                        break;
                    case "normal":
                        variable.variable = geometry.Normal;
                        break;
                    case "color":
                        variable.variable = geometry.Color;
                        break;
                    case "texcoord":
                        variable.variable = geometry.TexCoord;
                        break;
                    case "index":
                        variable.variable = geometry.Index;
                        break;
                    case "uMVP":
                        Matrix4 vp = Scene.ActiveScene.MainCamera.CameraProjMatrix;
                        variable.variable = geometry.ModelMatrix * vp;
                        break;
                    case "uModelMatrix":
                        variable.variable = geometry.ModelMatrix;
                        break;
                    case "uNormalMatrix":
                        variable.variable = geometry.NormalMatrix;
                        break;
                    case "uProjectMatrix":
                        variable.variable = Scene.ActiveScene.MainCamera.ProjMatrix;
                        break;
                    case "uCameraPosition":
                        variable.variable = Scene.ActiveScene.MainCamera.Position;
                        break;
                    case "uCameraMatrix":
                        variable.variable = Scene.ActiveScene.MainCamera.Matrix;
                        break;
                    case "uLightPosition":
                        variable.variable = Scene.ActiveScene.SunLight.Position;
                        break;
                    case "uLightDirection":
                        variable.variable = Scene.ActiveScene.SunLight.Direction;
                        break;
                    case "uLightMatrix":
                        variable.variable = Scene.ActiveScene.SunLight.Matrix;
                        break;
                    case "uAlbedoMap":
                        if (TextureItem.ContainsKey(TextureKind.Albedo))
                        {
                            variable.variable = TextureItem[TextureKind.Albedo];
                        }
                        break;
                    case "uNormalMap":
                        if (TextureItem.ContainsKey(TextureKind.Normal))
                        {
                            variable.variable = TextureItem[TextureKind.Normal];
                        }
                        break;
                    case "uHeightMap":
                        if (TextureItem.ContainsKey(TextureKind.Height))
                        {
                            variable.variable = TextureItem[TextureKind.Height];
                        }
                        break;
                    case "uEmissiveMap":
                        if (TextureItem.ContainsKey(TextureKind.Emissive))
                        {
                            variable.variable = TextureItem[TextureKind.Emissive];
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
        public void AnalizeShaderProgram()
        {
            _shaderVariable.Clear();
            GL.UseProgram(_program);
            foreach(ShaderProgram loop in _activeShader)
            {
                AnalizeShaderProgram(loop);
            }
            SetShaderVariableID();

            // index buffer
            ShaderProgramInfo info = new ShaderProgramInfo();
            info.Name = "index";
            info.variable = new List<int>();
            info.variableType = EVariableType.Attribute;
            info.VertexBufferId = GL.GenBuffer();
            _shaderVariable.Add(info.Name, info);

        }
        /// <summary>
        /// attributeをDictionaryに追加
        /// </summary>
        /// <param name="code"></param>
        private object AttributeParameter(string[] code)
        {
            if (code[0] == "attribute" || code[0] == "in")
            {

            }
            else
            {
                return null;
            }
            return GetAttributeVariableCode(code[1], code[2]);
        }
        /// <summary>
        /// uniformをDictionaryに追加
        /// </summary>
        /// <param name="code"></param>
        private object UniformParameter(string[] code)
        {
            if (code[0] != "uniform")
            {
                return null;
            }
            return GetUniformVariableCode(code[1], code[2]);
        }
        /// <summary>
        /// シェーダ解析
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="name"></param>
        private object GetAttributeVariableCode(string variable, string name)
        {
            object shaderVariable = null;
            switch (variable)
            {
                case "vec2":
                    shaderVariable = new List<Vector2>();
                    break;
                case "vec3":
                    shaderVariable = new List<Vector3>();
                    break;
                case "vec4":
                    shaderVariable = new List<Vector4>();
                    break;
                case "int":
                    shaderVariable = new List<int>();
                    break;
                case "float":
                case "double":
                    shaderVariable = new List<float>();
                    break;
                default:
                    Console.WriteLine("Shader ReadError" + name);
                    break;
            }
            return shaderVariable;
        }

        /// <summary>
        /// シェーダ解析
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="name"></param>
        private object GetUniformVariableCode(string variable, string name)
        {
            object shaderVariable = null;
            switch (variable)
            {
                case "vec2":
                    shaderVariable = new Vector2();
                    break;
                case "vec3":
                    shaderVariable = new Vector3();
                    break;
                case "vec4":
                    shaderVariable = new Vector4();
                    break;
                case "int":
                case "sampler2D":
                case "sampler3D":
                    shaderVariable = new Texture();
                    break;
                case "float":
                    shaderVariable = .0f;
                    break;
                case "double":
                    shaderVariable = 0.0;
                    break;
                case "mat3":
                    shaderVariable = new Matrix3();
                    break;
                case "mat4":
                    shaderVariable = new Matrix4();
                    break;
                default:
                    Console.WriteLine("Shader ReadError" + name);
                    break;
            }
            return shaderVariable;
        }
        /// <summary>
        /// UniformとAttributeの指定
        /// </summary>
        private void AnalizeShaderProgram(ShaderProgram shader)
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
                object variable = null;
                switch (code[0])
                {
                    case "attribute":
                    case "in":
                        variable = AttributeParameter(code);
                        if (variable != null && !_shaderVariable.ContainsKey(code[2]))
                        {
                            ShaderProgramInfo info = new ShaderProgramInfo();
                            info.Name = code[2];
                            info.variable = variable;
                            info.variableType = EVariableType.Attribute;
                            info.VertexBufferId = GL.GenBuffer();
                            _shaderVariable.Add(info.Name, info);
                        }
                        break;
                    case "uniform":
                        variable = UniformParameter(code);
                        if (variable != null && !_shaderVariable.ContainsKey(code[2]))
                        {
                            ShaderProgramInfo info = new ShaderProgramInfo();
                            info.Name = code[2];
                            info.variable = variable;
                            info.variableType = EVariableType.Uniform;
                            _shaderVariable.Add(info.Name, info);
                        }
                        break;
                }
            }
            Output.GLError();
        }
        #endregion
        

        #region [virtual process]
        /// <summary>
        /// シェーダの生成後に呼び出す。
        /// </summary>
        public void Initialize()
        {
            FilePath = _vert.FilePath;
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
            Scene.ActiveScene.AddSceneObject(this.ToString(), this);
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
                switch(loop.variableType)
                {
                    case EVariableType.Attribute:
                        loop.ID = GL.GetAttribLocation(_program, loop.Name);
                        break;
                    case EVariableType.Uniform:
                        loop.ID = GL.GetUniformLocation(_program, loop.Name);
                        break;
                }
            }
            Output.GLError();
        }
        
        #endregion
        
        #region [fin process]
        /// <summary>
        /// 解放
        /// </summary>
        public override void Dispose()
        {
            GL.DeleteProgram(_program);
            Output.GLError();
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
            Output.GLError();
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
            Console.WriteLine(info);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                Console.WriteLine(GL.GetProgramInfoLog(program));
            }
            Output.GLError();
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
                Console.WriteLine(shaderType.ToString());
                Console.WriteLine(info);
            }
            Output.GLError();
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
            int inType = 3;
            int outType = 3;

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
            Console.WriteLine(info);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                throw new Exception(GL.GetProgramInfoLog(program));
            }
            Output.GLError();
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
            Console.WriteLine(info);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                throw new Exception(GL.GetProgramInfoLog(program));
            }
            Output.GLError();
            return program;
        }
        #endregion
        

    }
}
