using System.IO;
using OpenTK.Graphics.OpenGL;
using KI.Foundation.Core;
namespace KI.Gfx
{
    /// <summary>
    /// シェーダプログラム
    /// </summary>
    public class ShaderProgram : KIFile
    {
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

        private ShaderType? _shaderType;
        public ShaderType shaderType
        {
            get
            {
                if (_shaderType == null)
                {
                    string extension = Path.GetExtension(FilePath);
                    switch (extension)
                    {
                        case ".vert":
                            _shaderType = ShaderType.VertexShader;
                            break;
                        case ".frag":
                            _shaderType = ShaderType.FragmentShader;
                            break;
                        case ".geom":
                            _shaderType = ShaderType.GeometryShader;
                            break;
                        case ".tcs":
                            _shaderType = ShaderType.TessControlShader;
                            break;
                        case ".tes":
                            _shaderType = ShaderType.TessEvaluationShader;
                            break;
                    }
                }
                return (ShaderType)_shaderType;
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
