//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Scene;
//namespace RenderApp.Assets
//{
//    public class HeightMap : Shader
//    {
//        private int uMVP;
//        private int uHeightMap;
//        private int uRatio;
//        private int uNormalMatrix;
//        private int uOuter;
//        private int uInner;
//        public int Ratio { get; set; }
//        public int Outer { get; set; }
//        public int Inner { get; set; }
//        public HeightMap(int _ratio,int _outer,int _inner)
//        {
//            Ratio = _ratio;
//            Inner = _inner;
//            Outer = _outer;
//        }
//        #region [Shaderの初期化関数]
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            string tcsShader;
//            string tesShader;
//            string geomShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.HeightMap_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.HeightMap_frag);
//            geomShader = Encoding.UTF8.GetString(Properties.Resources.HeightMap_geom);
//            tcsShader = Encoding.UTF8.GetString(Properties.Resources.HeightMap_tcs);
//            tesShader = Encoding.UTF8.GetString(Properties.Resources.HeightMap_tes);
//            return CreateShaderProgram(vertShader, fragShader,geomShader,tcsShader,tesShader);
//        }

//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//            uHeightMap = GL.GetUniformLocation(Program, "heightmap");
//            uRatio = GL.GetUniformLocation(Program, "ratio");
//            uOuter = GL.GetUniformLocation(Program, "Outer");
//            uInner = GL.GetUniformLocation(Program, "Inner");
//        }
//        #endregion
//        #region [レンダリング処理]

//        public override void ShaderRender(CModel model)
//        {
//            GL.PatchParameter(PatchParameterInt.PatchVertices, 4);
//            GL.Uniform1(uRatio, Ratio);
//            GL.Uniform1(uOuter, Outer);
//            GL.Uniform1(uInner, Inner);
//            if(model.GetTextureNum() == 1)
//            {
//                GL.ActiveTexture(TextureUnit.Texture0);
//                GL.BindTexture(TextureTarget.Texture2D,model.GetTexture()[0].ID);
//                GL.Uniform1(uHeightMap, 0);                
//            }
//            model.SetUniformMVP(uMVP, RenderScene.ActiveScene.MainCamera.GetProjMatrix(), RenderScene.ActiveScene.MainCamera.GetCameraMatrix());
//            model.Draw();
//        }
//        public override void AnimationTimer(int timerCount, float angleCount)
//        {
//            Outer = timerCount % 80;
//            Inner = Outer;
//        }
//        #endregion 
//    }
//}
