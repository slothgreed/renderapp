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

            OutputTexture.Add(texture[0]);
            OutputTexture.Add(texture[1]);
            OutputTexture.Add(texture[2]);
            OutputTexture.Add(texture[3]);

            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget(Name, width, height, OutputTexture.Count);
        }

        public override void Initialize()
        {
        }

        public override void Render()
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
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
