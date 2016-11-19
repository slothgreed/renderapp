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
//    class OutLine : Shader
//    {
//        #region [singlton method]
//        private static OutLine m_Instance = new OutLine();
//        public static OutLine Instance { get { return m_Instance; } }

//        private OutLine()
//        {
//        }
//        #endregion

//        private int uModelMatrix;
//        private int uLightPos;
//        private int uCameraPos;
//        private int uCameraMatrix;
//        private int uMVP;
//        private int uOffset;
//        private int uNormalMatrix;
//        protected override void SetUniformName()
//        {
//            uModelMatrix = GL.GetUniformLocation(Program, "ModelMatrix");
//            uLightPos = GL.GetUniformLocation(Program,"LightPos");
//            uCameraPos = GL.GetUniformLocation(Program,"CameraPos");
//            uMVP = GL.GetUniformLocation(Program,"MVP");
//            uCameraMatrix = GL.GetUniformLocation(Program,"CameraMatrix");
//            uOffset = GL.GetUniformLocation(Program, "Offset");
//            uNormalMatrix = GL.GetUniformLocation(Program, "NormalMatrix");
//        }

//    }
//}
