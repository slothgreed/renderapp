using KI.Asset;
using KI.Gfx.KITexture;
using KI.Gfx.Render;

namespace KI.Renderer
{
    public class GBuffer : RenderTechnique
    {
        public enum GBufferOutputType
        {
            Posit = 0,
            Normal,
            Color,
            Light
        }

        public GBuffer(RenderTechniqueType tech)
            : base("GBuffer", tech, RenderType.Original)
        {
        
        }

        public Texture GetOutputTexture(GBufferOutputType target)
        {
            return OutputTexture[(int)target];
        }
        public override void CreateRenderTarget(int width, int height)
        {
            Texture[] texture = new Texture[4];
            texture[0] = TextureFactory.Instance.CreateTexture("GPosit", width, height);
            texture[1] = TextureFactory.Instance.CreateTexture("GNormal", width, height);
            texture[2] = TextureFactory.Instance.CreateTexture("GColor", width, height);
            texture[3] = TextureFactory.Instance.CreateTexture("GLight", width, height);

            //RenderApp.Globals.Project.ActiveProject.AddChild(texture[0]);
            //RenderApp.Globals.Project.ActiveProject.AddChild(texture[1]);
            //RenderApp.Globals.Project.ActiveProject.AddChild(texture[2]);
            //RenderApp.Globals.Project.ActiveProject.AddChild(texture[3]);

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
            foreach (var asset in Global.Scene.RootNode.AllChildren())
            {
                if (asset.KIObject is RenderObject)
                {
                    var geometry = asset.KIObject as RenderObject;
                    geometry.Render(Global.Scene);
                }
            }
            RenderTarget.UnBindRenderTarget();
        }
    }
}
