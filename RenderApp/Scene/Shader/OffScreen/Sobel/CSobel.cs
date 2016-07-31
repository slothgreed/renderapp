//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using System.Drawing.Text;
//using OpenTK;
//using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Utility;
//using RenderApp.Scene;
//using RenderApp.AssetModel;
//namespace RenderApp.AssetModel.OffScreen
//{
//    /// <summary>
//    /// MainShader
//    /// </summary>
//    class CSobel : Shader
//    {
//        private int urender_2D;
//        private int uWidth;
//        private int uHeight;
//        private int uThreshold;
//        private int TextureBindId = 0;
//        public float Threshold { get; set; }
//        #region [Shaderの初期化関数]
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.sobel_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.sobel_frag);
//            return CreateShaderProgram(vertShader, fragShader);
//        }
//        public override void Initialize()
//        {
//            Threshold = 0.1f;
//        }
//        protected override void SetUniformName()
//        {
//            urender_2D = GL.GetUniformLocation(Program, "render_2D");
//            uWidth = GL.GetUniformLocation(Program, "width");
//            uHeight = GL.GetUniformLocation(Program, "height");
//            uThreshold = GL.GetUniformLocation(Program, "threshold");
//        }

//        #endregion
//        #region [レンダリング処理]
//        public void SetRenderTexture(int textureId)
//        {
//            TextureBindId = textureId;
//        }
        
//        #endregion

//        public override void AnimationTimer(int timerCount, float angleCount)
//        {
//            Threshold = (float)Math.Abs(-Math.Sin(angleCount));
//        }
//        public override void StopTimer()
//        {
//            Threshold = 0.9f;
//        }
//    }
//}
