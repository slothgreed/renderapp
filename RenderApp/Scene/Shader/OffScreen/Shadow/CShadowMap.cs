//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RenderApp.Utility;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Scene;
//namespace RenderApp.AssetModel.OffScreen
//{
//    public class CShadowMap : Shader
//    {
//        #region[シングルトン関数]
//        private static CShadowMap m_Instance = new CShadowMap();
//        private string Name = "ShadowMap";
//        private CShadowMap()
//        {

//        }
//        public static CShadowMap Instance { get { return m_Instance; } }

//        #endregion
//        public int uMVP;

//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.shadow_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.shadow_frag);
//            return CreateShaderProgram(vertShader, fragShader);
//        }
       
//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//        }

//        public override void ShaderRender(CModel model)
//        {
//            //Matrix4 matrix = Scene.Light.GetLightMatrix();
//            //model.SetUniformMVP(uMVP, RenderScene.MainCamera.GetProjMatrix(), matrix);
//            //model.Draw();
//        }
//        public override string ToString()
//        {
//            return this.Name;
//        }
//    }
//}
