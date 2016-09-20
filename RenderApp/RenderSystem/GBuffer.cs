using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
namespace RenderApp
{
    class GBuffer : FrameBuffer
    {
        Dictionary<TextureKind, Texture> textureList = new Dictionary<TextureKind, Texture>();

        public GBuffer(int width, int height)
            :base("GBuffer")
        {
            string[] textureName = new string[4];
            textureName[0] = "GPosit";
            textureName[1] = "GNormal";
            textureName[2] = "GColor";
            textureName[3] = "GLight";
            Initialize(width, height, textureName);
            textureList.Add(TextureKind.World, TextureList[0]);
            textureList.Add(TextureKind.Normal, TextureList[1]);
            textureList.Add(TextureKind.Albedo, TextureList[2]);
            textureList.Add(TextureKind.Lighting, TextureList[3]);
        }
        public Texture FindTexture(TextureKind kind)
        {
            if(textureList.ContainsKey(kind))
            {
                return textureList[kind];
            }
            return null;
        }

    }
}
