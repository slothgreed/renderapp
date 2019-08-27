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
//    class CDefferd : Shader
//    {
//        private static CDefferd m_Instance = new CDefferd();

//        private int uLightMatrix;
//        private int uCameraMatrix;
//        private int uShadowMode;
//        private int ufogStart;
//        private int ufogEnd;
//        private int ufogColor;

//        private int uposit_2D ;
//        private int ulight_2D ;
//        private int unormal_2D;
//        private int ucolor_2D ;
//        private int uCameraPos;
//        private int uLightPos ;
//        private int uUnProjectMatrix;
//        private int ushadow_2D;
//        private int uProjectMatrix;
//         ///<summary>
//         ///シングルトンインスタンスの取得
//         ///</summary>
//         ///<returns></returns>
//        public static CDefferd GetInstance()
//        {
//            return m_Instance;
//        }

//        #region [Shaderの初期化関数]
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.shading_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.shading_frag);
//            return CreateShaderProgram(vertShader, fragShader);
//        }
//        protected override void SetUniformName()
//        {
//            uposit_2D = GL.GetUniformLocation(Program, "posit_2D");
//            ulight_2D = GL.GetUniformLocation(Program, "light_2D");
//            unormal_2D = GL.GetUniformLocation(Program, "normal_2D");
//            ucolor_2D = GL.GetUniformLocation(Program, "color_2D");
//            uCameraPos = GL.GetUniformLocation(Program, "CameraPos");
//            uLightPos = GL.GetUniformLocation(Program, "LightPos");
//            uUnProjectMatrix = GL.GetUniformLocation(Program, "UnProject");
//            ushadow_2D = GL.GetUniformLocation(Program, "shadow_2D");
//            uLightMatrix = GL.GetUniformLocation(Program, "LightMatrix");
//            uCameraMatrix = GL.GetUniformLocation(Program, "CameraMatrix");
//            uProjectMatrix = GL.GetUniformLocation(Program, "Project");
//            uShadowMode = GL.GetUniformLocation(Program, "ShadowMode");
//            ufogStart = GL.GetUniformLocation(Program, "fogStart");
//            ufogEnd = GL.GetUniformLocation(Program, "fogEnd");
//            ufogColor = GL.GetUniformLocation(Program, "fogColor");
//        }

//        #endregion

//    }
//}
