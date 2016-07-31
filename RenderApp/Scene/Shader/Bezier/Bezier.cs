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
//    class Bezier : Shader
//    {
//        private int uMVP;
//        private int uSegmentNum;
//        private int uStripNum;
//        public int SegmentNum { get; set; }
//        public int StripNum { get; set; }
//        public Bezier()
//        {

//        }
//        #region [Shaderの初期化関数]
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            string tcsShader;
//            string tesShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.bezier_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.bezier_frag);
//            tcsShader = Encoding.UTF8.GetString(Properties.Resources.bezier_tcs);
//            tesShader = Encoding.UTF8.GetString(Properties.Resources.bezier_tes);
//            return CreateShaderProgram(vertShader, fragShader,tcsShader,tesShader);
//        }

//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//            uSegmentNum = GL.GetUniformLocation(Program, "SegmentNum");
//            uStripNum = GL.GetUniformLocation(Program, "StripNum");
//        }
//        public override void Initialize()
//        {
//            SegmentNum = 5;
//            StripNum = 5;
//        }
//        #endregion
//        #region [レンダリング処理]
//        public override void ShaderRender(CModel model)
//        {
//        }
//        public override void AnimationTimer(int timerCount, float angleCount)
//        {
//            SegmentNum = timerCount % 100 / 2;
//            StripNum = timerCount % 100 / 2;
//        }

//        #endregion
//    }
//}
