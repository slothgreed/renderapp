using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KI.Gfx
{
    public enum EShaderVariableType
    {
        None,
        Uniform,
        Attribute,
    }
    public enum EVariableType
    {
        None,
        Vec2,
        Vec3,
        Vec4,
        Int,
        Float,
        Double,
        Mat3,
        Mat4,
        Vec2Array,
        Vec3Array,
        Vec4Array,
        IntArray,
        FloatArray,
        DoubleArra,
        Texture2D,
        Texture3D
    }

    /// <summary>
    /// シェーダ情報
    /// </summary>
    public class ShaderProgramInfo
    {
        public string Name { get; set; }
        public int ShaderID { get; set; }
        public int VertexBufferId { get; set; }
        public object variable { get; set; }
        public EShaderVariableType shaderVariableType { get; set; }
        public EVariableType variableType { get; set; }
    }
}
