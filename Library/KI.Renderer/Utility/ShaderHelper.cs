using System.Collections.Generic;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
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
        /// <param name="sceneNode">形状</param>
        /// <param name="vertexBuffer">頂点バッファ</param>
        /// <param name="Material">マテリアル</param>
        public static void InitializeState(Scene scene, SceneNode sceneNode, VertexBuffer vertexBuffer,  Material material)
        {
            foreach (ShaderProgramInfo info in material.Shader.GetShaderVariable())
            {
                switch (info.Name)
                {
                    case "position":
                        info.Variable = vertexBuffer.PositionBuffer;
                        break;
                    case "normal":
                        info.Variable = vertexBuffer.NormalBuffer;
                        break;
                    case "color":
                        info.Variable = vertexBuffer.ColorBuffer;
                        break;
                    case "texcoord":
                        info.Variable = vertexBuffer.TexCoordBuffer;
                        break;
                    case "index":
                        if (vertexBuffer.EnableIndexBuffer)
                        {
                            info.Variable = vertexBuffer.IndexBuffer;
                        }
                        break;
                    case "uGeometryID":
                        info.Variable = sceneNode.ID;
                        break;
                    case "uWidth":
                        info.Variable = DeviceContext.Instance.Width;
                        break;
                    case "uHeight":
                        info.Variable = DeviceContext.Instance.Height;
                        break;
                    case "uMVP":
                        Matrix4 vp = scene.MainCamera.CameraProjMatrix;
                        info.Variable = sceneNode.ModelMatrix * vp;
                        break;
                    case "uSMVP":
                        Matrix4 light = scene.MainLight.Matrix;
                        Matrix4 proj = scene.MainCamera.ProjMatrix;
                        info.Variable = sceneNode.ModelMatrix * light * proj;
                        break;
                    case "uModelMatrix":
                        info.Variable = sceneNode.ModelMatrix;
                        break;
                    case "uNormalMatrix":
                        info.Variable = sceneNode.NormalMatrix;
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
                        info.Variable = scene.MainLight.Position;
                        break;
                    case "uLightDirection":
                        info.Variable = scene.MainLight.Direction;
                        break;
                    case "uLightMatrix":
                        info.Variable = scene.MainLight.Matrix;
                        break;
                    case "uAlbedoMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Albedo))
                        {
                            info.Variable = material.Textures[TextureKind.Albedo].DeviceID;
                        }

                        break;
                    case "uCubeMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Cubemap))
                        {
                            info.Variable = material.Textures[TextureKind.Cubemap].DeviceID;
                        }

                        break;
                    case "uSpecularMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Specular))
                        {
                            info.Variable = material.Textures[TextureKind.Specular].DeviceID;
                        }

                        break;
                    case "uWorldMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.World))
                        {
                            info.Variable = material.Textures[TextureKind.World].DeviceID;
                        }

                        break;
                    case "uLightingMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Lighting))
                        {
                            info.Variable = material.Textures[TextureKind.Lighting].DeviceID;
                        }

                        break;
                    case "uNormalMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Normal))
                        {
                            info.Variable = material.Textures[TextureKind.Normal].DeviceID;
                        }

                        break;
                    case "uHeightMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Height))
                        {
                            info.Variable = material.Textures[TextureKind.Height].DeviceID;
                        }

                        break;
                    case "uEmissiveMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Emissive))
                        {
                            info.Variable = material.Textures[TextureKind.Emissive].DeviceID;
                        }

                        break;
                }
            }
        }
    }
}
