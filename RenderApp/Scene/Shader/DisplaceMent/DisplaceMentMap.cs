//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using RenderApp.Scene;
//using RenderApp.Utility;
//namespace RenderApp.AssetModel
//{
//    class CDisplaceMentMap : Shader
//    {
//        #region [Singleton]

//        public CDisplaceMentMap()
//        {
//        }
//        #endregion

//        public int uLightPos;        // 光源位置
//        public int uCameraPos;       // カメラ位置
//        public int ut_2D;            // テキストテクスチャ
//        public int ut_Normal;        // 法線マップ
//        public int ut_Height;        // ヘイトマップ
//        public int uNormalMatrix;    // 法線行列
//        public int uModelMatrix;     // モデルビューマトリックス
//        public int uMVP;             // 投影行列、カメラ行列、モデル行列すべてをかけたもの
//        public int uOuter;           // テッセレーションOuter
//        public int uInner;           // テッセレーションInner

//        /// <summary>
//        /// ユニフォーム名の取得
//        /// </summary>
//        protected override void SetUniformName()
//        {
//            uLightPos = GL.GetUniformLocation(Program, "LightPos");
//            uModelMatrix = GL.GetUniformLocation(Program, "ModelMatrix");
//            uMVP = GL.GetUniformLocation(Program, "MVP");
            
//            uCameraPos = GL.GetUniformLocation(Program, "CameraPos");
            
//            ut_2D = GL.GetUniformLocation(Program, "t_2D");
//            ut_Normal = GL.GetUniformLocation(Program, "t_Normal");
//            ut_Height = GL.GetUniformLocation(Program, "t_Height");
            
//            uOuter = GL.GetUniformLocation(Program, "Outer");
//            uInner = GL.GetUniformLocation(Program, "Inner");

//        }
//        protected override int LoadShader()
//        {
//            string vertShader;
//            string tcsShader;
//            string tesShader;
//            string geomShader;
//            string fragShader;

//            vertShader = Encoding.UTF8.GetString(Properties.Resources.displacement_vert);
//            tcsShader = Encoding.UTF8.GetString(Properties.Resources.displacement_tcs);
//            tesShader = Encoding.UTF8.GetString(Properties.Resources.displacement_tes);
//            geomShader = Encoding.UTF8.GetString(Properties.Resources.displacement_geom);
//            fragShader = Encoding.UTF8.GetString(Properties.Resources.displacement_frag);
//            return CreateShaderProgram(vertShader, tcsShader, tesShader, geomShader, fragShader);

//        }

//    }
//}
