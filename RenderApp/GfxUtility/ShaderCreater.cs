﻿using RenderApp.AssetModel;
using KI.Gfx.GLUtil;
using KI.Gfx.KIAsset;
using KI.Gfx.KIShader;
namespace RenderApp.GfxUtility
{
    public class ShaderCreater
    {
        private static ShaderCreater _Instance;
        public static ShaderCreater Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new ShaderCreater();
                }
                return _Instance;
            }
        }
        public string Directory
        {
            get
            {
                return ProjectInfo.ShaderDirectory + @"\";
            }
        }
        public bool CheckBufferEnable(BufferObject buffer)
        {
            if(buffer == null)
            {
                return false;
            }
            return buffer.Enable;
        }
        /// <summary>
        /// 汎用的な頂点シェーダの読み込み
        /// </summary>
        public string GetVertexShader(Geometry geometry)
        {
            if (CheckBufferEnable(geometry.PositionBuffer) &&
                CheckBufferEnable(geometry.NormalBuffer) &&
                CheckBufferEnable(geometry.ColorBuffer) &&
                CheckBufferEnable(geometry.TexCoordBuffer))
            {
                return Directory + "GeneralPNCT.vert";
            }
            if (CheckBufferEnable(geometry.PositionBuffer) &&
                CheckBufferEnable(geometry.NormalBuffer) &&
                CheckBufferEnable(geometry.TexCoordBuffer))
            {
                return Directory + "GeneralPNT.vert";
            }
            if (CheckBufferEnable(geometry.PositionBuffer) &&
                CheckBufferEnable(geometry.TexCoordBuffer))
            {
                return Directory + "GeneralPT.vert";
            }
            if (CheckBufferEnable(geometry.PositionBuffer))
            {
                return Directory + "GeneralP.vert";
            }
            return null;
        }
        /// <summary>
        /// フラグシェーダの切り替え
        /// </summary>
        public string GetConstantFragShader(Geometry geometry)
        {
            if (geometry.PositionBuffer.Enable && geometry.ColorBuffer.Enable && geometry.NormalBuffer.Enable)
            {
                return Directory + "ConstantPCN.vert";
            }
            if (geometry.PositionBuffer.Enable && geometry.ColorBuffer.Enable)
            {
                return Directory + "ConstantPC.frag";
            } 
            if (geometry.PositionBuffer.Enable && geometry.NormalBuffer.Enable)
            {
                return Directory + "ConstantPN.vert";
            }
            if (geometry.PositionBuffer.Enable)
            {
                return Directory + "ConstantP.frag";
            }
            return null;
        }
        public string GetOBJFragShader(Geometry geometry)
        {
            if (geometry.GetTexture(TextureKind.Albedo) != null &&
                geometry.GetTexture(TextureKind.Normal) != null &&
                geometry.GetTexture(TextureKind.Specular) != null)
            {
                return Directory + "GeneralANS.frag";
            }
            if (geometry.GetTexture(TextureKind.Albedo) != null &&
                geometry.GetTexture(TextureKind.Normal) != null)
            {
                return Directory + "GeneralAN.frag";
            }
            if (geometry.GetTexture(TextureKind.Albedo) != null &&
                geometry.GetTexture(TextureKind.Specular) != null)
            {
                return Directory + "GeneralAS.frag";
            }
            if (geometry.GetTexture(TextureKind.Albedo) != null)
            {
                return Directory + "GeneralA.frag";
            }
            return null;
        }

        private Shader _defaultAnalyzeShader;
        public Shader DefaultAnalyzeShader
        {
            get
            {
                if (_defaultAnalyzeShader == null)
                {
                    _defaultAnalyzeShader = ShaderFactory.Instance.CreateShaderVF(ProjectInfo.ShaderDirectory + @"\ConstantGeometry");
                }
                return _defaultAnalyzeShader;
            }
        }
    }
}
