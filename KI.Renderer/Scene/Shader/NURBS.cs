//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Scene;
//namespace RenderApp.AssetModel
//{
//    public class NURBS : Shader
//    {
//        private int uMVP;
//        private int uOuter;
//        private int uInner;
//        private int uNormalMatrix;
//        public int Outer { get; set; }
//        public int Inner { get; set; }
//        public NURBS()
//        {

//        }
//        #region [Shaderの初期化関数]
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            string tcsShader;
//            string tesShader;
//            string geomShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.nurbs_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.nurbs_frag);
//            geomShader = Encoding.UTF8.GetString(Properties.Resources.nurbs_geom);
//            tcsShader = Encoding.UTF8.GetString(Properties.Resources.nurbs_tcs);
//            tesShader = Encoding.UTF8.GetString(Properties.Resources.nurbs_tes);
//            return CreateShaderProgram(vertShader, fragShader,geomShader,tcsShader,tesShader);
//        }

//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//            uOuter = GL.GetUniformLocation(Program, "Outer");
//            uInner = GL.GetUniformLocation(Program, "Inner");
//            uNormalMatrix = GL.GetUniformLocation(Program, "NormalMatrix");
//        }
//        public override void Initialize()
//        {
//            Outer = 5;
//            Inner = 5;
//        }
//        #endregion
//        #region [レンダリング処理]

//        public override void AnimationTimer(int timerCount, float angleCount)
//        {
//            Outer = timerCount % 50 / 2;
//            Inner = Outer;
//        }
//        #endregion 
//    }
//}
