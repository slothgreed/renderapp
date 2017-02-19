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
    public class PostPlane
    {
        private Geometry Plane;
        public RenderTarget RenderTarget
        {
            get;
            set;
        }
        public PostPlane(string name, Shader shader, RenderTarget frame)
        {
            RenderTarget = frame;
            Plane = AssetFactory.Instance.CreatePlane(name, shader);
        }
        public PostPlane(string name,Shader shader)
        {
            Plane = AssetFactory.Instance.CreatePlane(name, shader);
        }
        public void SetValue(string key,object value)
        {
            Plane.MaterialItem.CurrentShader.SetValue(key,value);
        }

        public void SetPlaneTexture(TextureKind kind,Texture texture)
        {
            Plane.MaterialItem.AddTexture(kind, texture);
        }
        public void Render()
        {
            if(RenderTarget != null)
            {
                RenderTarget.BindRenderTarget();
                Plane.Render();
                RenderTarget.UnBindRenderTarget();
            }
            else
            {
                Plane.Render();
            }
        }
        public void ClearBuffer()
        {
            RenderTarget.ClearBuffer();
        }

        public void SizeChanged(int width,int height)
        {
            if(RenderTarget != null)
            {
                RenderTarget.SizeChanged(width, height);
            }
        }

        public void Dispose()
        {
            Plane.Dispose();
        }
    }
}
