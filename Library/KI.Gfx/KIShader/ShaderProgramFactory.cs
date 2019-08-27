using KI.Foundation.Core;

namespace KI.Gfx.KIShader
{
    /// <summary>
    /// シェーダプログラムファクトリ
    /// </summary>
    public class ShaderProgramFactory : KIFactoryBase<ShaderProgram>
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        public static ShaderProgramFactory Instance { get; } = new ShaderProgramFactory();

        /// <summary>
        /// シェーダの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="path">パス</param>
        /// <returns>シェーダプログラム</returns>
        public ShaderProgram CreateShaderProgram(string name, string path)
        {
            var program = FindByName(name);

            if (program != null)
            {
                return program;
            }
            else
            {
                program = new ShaderProgram(path);
                AddItem(program);
                return program;
            }
        }
    }
}
