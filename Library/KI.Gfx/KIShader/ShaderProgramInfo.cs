namespace KI.Gfx
{
    /// <summary>
    /// 変数の種類
    /// </summary>
    public enum ShaderValueType
    {
        None,
        Uniform,
        Attribute,
    }

    /// <summary>
    /// 変数の型
    /// </summary>
    public enum ValueType
    {
        None,
        Bool,
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
    /// シェーダ定義情報
    /// </summary>
    public class ShaderDefineInfo
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 変数の値
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 型
        /// </summary>
        public ValueType ValueType { get; set; }

        /// <summary>
        /// 有効かどうか
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// シェーダ種類
        /// </summary>
        public ShaderKind ShaderKind { get; set; }
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
        public object Value { get; set; }

        /// <summary>
        /// 配列のときの要素数
        /// </summary>
        public int ArrayNum { get; set; }

        /// <summary>
        /// 種類
        /// </summary>
        public ShaderValueType ShaderVariableType { get; set; }

        /// <summary>
        /// 型
        /// </summary>
        public ValueType VauleType { get; set; }

        /// <summary>
        /// でファインマクロ用のヘッダ
        /// </summary>
        public string Header { get; set; }
    }
}
