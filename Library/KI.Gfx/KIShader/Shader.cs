using System;
using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Linq;

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
        /// <param name="stage">シェーダステージ</param>
        public Shader(ShaderProgram vert, ShaderProgram frag)
        {
            if (vert.ShaderKind == ShaderKind.VertexShader && frag.ShaderKind == ShaderKind.FragmentShader)
            {
                ActiveShader.Add(vert);
                ActiveShader.Add(frag);
                Program = ShaderBuilder.CreateShaderProgram(vert, frag);

            }

            Initialize();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        /// <param name="geom">ジオメトリシェーダ</param>
        /// <param name="stage">シェーダステージ</param>
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram geom)
        {
            if (vert.ShaderKind == ShaderKind.VertexShader &&
                frag.ShaderKind == ShaderKind.FragmentShader &&
                geom.ShaderKind == ShaderKind.GeometryShader)
            {
                ActiveShader.Add(vert);
                ActiveShader.Add(frag);
                ActiveShader.Add(geom);
                Program = ShaderBuilder.CreateShaderProgram(vert, frag, geom, 3, 3);
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
        /// <param name="stage">シェーダステージ</param>
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram geom, ShaderProgram tcs, ShaderProgram tes)
        {
            if (vert.ShaderKind == ShaderKind.VertexShader &&
                frag.ShaderKind == ShaderKind.FragmentShader &&
                geom.ShaderKind == ShaderKind.GeometryShader &&
                tcs.ShaderKind == ShaderKind.TessControlShader)
            {
                ActiveShader.Add(vert);
                ActiveShader.Add(frag);
                ActiveShader.Add(geom);
                ActiveShader.Add(tcs);
                ActiveShader.Add(tes);
                Program = ShaderBuilder.CreateShaderProgram(vert, frag, geom, tcs, tes);
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
        /// <param name="stage">シェーダステージ</param>
        public Shader(ShaderProgram vert, ShaderProgram frag, ShaderProgram tcs, ShaderProgram tes)
        {
            if (vert.ShaderKind == ShaderKind.VertexShader &&
                frag.ShaderKind == ShaderKind.FragmentShader &&
                tcs.ShaderKind == ShaderKind.TessControlShader &&
                tes.ShaderKind == ShaderKind.TessEvaluationShader)
            {
                ActiveShader.Add(vert);
                ActiveShader.Add(frag);
                ActiveShader.Add(tcs);
                ActiveShader.Add(tes);
                Program = ShaderBuilder.CreateShaderProgram(vert, frag, tcs, tes);
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
        /// 利用しているシェーダ
        /// </summary>
        private List<ShaderProgram> ActiveShader { get; set; } = new List<ShaderProgram>();

        /// <summary>
        /// デファインマクロの設定値
        /// </summary>
        private Dictionary<string,object> Define { get; set; }

        /// <summary>
        /// シェーダプログラムの取得
        /// </summary>
        /// <param name="type">シェーダ種類</param>
        /// <returns>シェーダプログラム</returns>
        public ShaderProgram GetShaderProgram(ShaderKind type)
        {
            return ActiveShader.FirstOrDefault(p => p.ShaderKind == type);
        }
        #endregion

        /// <summary>
        /// 同一シェーダの探索
        /// </summary>
        /// <param name="vert">頂点シェーダ</param>
        /// <param name="frag">フラグシェーダ</param>
        /// <param name="geom">ジオメトリシェーダ</param>
        /// <param name="tcs">テッセレーション制御シェーダ</param>
        /// <param name="tes">テッセレーション評価シェーダ</param>
        /// <returns>成功</returns>
        public bool FindShaderCombi(string vert, string frag, string geom, string tes, string tcs)
        {
            if (GetShaderProgram(ShaderKind.VertexShader).FilePath == vert &&
                GetShaderProgram(ShaderKind.FragmentShader).FilePath == frag &&
                GetShaderProgram(ShaderKind.GeometryShader).FilePath == geom &&
                GetShaderProgram(ShaderKind.TessEvaluationShader).FilePath == tes &&
                GetShaderProgram(ShaderKind.TessControlShader).FilePath == tcs)
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
        private void BindAttributeState(ShaderProgramInfo attribute)
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
                            Logger.Log(Logger.LogLevel.Warning, "Not Supported ArrayType");
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
        /// 値の取得
        /// </summary>
        /// <param name="name">変数名</param>
        /// <returns>変数</returns>
        public object GetValue(string name)
        {
            if (shaderVariable.ContainsKey(name))
            {
                return shaderVariable[name].Variable;
            }

            return null;
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
                if (value is TextureBuffer)
                {
                    var texture = value as TextureBuffer;
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
        public IEnumerable<ShaderProgramInfo> GetShaderVariable()
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
            CreateShaderProgramInfo();
        }

        /// <summary>
        /// オブジェクトを表す文字列
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            string name = string.Empty;
            foreach (var shader in ActiveShader)
            {
                name += shader.ToString();
            }

            return name;
        }

        #endregion

        #region [analyze shader code]

        /// <summary>
        /// シェーダプログラムの解析
        /// </summary>
        public void CreateShaderProgramInfo()
        {
            shaderVariable.Clear();
            foreach (ShaderProgram loop in ActiveShader)
            {
                AnalyzeShaderProgram(loop);
            }


            // index buffer
            ShaderProgramInfo info = new ShaderProgramInfo();
            info.Name = "index";
            info.Variable = new List<int>();
            info.ShaderVariableType = ShaderVariableType.Attribute;
            shaderVariable.Add(info.Name, info);

            SetShaderVariableID();
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            if (Program != -1)
            {
                GL.DeleteProgram(Program);
                Logger.GLLog(Logger.LogLevel.Error);
            }
        }

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
            else if (uniform.VariableType == VariableType.Vec4)
            {
                GL.Uniform4(uniform.ShaderID, (Vector4)uniform.Variable);
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
        /// Attrib の設定
        /// </summary>
        private void SetShaderVariableID()
        {
            GL.UseProgram(Program);
            foreach (ShaderProgramInfo loop in shaderVariable.Values)
            {
                switch (loop.ShaderVariableType)
                {
                    case ShaderVariableType.Attribute:
                        if (loop.Name == "index")
                        {
                            continue;
                        }
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
        /// UniformとAttributeの変数取得
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

                            if (!info.Name.StartsWith("u"))
                            {
                                Logger.Log(Logger.LogLevel.Error, "Uniform Name does not coding rule . name :" + info.Name + Environment.NewLine + shader.FilePath);
                            }

                            shaderVariable.Add(info.Name, info);
                        }

                        Logger.GLLog(Logger.LogLevel.Error);
                        break;
                }
            }

            Logger.GLLog(Logger.LogLevel.Error);
        }
        #endregion

    }
}
