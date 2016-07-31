using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Assets;
using RenderApp.GLUtil;
namespace RenderApp
{
    public class PostProcess
    {
        private Geometry Plane;
        private Shader _shader;
        public Shader ShaderItem
        {
            get
            {
                return _shader;
            }
            set
            {
                _shader = value;
                _shader.AnalizeShaderProgram();
            }
        }


        public void PostProcess(Shader shader)
        {
            ShaderItem = shader;
            Plane = new Plane();
        }

        public void BindFrameBuffer(FrameBuffer frame)
        {
            
        }
        private void Render()
        {
            Plane.Render();
        }
    }
}
