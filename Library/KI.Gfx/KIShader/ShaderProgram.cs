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
        /// <summary>
        /// シェーダ種類
        /// </summary>
        private ShaderKind shaderKind;
        
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

            SetShaderType(FilePath);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="code">シェーダコード</param>
        /// <param name="type">種類</param>
        public ShaderProgram(string code, ShaderType type)
            : base(type.ToString())
        {

        }

        public void SetShaderType(string filePath)
        {
            string extension = Path.GetExtension(FilePath);
            switch (extension)
            {
                case ".vert":
                    shaderKind = ShaderKind.VertexShader;
                    break;
                case ".frag":
                    shaderKind = ShaderKind.FragmentShader;
                    break;
                case ".geom":
                    shaderKind = ShaderKind.GeometryShader;
                    break;
                case ".tcs":
                    shaderKind = ShaderKind.TessControlShader;
                    break;
                case ".tes":
                    shaderKind = ShaderKind.TessEvaluationShader;
                    break;
            }
        }



        /// <summary>
        /// シェーダ種類
        /// </summary>
        public ShaderKind ShaderKind
        {
            get
            {
                return shaderKind;
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
