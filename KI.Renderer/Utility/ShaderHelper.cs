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
                        info.Variable = renderObject.PositionBuffer;
                        break;
                    case "normal":
                        info.Variable = renderObject.NormalBuffer;
                        break;
                    case "color":
                        info.Variable = renderObject.ColorBuffer;
                        break;
                    case "texcoord":
                        info.Variable = renderObject.TexCoordBuffer;
                        break;
                    case "index":
                        info.Variable = renderObject.IndexBuffer;
                        break;
                    case "uGeometryID":
                        info.Variable = renderObject.Geometry.ID;
                        break;
                    case "uWidth":
                        info.Variable = DeviceContext.Instance.Width;
                        break;
                    case "uHeight":
                        info.Variable = DeviceContext.Instance.Height;
                        break;
                    case "uMVP":
                        Matrix4 vp = scene.MainCamera.CameraProjMatrix;
                        info.Variable = renderObject.Geometry.ModelMatrix * vp;
                        break;
                    case "uSMVP":
                        Matrix4 light = scene.SunLight.Matrix;
                        Matrix4 proj = scene.MainCamera.ProjMatrix;
                        info.Variable = renderObject.Geometry.ModelMatrix * light * proj;
                        break;
                    case "uModelMatrix":
                        info.Variable = renderObject.Geometry.ModelMatrix;
                        break;
                    case "uNormalMatrix":
                        info.Variable = renderObject.Geometry.NormalMatrix;
                        break;
                    case "uProjectMatrix":
                        info.Variable = scene.MainCamera.ProjMatrix;
                        break;
                    case "uUnProjectMatrix":
                        info.Variable = scene.MainCamera.UnProject;
                        break;
                    case "uCameraPosition":
                        info.Variable = scene.MainCamera.Position;
                        break;
                    case "uCameraMatrix":
                        info.Variable = scene.MainCamera.Matrix;
                        break;
                    case "uLightPosition":
                        info.Variable = scene.SunLight.Position;
                        break;
                    case "uLightDirection":
                        info.Variable = scene.SunLight.Direction;
                        break;
                    case "uLightMatrix":
                        info.Variable = scene.SunLight.Matrix;
                        break;
                    case "uAlbedoMap":
                        if (textures.ContainsKey(TextureKind.Albedo))
                        {
                            info.Variable = textures[TextureKind.Albedo].DeviceID;
                        }

                        break;
                    case "uCubeMap":
                        if (textures.ContainsKey(TextureKind.Cubemap))
                        {
                            info.Variable = textures[TextureKind.Cubemap].DeviceID;
                        }

                        break;
                    case "uSpecularMap":
                        if (textures.ContainsKey(TextureKind.Specular))
                        {
                            info.Variable = textures[TextureKind.Specular].DeviceID;
                        }

                        break;
                    case "uWorldMap":
                        if (textures.ContainsKey(TextureKind.World))
                        {
                            info.Variable = textures[TextureKind.World].DeviceID;
                        }

                        break;
                    case "uLightingMap":
                        if (textures.ContainsKey(TextureKind.Lighting))
                        {
                            info.Variable = textures[TextureKind.Lighting].DeviceID;
                        }

                        break;
                    case "uNormalMap":
                        if (textures.ContainsKey(TextureKind.Normal))
                        {
                            info.Variable = textures[TextureKind.Normal].DeviceID;
                        }

                        break;
                    case "uHeightMap":
                        if (textures.ContainsKey(TextureKind.Height))
                        {
                            info.Variable = textures[TextureKind.Height].DeviceID;
                        }

                        break;
                    case "uEmissiveMap":
                        if (textures.ContainsKey(TextureKind.Emissive))
                        {
                            info.Variable = textures[TextureKind.Emissive].DeviceID;
                        }

                        break;
                }
            }
        }
    }
}
