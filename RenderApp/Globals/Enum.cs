using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp
{
    public enum EVariableType
    {
        None,
        Uniform,
        Attribute,
    }

    public enum EAssetType
    {
        Unknown,
        Geometry,
        Light,
        Camera,
        Textures,
        EnvProbe,
        Materials,
        ShaderProgram
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
    public enum ERenderMode
    {
        None,
        Defferred,
        Forward
    }
    public enum TextureType
    {
        Texture2D,
        FrameBuffer,
        CubeMap
    }
    public enum TextureKind
    {
        None = -1,
        Albedo,
        Normal,
        Height,
        Emissive
    }
}
