using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.Utility;
namespace RenderApp.Render_System.Post_Effect
{
    partial class Sobel : PostEffect
    {
        private int _render2D;
        private int uRender2D
        {
            get
            {
                return _render2D;
            }
            set
            {
                SetValue<int>(ref _render2D,value);
            }
        }
        private int _uWidth;
        public int uWidth
        {
            get
            {
                return _uWidth;
            }
            set
            {
                SetValue<int>(ref _uWidth, value);
            }
        }
        private int _uHeight;
        public int uHeight
        {
            get
            {
                return _uHeight;
            }
            set
            {
                SetValue<int>(ref _uHeight, value);
            }
        }

        private float _threshold;
        private float uThreshold
        {
            get
            {
                return _threshold;
            }
            set
            {
                SetValue<float>(ref _threshold, value);
            }
        }
        #region [Shaderの初期化関数]
        public override void Initialize()
        {
            uThreshold = 1.0f;
            uWidth = 1;
            uHeight = 1;
            uRender2D = 1;
        }
        public Sobel(Shader shader)
            :base(shader)
        {

        }
        #endregion

    }
}
