using RenderApp.AssetModel;
using KI.Gfx.GLUtil;
using KI.Gfx.KIAsset;
using KI.Gfx.KIShader;
namespace RenderApp.GfxUtility
{
    public enum ShaderType
    {
        Bezier,
        Displacement,
        EffectLine,
        NURBS,
        Fur
    }
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
                return Directory + @"GBuffer\GeneralPNCT.vert";
            }
            if (CheckBufferEnable(geometry.PositionBuffer) &&
                CheckBufferEnable(geometry.NormalBuffer) &&
                CheckBufferEnable(geometry.TexCoordBuffer))
            {
                return Directory + @"GBuffer\GeneralPNT.vert";
            }
            if (CheckBufferEnable(geometry.PositionBuffer) &&
                CheckBufferEnable(geometry.NormalBuffer) &&
                CheckBufferEnable(geometry.ColorBuffer))
            {
                return Directory + @"GBuffer\GeneralPCN.vert";
            }

            if (CheckBufferEnable(geometry.PositionBuffer) &&
                CheckBufferEnable(geometry.ColorBuffer))
            {
                return Directory + @"GBuffer\GeneralPC.vert";
            }
            
            if (CheckBufferEnable(geometry.PositionBuffer) &&
                CheckBufferEnable(geometry.TexCoordBuffer))
            {
                return Directory + @"GBuffer\GeneralPT.vert";
            }
            if (CheckBufferEnable(geometry.PositionBuffer))
            {
                return Directory + @"GBuffer\GeneralP.vert";
            }
            return null;
        }
        /// <summary>
        /// フラグシェーダの切り替え
        /// </summary>
        public string GetFragShader(Geometry geometry)
        {
            if (geometry.PositionBuffer != null && geometry.TexCoordBuffer != null && geometry.NormalBuffer != null)
            {
                if (geometry.PositionBuffer.Enable && geometry.TexCoordBuffer.Enable && geometry.NormalBuffer.Enable)
                {
                    return Directory + @"GBuffer\GeneralPNT.frag";
                }
            }

            if(geometry.PositionBuffer != null && geometry.ColorBuffer != null && geometry.NormalBuffer != null)
            {
                if (geometry.PositionBuffer.Enable && geometry.ColorBuffer.Enable && geometry.NormalBuffer.Enable)
                {
                    return Directory + @"GBuffer\GeneralPCN.frag";
                }
            }
            if (geometry.PositionBuffer != null && geometry.ColorBuffer != null)
            {
                if (geometry.PositionBuffer.Enable && geometry.ColorBuffer.Enable)
                {
                    return Directory + @"GBuffer\GeneralPC.frag";
                }
            }
            if (geometry.PositionBuffer != null && geometry.NormalBuffer != null)
            {

                if (geometry.PositionBuffer.Enable && geometry.NormalBuffer.Enable)
                {
                    return Directory + @"GBuffer\GeneralPN.frag";
                }

            }
            if (geometry.PositionBuffer != null)
            {
                if (geometry.PositionBuffer.Enable)
                {
                    return Directory + @"GBuffer\GeneralP.frag";
                }
            }
            return null;
        }
        public string GetOBJFragShader(Geometry geometry)
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
