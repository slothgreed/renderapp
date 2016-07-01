//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Utility;
//using RenderApp.Scene;
//namespace RenderApp.Assets
//{
//    class CDiffuse : Shader
//    {
//        #region [シングルトン]
//        private static CDiffuse m_Instance = new CDiffuse();
//        public static CDiffuse GetInstance()
//        {
//            return m_Instance;
//        }
//        #endregion



//        private int uMVP;
//        private int uModelMatrix;
//        private int uNormalMatrix;
//        private int uLightPos;
//        private int uCameraPos;
//        private int uProject;
//        private int uLightMatrix;
//        private int uShadowMap;

//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.diffuse_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.diffuse_frag);
//            return CreateShaderProgram(vertShader, fragShader);
//        }

//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//            uModelMatrix = GL.GetUniformLocation(Program, "ModelMatrix");
//            uNormalMatrix = GL.GetUniformLocation(Program, "NormalMatrix");
//            uLightPos = GL.GetUniformLocation(Program, "LightPos");
//            uCameraPos = GL.GetUniformLocation(Program, "CameraPos");
//            uLightMatrix = GL.GetUniformLocation(Program, "LightMatrix");
//            uProject = GL.GetUniformLocation(Program, "Project");
//            uShadowMap = GL.GetUniformLocation(Program, "ShadowMap");
//        }
//        public override void ShaderRender(CModel model)
//        {
//            RenderScene.ActiveScene.SunLight.SetUniformLightPos(uLightPos);
//            RenderScene.ActiveScene.MainCamera.SetUniformCameraPos(uCameraPos);
//            RenderScene.ActiveScene.SunLight.SetUniformLightMatrix(uLightMatrix);
//            RenderScene.ActiveScene.MainCamera.SetUniformProjMatrix(uProject);
//            GL.ActiveTexture(TextureUnit.Texture0);
//            GL.BindTexture(TextureTarget.Texture2D, CFrameBufferManager.GetTextureId(Assets.OffScreen.CShadowMap.Instance.ToString()));
//            GL.Uniform1(uShadowMap, 0);
//            model.SetUniformMVP(uMVP, RenderScene.ActiveScene.MainCamera.GetProjMatrix(), RenderScene.ActiveScene.MainCamera.GetCameraMatrix());
//            model.SetUniformModelMatrix(uModelMatrix, uNormalMatrix);
//            model.Draw();
            
//        }
//    }
//}
