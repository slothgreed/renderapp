//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK.Graphics.OpenGL;
//namespace RenderApp.Render_System.PostEffect
//{
//    class DOF : PostEffect
//    {
//        private static DOF m_Instance = new DOF();
//        public static DOF GetInstance()
//        {
//            return m_Instance;
//        }

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
//            //m_uniform.MVP = GL.GetUniformLocation(mProgram, "MVP");
//            //m_uniform.ModelMatrix = GL.GetUniformLocation(mProgram, "ModelMatrix");
//            //m_uniform.NormalMatrix = GL.GetUniformLocation(mProgram, "NormalMatrix");
//            //m_uniform.LightPos = GL.GetUniformLocation(mProgram, "LightPos");
//            //m_uniform.CameraPos = GL.GetUniformLocation(mProgram, "CameraPos");
//        }

//    }
//}
