using System.Collections.Generic;
using OpenTK;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;
using KI.Asset;

namespace KI.Renderer
{
    public static class ShaderHelper
    {
        /// <summary>
        /// 初期状態の設定
        /// </summary>
        public static void InitializeState(Scene scene,Shader shader, RenderObject geometry, Dictionary<TextureKind, Texture> TextureItem)
        {
            foreach (ShaderProgramInfo info in shader.GetShaderVariable())
            {
                switch (info.Name)
                {
                    case "position":
                        info.variable = geometry.PositionBuffer;
                        break;
                    case "normal":
                        info.variable = geometry.NormalBuffer;
                        break;
                    case "color":
                        info.variable = geometry.ColorBuffer;
                        break;
                    case "texcoord":
                        info.variable = geometry.TexCoordBuffer;
                        break;
                    case "index":
                        info.variable = geometry.IndexBuffer;
                        break;
                    case "uGeometryID":
                        info.variable = geometry.ID;
                        break;
                    case "uWidth":
                        info.variable = DeviceContext.Instance.Width;
                        break;
                    case "uHeight":
                        info.variable = DeviceContext.Instance.Height;
                        break;
                    case "uMVP":
                        Matrix4 vp = scene.MainCamera.CameraProjMatrix;
                        info.variable = geometry.ModelMatrix * vp;
                        break;
                    case "uSMVP":
                        Matrix4 light = scene.SunLight.Matrix;
                        Matrix4 proj = scene.MainCamera.ProjMatrix;
                        info.variable = geometry.ModelMatrix * light * proj;
                        break;
                    case "uModelMatrix":
                        info.variable = geometry.ModelMatrix;
                        break;
                    case "uNormalMatrix":
                        info.variable = geometry.NormalMatrix;
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
                        info.variable =scene.MainCamera.Matrix;
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
                        if (TextureItem.ContainsKey(TextureKind.Albedo))
                        {
                            info.variable = TextureItem[TextureKind.Albedo].DeviceID;
                        }
                        break;
                    case "uCubeMap":
                        if (TextureItem.ContainsKey(TextureKind.Cubemap))
                        {
                            info.variable = TextureItem[TextureKind.Cubemap].DeviceID;
                        }
                        break;
                    case "uSpecularMap":
                        if (TextureItem.ContainsKey(TextureKind.Specular))
                        {
                            info.variable = TextureItem[TextureKind.Specular].DeviceID;
                        }
                        break;
                    case "uWorldMap":
                        if (TextureItem.ContainsKey(TextureKind.World))
                        {
                            info.variable = TextureItem[TextureKind.World].DeviceID;
                        }
                        break;
                    case "uLightingMap":
                        if (TextureItem.ContainsKey(TextureKind.Lighting))
                        {
                            info.variable = TextureItem[TextureKind.Lighting].DeviceID;
                        }
                        break;
                    case "uNormalMap":
                        if (TextureItem.ContainsKey(TextureKind.Normal))
                        {
                            info.variable = TextureItem[TextureKind.Normal].DeviceID;
                        }
                        break;
                    case "uHeightMap":
                        if (TextureItem.ContainsKey(TextureKind.Height))
                        {
                            info.variable = TextureItem[TextureKind.Height].DeviceID;
                        }
                        break;
                    case "uEmissiveMap":
                        if (TextureItem.ContainsKey(TextureKind.Emissive))
                        {
                            info.variable = TextureItem[TextureKind.Emissive].DeviceID;
                        }
                        break;
                }
            }
        }
    }
}
