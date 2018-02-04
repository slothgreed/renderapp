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
        Outline
    }

    /// <summary>
    /// シェーダ生成クラス
    /// </summary>
    public class ShaderCreater
    {
        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static ShaderCreater Instance { get; } = new ShaderCreater();

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
        public string GetVertexShader(RenderObject renderObject)
        {
            if (renderObject.Polygon.Type == PrimitiveType.Lines ||
                renderObject.Polygon.Type == PrimitiveType.Points)
            {
                return Directory + @"GBuffer\GeneralPC.vert";
            }

            if (renderObject.Polygon.Textures.Count != 0)
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
        public string GetFragShader(RenderObject renderObject)
        {
            string shaderPath = GetTextureFragShader(renderObject.Polygon);
            if (shaderPath != null)
            {
                return shaderPath;
            }

            if (renderObject.Polygon.Type == PrimitiveType.Lines ||
                renderObject.Polygon.Type == PrimitiveType.Points)
            {
                return Directory + @"GBuffer\GeneralPC.frag";
            }

            if (renderObject.Polygon.Textures.Count != 0)
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
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"Special\bezier");
                case ShaderType.Displacement:
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"Special\disp");
                case ShaderType.EffectLine:
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"Special\effectline");
                case ShaderType.NURBS:
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"Special\nurbs");
                case ShaderType.Fur:
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"Special\fur");
                case ShaderType.Outline:
                    return ShaderFactory.Instance.CreateGeometryShader(Directory + @"Special\outline");
            }

            return null;
        }

        /// <summary>
        /// 形状データのバッファ状態の取得
        /// </summary>
        /// <param name="buffer">バッファ</param>
        /// <returns>enable</returns>
        private bool CheckBufferEnable(BufferObject buffer)
        {
            if (buffer == null)
            {
                return false;
            }

            return buffer.Enable;
        }
    }
}
