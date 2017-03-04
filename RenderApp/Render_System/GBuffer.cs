using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
using KI.Gfx.KIAsset;
using KI.Gfx.Render;
using RenderApp.AssetModel;
namespace RenderApp.Render_System
{
    public class GBuffer : RenderTechnique
    {
        Dictionary<TextureKind, Texture> textureList = new Dictionary<TextureKind, Texture>();

        public GBuffer(int width, int height)
            : base("GBuffer", RenderType.Original)
        {
        
        
        }

        public override void CreateRenderTarget(int width, int height)
        {
            Texture[] texture = new Texture[4];
            texture[0] = TextureFactory.Instance.CreateTexture("GPosit", width, height);
            texture[1] = TextureFactory.Instance.CreateTexture("GNormal", width, height);
            texture[2] = TextureFactory.Instance.CreateTexture("GColor", width, height);
            texture[3] = TextureFactory.Instance.CreateTexture("GLight", width, height);

            RenderApp.Globals.Project.ActiveProject.AddChild(texture[0]);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture[1]);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture[2]);
            RenderApp.Globals.Project.ActiveProject.AddChild(texture[3]);

            textureList.Add(TextureKind.World, texture[0]);
            textureList.Add(TextureKind.Normal, texture[1]);
            textureList.Add(TextureKind.Albedo, texture[2]);
            textureList.Add(TextureKind.Lighting, texture[3]);

            RenderTarget = new RenderTarget(Name, width, height, texture);
        }

        public Texture FindTexture(TextureKind kind)
        {
            if(textureList.ContainsKey(kind))
            {
                return textureList[kind];
            }
            return null;
        }


        public override void Initialize()
        {
        }

        public override void Render()
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget();
            foreach (var asset in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            {
                if (asset.KIObject is Geometry)
                {
                    var geometry = asset.KIObject as Geometry;
                    geometry.Render();
                }
            }
            RenderTarget.UnBindRenderTarget();
        }
    }
}
