using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using KI.Asset;
using KI.Gfx.KITexture;
using KI.Renderer;

namespace KI.Asset
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
                return Global.ShaderDirectory + @"\";
            }
        }
        private bool CheckBufferEnable(BufferObject buffer)
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
        public string GetVertexShader(RenderObject renderObject)
        {
            string shaderPath = GetTextureFragShader(renderObject);
            if(shaderPath != null)
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
        /// フラグシェーダの切り替え
        /// </summary>
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
