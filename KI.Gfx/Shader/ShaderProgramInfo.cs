namespace KI.Gfx
{
    /// <summary>
    /// 変数の種類
    /// </summary>
    public enum ShaderVariableType
    {
        None,
        Uniform,
        Attribute,
    }

    /// <summary>
    /// 変数の型
    /// </summary>
    public enum VariableType
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
        DoubleArray,
        Texture2D,
        Texture3D,
        Cubemap
    }

    /// <summary>
    /// シェーダプログラム
    /// </summary>
    public class ShaderProgramInfo
    {
        /// <summary>
        /// 変数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public int ShaderID { get; set; }

        /// <summary>
        /// 値
        /// </summary>
        public object Variable { get; set; }

        /// <summary>
        /// 配列のときの要素数
        /// </summary>
        public int ArrayNum { get; set; }

        /// <summary>
        /// 種類
        /// </summary>
        public ShaderVariableType ShaderVariableType { get; set; }

        /// <summary>
        /// 型
        /// </summary>
        public VariableType VariableType { get; set; }
    }
}
