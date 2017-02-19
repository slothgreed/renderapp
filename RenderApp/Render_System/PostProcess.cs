using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
using KI.Gfx.KIAsset;
using KI.Gfx.Render;
namespace RenderApp.Render_System
{
    class PostProcess
    {
        private Geometry Plane;

        public List<KeyValuePair<Material, RenderTarget>> RenderTargets = new List<KeyValuePair<Material, RenderTarget>>();

        public PostProcess(string name)
        {
            Plane = AssetFactory.Instance.CreatePostProcessPlane("PostProcess");
        }

        private void ClearBuffer()
        {
            foreach(var render in RenderTargets)
            {
                render.Value.ClearBuffer();
            }
        }

        public void SizeChanged(int width, int height)
        {
            foreach (var render in RenderTargets)
            {
                render.Value.SizeChanged(width, height);
            }
        }
        public void Render()
        {
           foreach(var render in RenderTargets)
           {
               render.Value.BindRenderTarget();
               Plane.MaterialItem = render.Key;
               Plane.Render();
               render.Value.UnBindRenderTarget();
           }
        }

        public void Dispose()
        {
            Plane.Dispose();
        }
    }
}
