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

        public List<RenderTechnique> PostEffects = new List<RenderTechnique>();

        public PostProcess(Geometry plane)
        {
            Plane = plane;
        }

        private void ClearBuffer()
        {
            foreach(var post in PostEffects)
            {
                post.ClearBuffer();
            }
        }

        public void SizeChanged(int width, int height)
        {
            foreach (var post in PostEffects)
            {
                post.SizeChanged(width, height);
            }
        }
        public void Render()
        {
           foreach(var post in PostEffects)
           {
               post.RenderTarget.BindRenderTarget(post.OutputTexture.ToArray());
               Plane.Shader = post.Shader;
               Plane.Render();
               post.RenderTarget.UnBindRenderTarget();
           }
        }

        public void Dispose()
        {
            Plane.Dispose();
        }
    }
}
