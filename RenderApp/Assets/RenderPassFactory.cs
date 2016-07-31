using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderApp.GLUtil;
namespace RenderApp
{
    public class RenderPassFactory
    {
        private static RenderPassFactory _instance = new RenderPassFactory();
        public static RenderPassFactory Instance
        {
            get
            {
                return _instance;
            }
        }
        public FrameBuffer CreateGBuffer(int width,int height)
        {
            string[] textureName = new string[4];
            textureName[0] = "Posit";
            textureName[1] = "Normal";
            textureName[2] = "Color";
            textureName[3] = "Light";

           return new FrameBuffer("GBuffer", width, height, textureName);
        }
        public FrameBuffer CreateDefaultLithingBuffer(int width,int height)
        {
            //todo:
            return new FrameBuffer("todo",width,height,"todo");
        }
    }

}
