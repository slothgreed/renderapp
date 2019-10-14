namespace KI.Gfx
{
    public enum KIPrimitiveType
    {
        Points,
        Lines,
        LineLoop,
        Triangles,
        Quads,
        Patches
    }

    /// <summary>
    /// テクスチャ種類
    /// </summary>
    public enum TextureKind
    {
        None = -1,
        Albedo,
        Normal,
        Specular,
        Height,
        Emissive,
        World,
        Lighting,
        Cubemap
    }

    public enum ShaderKind
    {
        VertexShader,
        FragmentShader,
        GeometryShader,
        TessEvaluationShader,
        TessControlShader
    }

}
