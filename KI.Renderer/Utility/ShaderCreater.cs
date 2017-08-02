using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using KI.Asset;
using KI.Gfx.KITexture;
using KI.Renderer;

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
        Fur
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
            string shaderPath = GetTextureFragShader(renderObject);
            if (shaderPath != null)
            {
                return shaderPath;
            }

            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.NormalBuffer) &&
                CheckBufferEnable(renderObject.ColorBuffer) &&
                CheckBufferEnable(renderObject.TexCoordBuffer))
            {
                return Directory + @"GBuffer\GeneralPNCT.vert";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.NormalBuffer) &&
                CheckBufferEnable(renderObject.TexCoordBuffer))
            {
                return Directory + @"GBuffer\GeneralPNT.vert";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.NormalBuffer) &&
                CheckBufferEnable(renderObject.ColorBuffer))
            {
                return Directory + @"GBuffer\GeneralPCN.vert";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.ColorBuffer))
            {
                return Directory + @"GBuffer\GeneralPC.vert";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.TexCoordBuffer))
            {
                return Directory + @"GBuffer\GeneralPT.vert";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer))
            {
                return Directory + @"GBuffer\GeneralP.vert";
            }

            return null;
        }

        /// <summary>
        /// フラグシェーダの取得
        /// </summary>
        /// <param name="renderObject">描画オブジェクト</param>
        /// <returns>ファイルパス</returns>
        public string GetFragShader(RenderObject renderObject)
        {
            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.TexCoordBuffer) &&
                CheckBufferEnable(renderObject.NormalBuffer))
            {
                return Directory + @"GBuffer\GeneralPNT.frag";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.ColorBuffer) &&
                CheckBufferEnable(renderObject.NormalBuffer))
            {
                return Directory + @"GBuffer\GeneralPCN.frag";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.ColorBuffer))
            {
                return Directory + @"GBuffer\GeneralPC.frag";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer) &&
                CheckBufferEnable(renderObject.NormalBuffer))
            {
                return Directory + @"GBuffer\GeneralPN.frag";
            }

            if (CheckBufferEnable(renderObject.PositionBuffer))
            {
                return Directory + @"GBuffer\GeneralPN.frag";
            }

            return null;
        }

        /// <summary>
        /// テクスチャ付フラグメントシェーダの取得
        /// </summary>
        /// <param name="geometry">形状</param>
        /// <returns>ファイルパス</returns>
        public string GetTextureFragShader(Geometry geometry)
        {
            if (geometry.GetTexture(TextureKind.Albedo) != null &&
                geometry.GetTexture(TextureKind.Normal) != null &&
                geometry.GetTexture(TextureKind.Specular) != null)
            {
                return Directory + @"GBuffer\GeneralANS.frag";
            }

            if (geometry.GetTexture(TextureKind.Albedo) != null &&
                geometry.GetTexture(TextureKind.Normal) != null)
            {
                return Directory + @"GBuffer\GeneralAN.frag";
            }

            if (geometry.GetTexture(TextureKind.Albedo) != null &&
                geometry.GetTexture(TextureKind.Specular) != null)
            {
                return Directory + @"GBuffer\GeneralAS.frag";
            }

            if (geometry.GetTexture(TextureKind.Albedo) != null)
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
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"GBuffer\bezier");
                case ShaderType.Displacement:
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"GBuffer\disp");
                case ShaderType.EffectLine:
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"GBuffer\effectline");
                case ShaderType.NURBS:
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"GBuffer\nurbs");
                case ShaderType.Fur:
                    return ShaderFactory.Instance.CreateShaderVF(Directory + @"GBuffer\fur");
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

        //private Shader _defaultShader;
        //public Shader DefaultShader
        //{
        //    get
        //    {
        //        if (_defaultShader == null)
        //        {
        //            _defaultShader = ShaderFactory.Instance.CreateShaderVF(ProjectInfo.ShaderDirectory + @"\GBuffer\ConstantGeometry");
        //        }
        //        return _defaultShader;
        //    }
        //}
    }
}
