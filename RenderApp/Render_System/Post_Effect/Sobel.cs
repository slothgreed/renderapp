using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using RenderApp.Utility;
namespace RenderApp.Render_System
{
    partial class Sobel : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\sobel.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\sobel.frag";
        
        #region [Shaderの初期化関数]
        public override void Initialize()
        {
            uThreshold = 0.001f;
            uWidth = 1;
            uHeight = 1;
            uTarget = null;
        }
        public Sobel(RenderTechniqueType tech)
            : base("Sobel", vertexShader, fragShader, tech, RenderType.OffScreen)
        {

        }

        public override void SizeChanged(int width, int height)
        {
            base.SizeChanged(width, height);
            uWidth = width;
            uHeight = height;
        }
        #endregion

    }
}
