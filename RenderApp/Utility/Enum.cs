using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RenderApp
{
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
}
