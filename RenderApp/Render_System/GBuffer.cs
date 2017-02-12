using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
using KI.Gfx.KIAsset;
namespace RenderApp.Render_System
{
    class GBuffer : FrameBuffer
    {
        Dictionary<TextureKind, Texture> textureList = new Dictionary<TextureKind, Texture>();

        public GBuffer(int width, int height)
            :base("GBuffer")
        {
            Texture[] texture = new Texture[4];
            texture[0] = TextureFactory.Instance.CreateTexture("GPosit", width, height);
            texture[1] = TextureFactory.Instance.CreateTexture("GNormal", width, height);
            texture[2] = TextureFactory.Instance.CreateTexture("GColor", width, height);
            texture[3] = TextureFactory.Instance.CreateTexture("GLight", width, height);
            Initialize(width, height, texture);

            RenderApp.Globals.Project.ActiveProject.AddChild(texture[0]);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture[1]);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture[2]);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture[3]);

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
