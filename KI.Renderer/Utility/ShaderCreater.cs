using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;
using KI.Renderer;
using OpenTK.Graphics.OpenGL;

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
        WireFrame
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
                return Global.ShaderDirectory + @"\";
            }
        }

        /// <summary>
        /// 頂点シェーダの取得
        /// </summary>
        /// <param name="renderObject">描画オブジェクト</param>
        /// <returns>ファイルパス</returns>
        public string GetVertexShader(Polygon polygon)
        {
            if (polygon.Type == PrimitiveType.Lines ||
                polygon.Type == PrimitiveType.Points)
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

            if (polygon.Type == PrimitiveType.Lines ||
                polygon.Type == PrimitiveType.Points)
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
        public string GetTextureFragShader(Polygon polygon)
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
                        Directory + @"Special\bezier.frag",
                        ShaderStage.Geometry);
                case ShaderType.Displacement:
                    return ShaderFactory.Instance.CreateTesselation(
                        Directory + @"Special\displacement.vert",
                        Directory + @"Special\displacement.frag",
                        Directory + @"Special\displacement.geom",
                        Directory + @"Special\displacement.tcs",
                        Directory + @"Special\displacement.tes",
                        ShaderStage.Geometry);
                case ShaderType.EffectLine:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"Special\effectline.vert",
                        Directory + @"Special\effectline.frag",
                        ShaderStage.Geometry);
                case ShaderType.NURBS:
                    return ShaderFactory.Instance.CreateTesselation(
                        Directory + @"Special\nurbs.vert",
                        Directory + @"Special\nurbs.frag",
                        Directory + @"Special\nurbs.geom",
                        Directory + @"Special\nurbs.tcs",
                        Directory + @"Special\nurbs.tes",
                        ShaderStage.Geometry);
                case ShaderType.Fur:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"Special\fur.vert",
                        Directory + @"Special\fur.frag",
                        ShaderStage.Geometry);
                case ShaderType.Outline:
                    return ShaderFactory.Instance.CreateGeometryShader(
                        Directory + @"Special\outline.vert",
                        Directory + @"Special\outline.frag",
                        Directory + @"Special\outline.geom",
                        ShaderStage.Geometry);
                case ShaderType.WireFrame:
                    return ShaderFactory.Instance.CreateShaderVF(
                        Directory + @"GBuffer\WireFrame.vert",
                        Directory + @"GBuffer\WireFrame.frag",
                        ShaderStage.Geometry);
            }

            return null;
        }
    }
}
