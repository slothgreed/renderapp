using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;

namespace KI.Asset
{
    /// <summary>
    /// シェーダ種類
    /// </summary>
    public enum ShaderType
    {
        Bezier,
        Displacement,
        EffectLine,
        NURBS,
        Fur,
        Outline,
        WireFrame,
        Split,
        VectorField,
        SSLIC,
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
                return AssetDirectory.ShaderDirectory + @"\";
            }
        }

        /// <summary>
        /// 頂点シェーダの取得
        /// </summary>
        /// <param name="renderObject">描画オブジェクト</param>
        /// <returns>ファイルパス</returns>
        public string GetVertexShader(Polygon polygon)
        {
            if (polygon.Type == PolygonType.Lines ||
                polygon.Type == PolygonType.Points)
            {
                return Directory + @"GBuffer\GeneralPC.vert";
            }

            if (polygon.Textures.Count != 0)
            {
                return Directory + @"GBuffer\GeneralPNCT.vert";
            }
            else
            {
                return Directory + @"GBuffer\GeneralPNC.vert";
            }
        }

        public string GetVertexShader(GBufferType type)
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

        public string GetFragShader(GBufferType type)
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
        /// フラグシェーダの取得
        /// </summary>
        /// <param name="renderObject">描画オブジェクト</param>
        /// <returns>ファイルパス</returns>
        public string GetFragShader(Polygon polygon)
        {
            string shaderPath = GetTextureFragShader(polygon);
            if (shaderPath != null)
            {
                return shaderPath;
            }

            if (polygon.Type == PolygonType.Lines ||
                polygon.Type == PolygonType.Points)
            {
                return Directory + @"GBuffer\GeneralPC.frag";
            }

            if (polygon.Textures.Count != 0)
            {
                return Directory + @"GBuffer\GeneralPNT.frag";
            }
            else
            {
                return Directory + @"GBuffer\GeneralPNC.frag";
            }
        }

        /// <summary>
        /// テクスチャ付フラグメントシェーダの取得
        /// </summary>
        /// <param name="polygon">形状</param>
        /// <returns>ファイルパス</returns>
        private string GetTextureFragShader(Polygon polygon)
        {
            if (polygon.GetTexture(TextureKind.Albedo) != null &&
                polygon.GetTexture(TextureKind.Normal) != null &&
                polygon.GetTexture(TextureKind.Specular) != null)
            {
                return Directory + @"GBuffer\GeneralANS.frag";
            }

            if (polygon.GetTexture(TextureKind.Albedo) != null &&
                polygon.GetTexture(TextureKind.Normal) != null)
            {
                return Directory + @"GBuffer\GeneralAN.frag";
            }

            if (polygon.GetTexture(TextureKind.Albedo) != null &&
                polygon.GetTexture(TextureKind.Specular) != null)
            {
                return Directory + @"GBuffer\GeneralAS.frag";
            }

            if (polygon.GetTexture(TextureKind.Albedo) != null)
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
            var vertexShader = GetVertexShader(type);
            var fragShader = GetFragShader(type);

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
        public Shader CreateShader(ShaderType type)
        {
            switch (type)
            {
                case ShaderType.Bezier:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"Special\bezier.vert",
                        Directory + @"Special\bezier.frag");
                case ShaderType.Displacement:
                    return ShaderFactory.Instance.CreateTesselation(
                        Directory + @"Special\Tesselation\displacement.vert",
                        Directory + @"Special\Tesselation\displacement.frag",
                        Directory + @"Special\Tesselation\displacement.geom",
                        Directory + @"Special\Tesselation\displacement.tcs",
                        Directory + @"Special\Tesselation\displacement.tes");
                case ShaderType.EffectLine:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"Special\effectline.vert",
                        Directory + @"Special\effectline.frag");
                case ShaderType.NURBS:
                    return ShaderFactory.Instance.CreateTesselation(
                        Directory + @"Special\Tesselation\nurbs.vert",
                        Directory + @"Special\Tesselation\nurbs.frag",
                        Directory + @"Special\Tesselation\nurbs.geom",
                        Directory + @"Special\Tesselation\nurbs.tcs",
                        Directory + @"Special\Tesselation\nurbs.tes");
                case ShaderType.Split:
                    return ShaderFactory.Instance.CreateTesselation(
                        Directory + @"Special\Tesselation\split.vert",
                        Directory + @"Special\Tesselation\split.frag",
                        Directory + @"Special\Tesselation\split.geom",
                        Directory + @"Special\Tesselation\split.tcs",
                        Directory + @"Special\Tesselation\split.tes");
                case ShaderType.Fur:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"Special\fur.vert",
                        Directory + @"Special\fur.frag");
                case ShaderType.Outline:
                    return ShaderFactory.Instance.CreateGeometryShader(
                        Directory + @"Special\outline.vert",
                        Directory + @"Special\outline.frag",
                        Directory + @"Special\outline.geom");
                case ShaderType.WireFrame:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"GBuffer\WireFrame.vert",
                        Directory + @"GBuffer\WireFrame.frag");
                case ShaderType.VectorField:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"GBuffer\DirectionShader.vert",
                        Directory + @"GBuffer\DirectionShader.frag");
                case ShaderType.SSLIC:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"PostEffect\sslic.vert",
                        Directory + @"PostEffect\sslic.frag");
                case ShaderType.Output:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"PostEffect\output.vert",
                        Directory + @"PostEffect\output.frag");
            }

            return null;
        }
    }
}
