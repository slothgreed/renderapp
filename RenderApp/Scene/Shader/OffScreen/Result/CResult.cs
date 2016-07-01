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
//using RenderApp.Assets;
//namespace RenderApp.Assets.OffScreen
//{
//    /// <summary>
//    /// MainShader
//    /// </summary>
//    class CResult : Shader
//    {
//        private static CResult m_Instance = new CResult();
//        public static CResult Instance { get { return m_Instance; } }

//        private int   urender_2D;
//        private int TextureBindId = 0;
//        #region [Shaderの初期化関数]
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.result_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.result_frag);
//            return CreateShaderProgram(vertShader, fragShader);
//        }

//        protected override void SetUniformName()
//        {
//            urender_2D = GL.GetUniformLocation(Program, "render_2D");
//        }

//        #endregion
//        #region [レンダリング処理]
//        public void SetRenderTexture(int textureId)
//        {
//            TextureBindId = textureId;
//        }
//        public override void ShaderRender(CModel model)
//        {
//            model.SetUniform2DTexture(urender_2D, 0);
//            GL.BindTexture(TextureTarget.Texture2D,TextureBindId);
//            model.Draw();
//        }
        
//        #endregion
//    }
//}
