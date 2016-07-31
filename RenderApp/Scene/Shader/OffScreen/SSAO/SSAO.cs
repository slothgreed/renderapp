//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Scene;
//namespace RenderApp.AssetModel.OffScreen
//{
//    public class SSAO
//    {

//        private readonly string FrameName = "SSAO";
//        private int urender_2D;
//        private int uposit_2D;
//        private int uMain_2D;
//        private int m_TextureId;
//        /// <summary>
//        /// 重み
//        /// </summary>
//        private int m_SampleId;
//        private float[] m_Sample = new float[1200];
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.ssao_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.ssao_frag);
            
//            return CreateShaderProgram(vertShader, fragShader);
//        }
        

//        public override void Initialize()
//        {

//            float radius = 0.02f;//10.0f / m_ViewPort.w;
//            Random rand = new Random();
//            for (int i = 0; i < m_Sample.Length; i+=3)
//            {
//                float r  = (float)(radius * rand.NextDouble());
//                float t  = (float)(Math.PI * 2 * rand.NextDouble());
//                float cp = (float)(2 * rand.NextDouble()) - 1.0f;
//                float sp = (float)Math.Sqrt(1.0f - cp * cp);
//                float ct = (float)Math.Cos(t);
//                float st = (float)Math.Sin(t);
//                m_Sample[i    ] = r * sp * ct;
//                m_Sample[i + 1] = r * sp * st;
//                m_Sample[i + 2] = r * cp;

//            }
//        }
//        protected override void SetUniformName()
//        {
//            m_SampleId = GL.GetUniformLocation(Program, "Sample");
//            uposit_2D = GL.GetUniformLocation(Program, "posit_2D");
//            uMain_2D = GL.GetUniformLocation(Program, "main_2D");
//        }
//        public void SetTexture(int textureId)
//        {
//            m_TextureId = textureId;
//        }
        
//    }
//}
