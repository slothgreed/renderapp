﻿using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
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
            string shaderPath = GetTextureFragShader(renderObject.Geometry);
            if (shaderPath != null)
            {
                return shaderPath;
            }

            if (renderObject.Geometry.GeometryType == GeometryType.Line ||
                renderObject.Geometry.GeometryType == GeometryType.Point)
            {
                return Directory + @"GBuffer\GeneralPC.vert";
            }

            if (renderObject.Geometry.TextureNum != 0)
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
            if (renderObject.Geometry.GeometryType == GeometryType.Line ||
                renderObject.Geometry.GeometryType == GeometryType.Point)
            {
                return Directory + @"GBuffer\GeneralPC.frag";
            }

            if (renderObject.Geometry.TextureNum != 0)
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
    }
}
