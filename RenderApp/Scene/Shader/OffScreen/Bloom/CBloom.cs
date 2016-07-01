//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK.Graphics.OpenGL;
//using OpenTK;

//using RenderApp.Scene;
//namespace RenderApp.Assets.OffScreen
//{
//    class CBloom : Shader
//    {
//        private int HorizonMode;
//        /// <summary>
//        /// 水平方向
//        /// </summary>
//        private int uHorizon;
//        /// <summary>
//        /// 横縦のスクリーンの大きさ
//        /// </summary>
//        private int uScale;
//        /// <summary>
//        /// 重み
//        /// </summary>
//        private int uWeight;
//        private int urender_2D;
//        private int uMVP;
//        private float[] m_Weight = new float[5];

//        public int m_TextureId { get; set; }
//        public int Scale { get; set; }
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.bloom_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.bloom_frag);
//            return CreateShaderProgram(vertShader, fragShader);
//        }
//        public override void Initialize()
//        {
//            float sigma = 100;
//            float kei = (float)(1.0f / (2 * Math.PI * sigma * sigma));
//            float sum = 0;
//            float rad = 0;
//            for (int i = 0; i < m_Weight.Length; i++)
//            {
//                rad = (1.0f + 2.0f * i) * (1.0f + 2.0f * i);
//                m_Weight[i] = (float)Math.Exp(-0.5 * (rad / sigma));
//                if (i != 0)
//                {
//                    sum += m_Weight[i] * 2;
//                }
//            }
//            for (int i = 0; i < m_Weight.Length; i++)
//            {
//                m_Weight[i] /= (sum);
//            }
//        }

//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//            uHorizon = GL.GetUniformLocation(Program, "horizon");
//            uScale = GL.GetUniformLocation(Program, "scale");
//            uWeight = GL.GetUniformLocation(Program, "Weight");
//            urender_2D = GL.GetUniformLocation(Program, "render_2D");
//        }
//        /// <summary>
//        /// 縦か横か
//        /// </summary>
//        public void SetHorizon(bool horizon)
//        {
//            if(horizon)
//            {
//                HorizonMode = 1;
//            }
//            else
//            {
//                HorizonMode = 0;
//            }
//        }
        
//        }
//    }
//}
