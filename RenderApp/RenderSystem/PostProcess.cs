using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp
{
    public class PostProcess
    {
        private Geometry Plane;
        public PostProcess(string name,Shader shader,FrameBuffer frame)
        {
            FrameBufferItem = frame;
            Plane = new Plane(name);
            Plane.MaterialItem = new Material(name);
            Scene.ActiveScene.AddSceneObject(name,Plane.MaterialItem);
            Plane.MaterialItem.SetShader(shader);
        }
        public PostProcess(string name,Shader shader)
        {
            Plane = new Plane(name);
            Plane.MaterialItem = new Material(name);
            Scene.ActiveScene.AddSceneObject(name, Plane.MaterialItem); 
            Plane.MaterialItem.SetShader(shader);
        }
        private FrameBuffer frameBuffer;
        public FrameBuffer FrameBufferItem
        {
            get;
            set;
        }
        public void SetPlaneTexture(TextureKind kind,Texture texture)
        {
            Plane.MaterialItem.AddTexture(kind, texture);
        }
        public void Render()
        {
            FrameBufferItem.BindBuffer();
            Plane.Render();
            FrameBufferItem.UnBindBuffer();
        }
        public void ClearBuffer()
        {
            FrameBufferItem.ClearBuffer();
        }
        public void OutputRender()
        {
            Plane.Render();
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
