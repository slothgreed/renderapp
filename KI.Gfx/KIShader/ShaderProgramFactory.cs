using KI.Foundation.Core;

namespace KI.Gfx.KIShader
{
    class ShaderProgramFactory : KIFactoryBase<ShaderProgram>
    {
        public static ShaderProgramFactory Instance { get; } = new ShaderProgramFactory();

        public ShaderProgram CreateShaderProgram(string key, string path)
        {
            var program = FindByKey(key);

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
