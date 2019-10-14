using KI.Gfx;
using KI.Gfx.Buffer;
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
        public static void InitializeState(Scene scene, SceneNode sceneNode, VertexBuffer vertexBuffer, Material material)
        {
            foreach (ShaderProgramInfo info in material.Shader.GetShaderVariable())
            {
                switch (info.Name)
                {
                    case "position":
                        info.Value = vertexBuffer.PositionBuffer;
                        break;
                    case "normal":
                        info.Value = vertexBuffer.NormalBuffer;
                        break;
                    case "color":
                        info.Value = vertexBuffer.ColorBuffer;
                        break;
                    case "texcoord":
                        info.Value = vertexBuffer.TexCoordBuffer;
                        break;
                    case "index":
                        if (vertexBuffer.EnableIndexBuffer)
                        {
                            info.Value = vertexBuffer.IndexBuffer;
                        }
                        break;
                    case "uGeometryID":
                        info.Value = sceneNode.ID;
                        break;
                    case "uWidth":
                        info.Value = DeviceContext.Instance.Width;
                        break;
                    case "uHeight":
                        info.Value = DeviceContext.Instance.Height;
                        break;
                    case "uMVP":
                        Matrix4 vp = scene.MainCamera.CameraProjMatrix;
                        info.Value = sceneNode.ModelMatrix * vp;
                        break;
                    case "uSMVP":
                        Matrix4 light = scene.MainLight.Matrix;
                        Matrix4 proj = scene.MainCamera.ProjMatrix;
                        info.Value = sceneNode.ModelMatrix * light * proj;
                        break;
                    case "uModelMatrix":
                        info.Value = sceneNode.ModelMatrix;
                        break;
                    case "uNormalMatrix":
                        info.Value = sceneNode.NormalMatrix;
                        break;
                    case "uProjectMatrix":
                        info.Value = scene.MainCamera.ProjMatrix;
                        break;
                    case "uUnProjectMatrix":
                        info.Value = scene.MainCamera.UnProject;
                        break;
                    case "uCameraPosition":
                        info.Value = scene.MainCamera.Position;
                        break;
                    case "uCameraMatrix":
                        info.Value = scene.MainCamera.Matrix;
                        break;
                    case "uLightPosition":
                        info.Value = scene.MainLight.Position;
                        break;
                    case "uLightDirection":
                        info.Value = scene.MainLight.Direction;
                        break;
                    case "uLightMatrix":
                        info.Value = scene.MainLight.Matrix;
                        break;
                    case "uAlbedoMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Albedo))
                        {
                            info.Value = material.Textures[TextureKind.Albedo].DeviceID;
                        }

                        break;
                    case "uCubeMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Cubemap))
                        {
                            info.Value = material.Textures[TextureKind.Cubemap].DeviceID;
                        }

                        break;
                    case "uSpecularMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Specular))
                        {
                            info.Value = material.Textures[TextureKind.Specular].DeviceID;
                        }

                        break;
                    case "uWorldMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.World))
                        {
                            info.Value = material.Textures[TextureKind.World].DeviceID;
                        }

                        break;
                    case "uLightingMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Lighting))
                        {
                            info.Value = material.Textures[TextureKind.Lighting].DeviceID;
                        }

                        break;
                    case "uNormalMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Normal))
                        {
                            info.Value = material.Textures[TextureKind.Normal].DeviceID;
                        }

                        break;
                    case "uHeightMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Height))
                        {
                            info.Value = material.Textures[TextureKind.Height].DeviceID;
                        }

                        break;
                    case "uEmissiveMap":
                        if (material.Textures != null &&
                            material.Textures.ContainsKey(TextureKind.Emissive))
                        {
                            info.Value = material.Textures[TextureKind.Emissive].DeviceID;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// 初期状態の設定
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="sceneNode">形状</param>
        /// <param name="vertexBuffer">頂点バッファ</param>
        /// <param name="Material">マテリアル</param>
        public static void InitializeHUD(VertexBuffer vertexBuffer, Material material)
        {
            foreach (ShaderProgramInfo info in material.Shader.GetShaderVariable())
            {
                switch (info.Name)
                {
                    case "position":
                        info.Value = vertexBuffer.PositionBuffer;
                        break;
                    case "normal":
                        info.Value = vertexBuffer.NormalBuffer;
                        break;
                    case "color":
                        info.Value = vertexBuffer.ColorBuffer;
                        break;
                    case "texcoord":
                        info.Value = vertexBuffer.TexCoordBuffer;
                        break;
                    case "index":
                        if (vertexBuffer.EnableIndexBuffer)
                        {
                            info.Value = vertexBuffer.IndexBuffer;
                        }
                        break;
                }
            }
        }
    }
}
