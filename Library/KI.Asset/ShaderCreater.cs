using KI.Gfx;
using KI.Gfx.KIShader;

namespace KI.Asset
{
    /// <summary>
    /// シェーダ種類
    /// </summary>
    public enum SHADER_TYPE
    {
        Bezier,
        Displacement,
        EffectLine,
        NURBS,
        Fur,
        Outline,
        SingleColor,
        Split,
        VectorField,
        SSLIC,
        HUD,
        Output
    }

    public enum GBufferType
    {
        PointColor,
        PointNormalColor,
        PointNormalTexcoord,
        Albedo,
        AlbedoNormal,
        AlbedoSpecular,
        AlbedoNormalSpecular
    }

    /// <summary>
    /// シェーダ生成クラス
    /// </summary>
    public class ShaderCreater
    {
        public static ShaderCreater Instance = new ShaderCreater();
        /// <summary>
        /// シェーダディレクトリ
        /// </summary>
        public string Directory
        {
            get
            {
                return AssetConstants.ShaderDirectory + @"\";
            }
        }

        /// <summary>
        /// 頂点シェーダの取得
        /// </summary>
        /// <param name="polygonNode">描画オブジェクト</param>
        /// <returns>ファイルパス</returns>
        public string GetVertexShader(KIPrimitiveType primitiveType, Material material)
        {
            if (primitiveType == KIPrimitiveType.Lines ||
                primitiveType == KIPrimitiveType.Points)
            {
                return Directory + @"GBuffer\GeneralPC.vert";
            }

            if (material != null &&
                material.Textures.Count != 0)
            {
                return Directory + @"GBuffer\GeneralPNCT.vert";
            }
            else
            {
                return Directory + @"GBuffer\GeneralPNC.vert";
            }
        }

        /// <summary>
        /// フラグシェーダの取得
        /// </summary>
        /// <param name="polygonNode">描画オブジェクト</param>
        /// <returns>ファイルパス</returns>
        public string GetFragShader(KIPrimitiveType primitiveType, Material material)
        {
            string shaderPath = GetTextureFragShaderFilePath(material);
            if (shaderPath != null)
            {
                return shaderPath;
            }

            if (primitiveType == KIPrimitiveType.Lines ||
                primitiveType == KIPrimitiveType.Points)
            {
                return Directory + @"GBuffer\GeneralPC.frag";
            }

            if (material != null &&
                material.Textures.Count != 0)
            {
                return Directory + @"GBuffer\GeneralPNT.frag";
            }
            else
            {
                return Directory + @"GBuffer\GeneralPNC.frag";
            }
        }

        /// <summary>
        /// 頂点ファイルパスの取得
        /// </summary>
        /// <param name="type">GBufferに転送する頂点タイプ</param>
        /// <returns>ファイルパス</returns>
        public string GetVertexShaderFilePath(GBufferType type)
        {
            switch (type)
            {
                case GBufferType.PointColor:
                    return Directory + @"GBuffer\GeneralPC.vert";
                case GBufferType.PointNormalColor:
                    return Directory + @"GBuffer\GeneralPNC.vert";
                case GBufferType.PointNormalTexcoord:
                case GBufferType.Albedo:
                case GBufferType.AlbedoNormal:
                case GBufferType.AlbedoNormalSpecular:
                case GBufferType.AlbedoSpecular:
                    return Directory + @"GBuffer\GeneralPNT.vert";
            }

            return null;
        }

