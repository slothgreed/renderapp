//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Scene;
//namespace RenderApp.Assets.MRT
//{
//    class ConstantShader : Shader
//    {
//        private static ConstantShader m_Instance = new ConstantShader();

//        #region [singlton method]
//        private ConstantShader()
//        {
//        }
//        public static ConstantShader GetInstance()
//        {
//            return m_Instance;
//        }
//        #endregion
//        private int uMVP;
//        private int uModelMatrix;
//        private int uNormalMatrix;
//        private int uCameraPos;
//        private int uLightPos;
//        private int uCameraMatrix;
//        private int uTextureNum;
//        private int uTexture1;
//        /// <summary>
//        /// ユニフォーム名の取得
//        /// </summary>
//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//            uModelMatrix = GL.GetUniformLocation(Program, "ModelMatrix");
//            uNormalMatrix = GL.GetUniformLocation(Program, "NormalMatrix");
//            uCameraPos = GL.GetUniformLocation(Program, "CameraPos");
//            uLightPos = GL.GetUniformLocation(Program, "LightPos");
//            uCameraMatrix = GL.GetUniformLocation(Program, "CameraMatrix");
//            uTextureNum = GL.GetUniformLocation(Program, "textureNum");
//            uTexture1 = GL.GetUniformLocation(Program, "texture1");
//        }
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.General_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.General_frag);
//            return CreateShaderProgram(vertShader, fragShader);
//        }

//    }
//}
