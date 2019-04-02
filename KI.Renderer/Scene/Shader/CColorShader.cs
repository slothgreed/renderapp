//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using RenderApp.Utility;
//using RenderApp.Scene;
//namespace RenderApp.AssetModel
//{
//    /// <summary>
//    /// 原色レンダリング用
//    /// </summary>
//    class CColorShader : Shader
//    {
//         #region [Singleton]
//        private static CColorShader m_Instance = new CColorShader();
//        private int uMVP;
//        private CColorShader()
//        {
//        }
//        public static CColorShader GetInstance()
//        {
//            return m_Instance;
//        }
//        #endregion

//        public CLine m_Line;
//        public CPoint m_Point;
//        public CAxis m_Axis;

//        /// <summary>
//        /// ユニフォーム名の取得
//        /// </summary>
//        protected override void SetUniformName()
//        {
//            uMVP = GL.GetUniformLocation(Program, "MVP");
//        }
//        protected override int LoadShader()
//        {

//            string vertShader;
//            string fragShader;
//            vertShader = Encoding.UTF8.GetString(Properties.Resources.line_vert);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.line_frag);
//            return CreateShaderProgram(vertShader, fragShader);
//        }

//        public override void Initialize()
//        {
//            m_Axis = new CAxis(Global.WorldMin, Global.WorldMax);
//        }

//        public void SetLine(List<Vector3> position,List<Vector3> color)
//        {
//            m_Line = new CLine(position, color);
//        }

//        public void SetPoint(List<Vector3> position,List<Vector3>color)
//        {
//            m_Point = new CPoint(position, color);
//        }
//        ///// <summary>
//        ///// 形状の描画処理
//        ///// </summary>
//        //protected override void Render()
//        //{

//        //    if (m_Line != null)
//        //    {
//        //        m_Line.SetUniformMVP(m_uniform.MVP, RenderObject.Camera.GetProjMatrix(), RenderObject.Camera.GetCameraMatrix());
//        //        m_Line.Render(this);
//        //    }
//        //    if (m_Point != null)
//        //    {
//        //        m_Point.SetUniformMVP(m_uniform.MVP, RenderObject.Camera.GetProjMatrix(), RenderObject.Camera.GetCameraMatrix());
//        //        m_Point.Render(this);
//        //    }
//        //    m_Axis.SetUniformMVP(m_uniform.MVP, RenderObject.Camera.GetProjMatrix(), RenderObject.Camera.GetCameraMatrix());
//        //    m_Axis.Render(this);
//        //}
//        public override void ShaderRender(CModel model)
//        {
//        }

//    }
//}
