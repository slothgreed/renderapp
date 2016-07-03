using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderApp.GLUtil;
namespace RenderApp
{
    public class GBuffer
    {
        public readonly string Posit = "Posit";
        public readonly string Normal = "Normal";
        public readonly string Color = "Color";
        public readonly string Light = "Light";

        public GBuffer(int width,int height)
            :base()
        {
            string[] textureName = new string[4];
            textureName[0] = Posit;
            textureName[1] = Normal;
            textureName[2] = Color;
            textureName[3] = Light;

            FrameBuffer frame = new FrameBuffer("GBuffer", width, height, textureName);

        }
    }

}
