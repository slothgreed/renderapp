//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Scene;
//namespace RenderApp.AssetModel.MRT
//{
//    class Fur : Shader
//    {
//        #region [singlton method]
//        private static Fur m_Instance = new Fur();
//        public static Fur Instance { get { return m_Instance; } }

//        private Fur()
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
//        private int uFurTexture;
//        private int uFurBumpTexture;
//        private Texture FurTexture = null;
//        private Texture FurBumpTexture = null;
        
//        protected override void SetUniformName()
//        {
//            uModelMatrix = GL.GetUniformLocation(Program,"ModelMatrix");
//            uLightPos = GL.GetUniformLocation(Program,"LightPos");
//            uCameraPos = GL.GetUniformLocation(Program,"CameraPos");
//            uMVP = GL.GetUniformLocation(Program,"MVP");
//            uCameraMatrix = GL.GetUniformLocation(Program,"CameraMatrix");
//            uOffset = GL.GetUniformLocation(Program, "Offset");
//            uNormalMatrix = GL.GetUniformLocation(Program, "NormalMatrix");
//            uFurTexture = GL.GetUniformLocation(Program, "FurTexture");
//            uFurBumpTexture = GL.GetUniformLocation(Program, "FurBumpTexture");
//        }
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            string geomShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.fur_vert);
//            geomShader = Encoding.UTF8.GetString(Properties.Resources.fur_geom);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.fur_frag);
//            SetFurTexture("fur.png");
//            SetFurBumpTexture("furbump.png");

//            return CreateShaderProgram(vertShader,geomShader,fragShader,4,4);
//        }
//        public void SetFurTexture(string fileName)
//        {
//            if(FurTexture != null)
//            {
//                FurTexture.Dispose();
//            }
//            FurTexture = new Texture(fileName);
//        }
//        public void SetFurBumpTexture(string fileName)
//        {
//            if (FurBumpTexture != null)
//            {
//                FurBumpTexture.Dispose();
//            }
//            FurBumpTexture = new Texture(fileName);
//        }
//    }
//}
