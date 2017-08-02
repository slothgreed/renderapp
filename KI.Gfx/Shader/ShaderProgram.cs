using System.IO;
using KI.Foundation.Core;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx
{
    /// <summary>
    /// シェーダプログラム
    /// </summary>
    public class ShaderProgram : KIFile
    {
        private ShaderType? shaderType;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public ShaderProgram(string filePath)
            : base(filePath)
        {
            StreamReader reader = new StreamReader(FilePath);
            ShaderCode = reader.ReadToEnd();
            reader.Close();
        }

        public ShaderType ShaderType
        {
            get
            {
                if (shaderType == null)
                {
                    string extension = Path.GetExtension(FilePath);
                    switch (extension)
                    {
                        case ".vert":
                            shaderType = ShaderType.VertexShader;
                            break;
                        case ".frag":
                            shaderType = ShaderType.FragmentShader;
                            break;
                        case ".geom":
                            shaderType = ShaderType.GeometryShader;
                            break;
                        case ".tcs":
                            shaderType = ShaderType.TessControlShader;
                            break;
                        case ".tes":
                            shaderType = ShaderType.TessEvaluationShader;
                            break;
                    }
                }

                return (ShaderType)shaderType;
            }
        }

        /// <summary>
        /// シェーダコード
        /// </summary>
        public string ShaderCode { get; set; }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
        }
    }
}
