namespace RenderApp.Model
{
    public enum EAssetType
    {
        Unknown,
        Geometry,
        Light,
        Camera,
        Textures,
        EnvProbe,
        Materials,
    }

    /// <summary>
    /// Attribute(Shader用)
    /// </summary>
    public enum EAttrib
    {
        Vertex = 0,
        Normal = 1,
        Color = 2,
        Texture = 3,
        Index = 4,
        Timer = 5,
        Num = 6,
    }

    /// <summary>
    /// レンダリングステージ
    /// </summary>
    public enum ERenderingStage
    {
        GeometryPass,
        LightingPass,
        PostProcessPass,
        ForwardPass
    }

    public enum TextureType
    {
        Texture2D,
        FrameBuffer,
        CubeMap
    }

    public enum RAAsset
    {
        Model,
        Texture,
        ShaderProgram,
        Shader,
    }

    public enum AppWindow
    {
        Project,
        Scene,
        RenderSystem,
        Viewport
    }
}
