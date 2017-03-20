using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;
namespace KI.Gfx.KIShader
{
    class ShaderProgramFactory : KIFactoryBase<ShaderProgram>
    {
        private static ShaderProgramFactory _instance = new ShaderProgramFactory();
        public static ShaderProgramFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public ShaderProgram CreateShaderProgram(string key, string path)
        {
            var program = FindByKey(key);

            if (program != null)
            {
                return program;
            }
            else
            {
                program = new ShaderProgram(key, path);
                AddItem(program);
                return program;
            }
        }
    }
}
