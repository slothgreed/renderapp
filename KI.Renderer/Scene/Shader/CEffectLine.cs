//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Scene;
//using OpenTK;
//namespace RenderApp.AssetModel
//{
//    public class CEffectLine : Shader
//    {
//        private static CEffectLine m_Instance = new CEffectLine();

//        public static CEffectLine Instance { get {return m_Instance; } }
//        private int uMVP;
//        private int uTexture;
//        private int uProjMatrix;
//        private Texture Texture;
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            string geomShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.EffectLine_vert);
//            geomShader = Encoding.UTF8.GetString(Properties.Resources.EffectLine_geom);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.EffectLine_frag);
//            return CreateShaderProgram(vertShader,geomShader, fragShader,2,2);
//        }

//        public override void Initialize()
//        {
//            Texture = new Texture("grad2.bmp");
//        }
//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//            uProjMatrix = GL.GetUniformLocation(Program, "ProjMatrix");
//            uTexture = GL.GetUniformLocation(Program, "texture_2D");
//        }

//        public override void ShaderRender(CModel model)
//        {
//            GL.ActiveTexture(TextureUnit.Texture0);
//            GL.Uniform1(uTexture,0 );
//            GL.BindTexture(TextureTarget.Texture2D, Texture.ID);

//            Matrix4 proj = RenderScene.ActiveScene.MainCamera.GetProjMatrix();
//            GL.UniformMatrix4(uProjMatrix, false,ref proj);
//            model.SetUniformMVP(uMVP, Matrix4.Identity, RenderScene.ActiveScene.MainCamera.GetCameraMatrix());
//            model.Draw();
//            GL.BindTexture(TextureTarget.Texture2D, 0);

//        }
//    }
//}