        /// <summary>
        /// フラグメントファイルパスの取得
        /// </summary>
        /// <param name="type">GBufferに転送する頂点タイプ</param>
        /// <returns>ファイルパス</returns>
        public string GetFragShaderFilePath(GBufferType type)
        {
            switch (type)
            {
                case GBufferType.PointColor:
                    return Directory + @"GBuffer\GeneralPC.frag";
                case GBufferType.PointNormalColor:
                    return Directory + @"GBuffer\GeneralPNC.frag";
                case GBufferType.PointNormalTexcoord:
                    return Directory + @"GBuffer\GeneralPNT.frag";
                case GBufferType.Albedo:
                    return Directory + @"GBuffer\GeneralA.frag";
                case GBufferType.AlbedoNormal:
                    return Directory + @"GBuffer\GeneralAN.frag";
                case GBufferType.AlbedoSpecular:
                    return Directory + @"GBuffer\GeneralAS.frag";
                case GBufferType.AlbedoNormalSpecular:
                    return Directory + @"GBuffer\GeneralANS.frag";
                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// テクスチャ付フラグメントシェーダの取得
        /// </summary>
        /// <param name="polygon">形状</param>
        /// <returns>ファイルパス</returns>
        private string GetTextureFragShaderFilePath(Material material)
        {
            if (material == null)
            {
                return null;
            }

            if (material.GetTexture(TextureKind.Albedo) != null &&
                material.GetTexture(TextureKind.Normal) != null &&
                material.GetTexture(TextureKind.Specular) != null)
            {
                return Directory + @"GBuffer\GeneralANS.frag";
            }

            if (material.GetTexture(TextureKind.Albedo) != null &&
                material.GetTexture(TextureKind.Normal) != null)
            {
                return Directory + @"GBuffer\GeneralAN.frag";
            }

            if (material.GetTexture(TextureKind.Albedo) != null &&
                material.GetTexture(TextureKind.Specular) != null)
            {
                return Directory + @"GBuffer\GeneralAS.frag";
            }

            if (material.GetTexture(TextureKind.Albedo) != null)
            {
                return Directory + @"GBuffer\GeneralA.frag";
            }

            return null;
        }

        /// <summary>
        /// 指定シェーダの作成
        /// </summary>
        /// <param name="type">シェーダ種類</param>
        /// <returns>シェーダ</returns>
        public Shader CreateShader(GBufferType type)
        {
            var vertexShader = GetVertexShaderFilePath(type);
            var fragShader = GetFragShaderFilePath(type);

            if (vertexShader != null &&
                fragShader != null)
            {
                return ShaderFactory.Instance.CreateShaderVF(
                    vertexShader,
                    fragShader);
            }

            return null;
        }

        /// <summary>
        /// 特殊シェーダの作成
        /// </summary>
        /// <param name="type">シェーダ種類</param>
        /// <returns>シェーダ</returns>
        public Shader CreateShader(SHADER_TYPE type)
        {
            switch (type)
            {
                case SHADER_TYPE.Bezier:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"Special\bezier.vert",
                        Directory + @"Special\bezier.frag");
                case SHADER_TYPE.Displacement:
                    return ShaderFactory.Instance.CreateTesselation(
                        Directory + @"Special\Tesselation\displacement.vert",
                        Directory + @"Special\Tesselation\displacement.frag",
                        Directory + @"Special\Tesselation\displacement.geom",
                        Directory + @"Special\Tesselation\displacement.tcs",
                        Directory + @"Special\Tesselation\displacement.tes");
                case SHADER_TYPE.EffectLine:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"Special\effectline.vert",
                        Directory + @"Special\effectline.frag");
                case SHADER_TYPE.NURBS:
                    return ShaderFactory.Instance.CreateTesselation(
                        Directory + @"Special\Tesselation\nurbs.vert",
                        Directory + @"Special\Tesselation\nurbs.frag",
                        Directory + @"Special\Tesselation\nurbs.geom",
                        Directory + @"Special\Tesselation\nurbs.tcs",
                        Directory + @"Special\Tesselation\nurbs.tes");
                case SHADER_TYPE.Split:
                    return ShaderFactory.Instance.CreateTesselation(
                        Directory + @"Special\Tesselation\split.vert",
                        Directory + @"Special\Tesselation\split.frag",
                        Directory + @"Special\Tesselation\split.geom",
                        Directory + @"Special\Tesselation\split.tcs",
                        Directory + @"Special\Tesselation\split.tes");
                case SHADER_TYPE.Fur:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"Special\fur.vert",
                        Directory + @"Special\fur.frag");
                case SHADER_TYPE.Outline:
                    return ShaderFactory.Instance.CreateGeometryShader(
                        Directory + @"Special\outline.vert",
                        Directory + @"Special\outline.frag",
                        Directory + @"Special\outline.geom");
                case SHADER_TYPE.SingleColor:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"GBuffer\SingleColor.vert",
                        Directory + @"GBuffer\SingleColor.frag");
                case SHADER_TYPE.VectorField:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"GBuffer\DirectionShader.vert",
                        Directory + @"GBuffer\DirectionShader.frag");
                case SHADER_TYPE.SSLIC:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"PostEffect\sslic.vert",
                        Directory + @"PostEffect\sslic.frag");
                case SHADER_TYPE.HUD:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"hud.vert",
                        Directory + @"hud.frag");
                case SHADER_TYPE.Output:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"PostEffect\output.vert",
                        Directory + @"PostEffect\output.frag");
            }

            return null;
        }
    }
}
