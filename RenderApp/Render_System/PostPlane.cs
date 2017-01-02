using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.Render_System
{
    public class PostPlane
    {
        private Geometry Plane;
        public FrameBuffer FrameBufferItem
        {
            get;
            set;
        }
        public PostPlane(string name,Shader shader,FrameBuffer frame)
        {
            FrameBufferItem = frame;
            Plane = AssetFactory.Instance.CreatePlane(name, shader).geometry;
        }
        public PostPlane(string name,Shader shader)
        {
            Plane = AssetFactory.Instance.CreatePlane(name, shader).geometry;
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
            if(FrameBufferItem != null)
            {
                FrameBufferItem.BindBuffer();
                Plane.Render();
                FrameBufferItem.UnBindBuffer();
            }
            else
            {
                Plane.Render();
            }
        }
        public void ClearBuffer()
        {
            FrameBufferItem.ClearBuffer();
        }

        public void SizeChanged(int width,int height)
        {
            if(FrameBufferItem != null)
            {
                FrameBufferItem.SizeChanged(width, height);
            }
        }

        public void Dispose()
        {
            Plane.Dispose();
        }
    }
}
