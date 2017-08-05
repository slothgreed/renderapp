using System.Collections.Generic;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;
using OpenTK;

namespace KI.Renderer
{
    /// <summary>
    /// シェーダヘルパ
    /// </summary>
    public static class ShaderHelper
    {
        /// <summary>
        /// 初期状態の設定
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="shader">シェーダ</param>
        /// <param name="renderObject">形状</param>
        /// <param name="textures">テクスチャ</param>
        public static void InitializeState(Scene scene, Shader shader, RenderObject renderObject, Dictionary<TextureKind, Texture> textures)
        {
            foreach (ShaderProgramInfo info in shader.GetShaderVariable())
            {
                switch (info.Name)
                {
                    case "position":
                        info.variable = renderObject.PositionBuffer;
                        break;
                    case "normal":
                        info.variable = renderObject.NormalBuffer;
                        break;
                    case "color":
                        info.variable = renderObject.ColorBuffer;
                        break;
                    case "texcoord":
                        info.variable = renderObject.TexCoordBuffer;
                        break;
                    case "index":
                        info.variable = renderObject.IndexBuffer;
                        break;
                    case "uGeometryID":
                        info.variable = renderObject.Geometry.ID;
                        break;
                    case "uWidth":
                        info.variable = DeviceContext.Instance.Width;
                        break;
                    case "uHeight":
                        info.variable = DeviceContext.Instance.Height;
                        break;
                    case "uMVP":
                        Matrix4 vp = scene.MainCamera.CameraProjMatrix;
                        info.variable = renderObject.Geometry.ModelMatrix * vp;
                        break;
                    case "uSMVP":
                        Matrix4 light = scene.SunLight.Matrix;
                        Matrix4 proj = scene.MainCamera.ProjMatrix;
                        info.variable = renderObject.Geometry.ModelMatrix * light * proj;
                        break;
                    case "uModelMatrix":
                        info.variable = renderObject.Geometry.ModelMatrix;
                        break;
                    case "uNormalMatrix":
                        info.variable = renderObject.Geometry.NormalMatrix;
                        break;
                    case "uProjectMatrix":
                        info.variable = scene.MainCamera.ProjMatrix;
                        break;
                    case "uUnProjectMatrix":
                        info.variable = scene.MainCamera.UnProject;
                        break;
                    case "uCameraPosition":
                        info.variable = scene.MainCamera.Position;
                        break;
                    case "uCameraMatrix":
                        info.variable = scene.MainCamera.Matrix;
                        break;
                    case "uLightPosition":
                        info.variable = scene.SunLight.Position;
                        break;
                    case "uLightDirection":
                        info.variable = scene.SunLight.Direction;
                        break;
                    case "uLightMatrix":
                        info.variable = scene.SunLight.Matrix;
                        break;
                    case "uAlbedoMap":
                        if (textures.ContainsKey(TextureKind.Albedo))
                        {
                            info.variable = textures[TextureKind.Albedo].DeviceID;
                        }

                        break;
                    case "uCubeMap":
                        if (textures.ContainsKey(TextureKind.Cubemap))
                        {
                            info.variable = textures[TextureKind.Cubemap].DeviceID;
                        }

                        break;
                    case "uSpecularMap":
                        if (textures.ContainsKey(TextureKind.Specular))
                        {
                            info.variable = textures[TextureKind.Specular].DeviceID;
                        }

                        break;
                    case "uWorldMap":
                        if (textures.ContainsKey(TextureKind.World))
                        {
                            info.variable = textures[TextureKind.World].DeviceID;
                        }

                        break;
                    case "uLightingMap":
                        if (textures.ContainsKey(TextureKind.Lighting))
                        {
                            info.variable = textures[TextureKind.Lighting].DeviceID;
                        }

                        break;
                    case "uNormalMap":
                        if (textures.ContainsKey(TextureKind.Normal))
                        {
                            info.variable = textures[TextureKind.Normal].DeviceID;
                        }

                        break;
                    case "uHeightMap":
                        if (textures.ContainsKey(TextureKind.Height))
                        {
                            info.variable = textures[TextureKind.Height].DeviceID;
                        }

                        break;
                    case "uEmissiveMap":
                        if (textures.ContainsKey(TextureKind.Emissive))
                        {
                            info.variable = textures[TextureKind.Emissive].DeviceID;
                        }

                        break;
                }
            }
        }
    }
}
