﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp
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
    public enum RAGeometry
    {
        Cube,
        Sphere,
        Plane,
        WireFrame,
        Polygon,
        STL,
        OBJ,
        File
    }
    public enum RAAsset
    {
        Model,
        Texture,
        Material,
        ShaderProgram,
        Shader,
    }
    public enum TextureKind
    {
        None = -1,
        Albedo,
        Normal,
        Specular,
        Height,
        Emissive,
        World,
        Lighting
    }
    public enum RAController
    {
        Default,
        Dijkstra
    }

}
