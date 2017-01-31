using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK.Graphics.OpenGL;
using RenderApp.AssetModel;
using KI.Foundation.Core;
using OpenTK;
namespace RenderApp.GLUtil.ShaderModel
{
    public class ShaderProgram : KIFile
    {
       
        private ShaderType? _shaderType;
        public ShaderType shaderType
        {
            get
            {
                if(_shaderType == null)
                {
                    string extension = Path.GetExtension(FilePath);
                    switch(extension)
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
        private string _shaderCode;
        public string ShaderCode
        {
            get
            {
                return _shaderCode;
            }
            set
            {
                _shaderCode = value;
            }
        }
        public ShaderProgram(string name,string filePath)
            : base(filePath)
        {
            StreamReader reader = new StreamReader(FilePath);
            _shaderCode = reader.ReadToEnd();
            reader.Close();
        }
        public override void Dispose()
        {

        }

      
    }
}
