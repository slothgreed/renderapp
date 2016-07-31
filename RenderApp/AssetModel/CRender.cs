using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using RenderApp.Object;
namespace RenderApp
{
    /// <summary>
    /// MainShader
    /// </summary>
    class CRender : CShader
    {
        private static CRender m_Instance = new CRender();

        /// <summary>
        /// シングルトンインスタンスの取得
        /// </summary>
        /// <returns></returns>
        public static CRender GetInstance()
        {
            return m_Instance;
        }

        #region [メンバ変数]
        /// <summary>
        /// 光源オブジェクト
        /// </summary>
        public CLight m_Light;
        /// <summary>
        /// 投影行列
        /// </summary>
        public Matrix4 m_ProjView;
        /// <summary>
        /// カメラオブジェクト
        /// </summary>
        public CCamera m_Camera;
        /// <summary>
        /// CubeMap
        /// </summary>
        public CCubeMap m_CubeMap;
        /// <summary>
        /// 軸
        /// </summary>
        private CAxis m_Axis;
        /// <summary>
        /// 鋼材にかけるScale
        /// </summary>
        public float ModeScale = 100;//1000分の1スケール(かけているのはモデル部分だけ)
        /// <summary>
        /// 文字のテクスチャ
        /// </summary>
        public TrueTypeFont m_font;
        #region [UNIFORM構造体]
        /// <summary>
        /// ShaderのUniform構造体
        /// </summary>
        public struct UNIFORM
        {
            public int w_Height;        //Windowの高さ
            public int w_Width;         //Windowの幅
            public int Timer;           //タイマー
            public int onTimer;         // タイマー使うかどうか
            public int LightPos;        // 光源位置
            public int CameraPos;       // カメラ位置
            public int Shine;           // 輝度
            public int onTexture;       // テクスチャありかなしか
            public int primitive;       // ラインかポリゴンか
            public int t_2D;            // テキストテクスチャ
            public int t_Frame;         // フレームテクスチャ
            public int t_Cube;          // テキストテクスチャ
            public int NormalMatrix;    // 法線行列
            public int ModelMatrix;     // モデルビューマトリックス
            public int CameraMatrix;    // カメラ行列
            public int ProjMatrix;      // 投影行列
            public int MVP;             // 投影行列、カメラ行列、モデル行列すべてをかけたもの
            public int fogNear;         // 近クリップ面
            public int fogFar;          // 遠クリップ面
            public int fogStart;        // フォグのStart
            public int fogEnd;          // フォグのEnd
            public int projectorTexture;//投影機テクスチャ
            public int projectorMatrix; //投影機マトリックス
        };
        /// <summary>
        /// ShaderのUniform構造体のインスタンス
        /// </summary>
        public UNIFORM m_uniform;

        #endregion
        #endregion
        #region [Shaderの初期化関数]
        /// <summary>
        /// シェーダの初期化処理
        /// </summary>
        /// <param name="program"></param>
        public void SetShaderInit(int program)
        {
            SetProgram(program);
            SetAttribName();
            SetUniformName();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);//反時計回り
            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(1.0f, 1.0f);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(6 / 255.0f, 174 / 255.0f, 251 / 255.0f, 1.0f);

            m_Camera = new CCamera();
             m_CubeMap = new CCubeMap(RenderSystem.m_WorldMin, RenderSystem.m_WorldMax);
            m_Axis = new CAxis();

            Matrix4 lookAt = Matrix4.LookAt(new Vector3(100, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            Matrix4 persPos = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, 1, 1, 100);
            m_Light = new CLight(lookAt,persPos,"grid.png");
            //m_Light = new CLight(new Vector3(100, 150, 100), new Vector3(0, 0, 0));
        }
        protected override void SetUniformName()
        {
            m_uniform.onTimer = GL.GetUniformLocation(m_program, "onTimer");
            m_uniform.Timer = GL.GetUniformLocation(m_program, "timer");
            m_uniform.LightPos = GL.GetUniformLocation(m_program, "LightPos");
            m_uniform.Shine = GL.GetUniformLocation(m_program, "Shine");
            m_uniform.primitive = GL.GetUniformLocation(m_program, "line");
            m_uniform.t_2D = GL.GetUniformLocation(m_program, "t_2D");
            m_uniform.t_Frame = GL.GetUniformLocation(m_program, "t_Frame");
            m_uniform.t_Cube = GL.GetUniformLocation(m_program, "t_Cube");
            m_uniform.w_Height = GL.GetUniformLocation(m_program, "w_Height");
            m_uniform.w_Width = GL.GetUniformLocation(m_program, "w_Width");
            m_uniform.projectorTexture = GL.GetUniformLocation(m_program, "projectorTexture");
            m_uniform.projectorMatrix = GL.GetUniformLocation(m_program, "projectorMatrix");

            m_uniform.onTexture = GL.GetUniformLocation(m_program, "onTexture");

            m_uniform.NormalMatrix = GL.GetUniformLocation(m_program, "NormalMatrix");
            m_uniform.ModelMatrix = GL.GetUniformLocation(m_program, "ModelMatrix");
            m_uniform.CameraPos = GL.GetUniformLocation(m_program, "CameraPos");
            m_uniform.CameraMatrix = GL.GetUniformLocation(m_program, "CameraMatrix");
            m_uniform.ProjMatrix = GL.GetUniformLocation(m_program, "ProjMatrix");
            m_uniform.MVP = GL.GetUniformLocation(m_program, "MVP");

            m_uniform.fogNear = GL.GetUniformLocation(m_program, "fogNear");
            m_uniform.fogFar = GL.GetUniformLocation(m_program, "fogFar");
            m_uniform.fogStart = GL.GetUniformLocation(m_program, "fogStart");
            m_uniform.fogEnd = GL.GetUniformLocation(m_program, "fogEnd");
        }

        #endregion
        #region [文字テクスチャの初期化処理等]
        /// <summary>
        /// テキストテクスチャの生成
        /// </summary>
        public void CreateTextTexture()
        {
            //m_font = new TrueTypeFont(new Font("Times New Roman", 20f, FontStyle.Bold), true);
        }
        #endregion
        #region [シェーダに値を入れる]
        public void SetUniformOnTimer(bool timer)
        {
            if (timer)
            {
                GL.Uniform1(m_uniform.onTimer, 1);
            }
            else
            {
                GL.Uniform1(m_uniform.onTimer, 0);
            }

        }
        /// <summary>
        /// タイマー設定
        /// </summary>
        private void SetUniformTimer(float timer)
        {
            GL.Uniform1(m_uniform.Timer, timer);
        }
        /// <summary>
        /// フォグの設定
        /// </summary>
        public void SetUniformFog(float near, float far, float start, float end)
        {
            GL.Uniform1(m_uniform.fogNear, near);
            GL.Uniform1(m_uniform.fogFar, far);
            GL.Uniform1(m_uniform.fogStart, start);
            GL.Uniform1(m_uniform.fogEnd, end);
        }
        /// <summary>
        /// テクスチャの設定
        /// </summary>
        public void SetUniform2DTexture(int unitNumber)
        {
            GL.Uniform1(m_uniform.t_2D, unitNumber);
        }
        /// <summary>
        /// テクスチャの設定
        /// </summary>
        public void SetUniformCubeTexture(int unitNumber)
        {
            GL.Uniform1(m_uniform.t_Cube, unitNumber);
        }

        public enum EText
        {
            Non = 0,
            Tex2D = 1,
            TexCube = 2
        }

        /// <summary>
        /// Textureありかなしか
        /// </summary>
        public void SetUniformOnTexture(EText texture)
        {
            GL.Uniform1(m_uniform.onTexture, (int)texture);
        }
        /// <summary>
        /// ウィンドウの幅高さを入れる
        /// </summary>
        public void SetUniformWindowWH(int width, int height)
        {
            GL.Uniform1(m_uniform.w_Width, width);
            GL.Uniform1(m_uniform.w_Height, height);
        }
        /// <summary>
        ///　投影マトリクス
        /// </summary>
        public void SetUniformProjMatrix(Matrix4 proj)
        {
            GL.UniformMatrix4(m_uniform.ProjMatrix, false, ref proj);
        }
        /// <summary>
        /// 投影、カメラ、モデル全部かけたもの
        /// </summary>
        public void SetUniformMVP(Matrix4 MVP)
        {
            GL.UniformMatrix4(m_uniform.MVP, false, ref MVP);
        }
        /// <summary>
        /// ライトの情報を全て入れる
        /// </summary>
        public void SetUniformLight()
        {
            m_Light.SetUniformLightPos(m_uniform.LightPos);
            m_Light.SetUniformShine(m_uniform.Shine);
        }

        #endregion
        #region [GLの設定]
        /// <summary>
        /// 投影行列の設定
        /// </summary>
        public override void SetProjView(float aspect)
        {
            m_ProjView = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / 4.0f, aspect, 0.1f, 1000.0f);
        }
        #endregion
        #region [レンダリング処理]
        /// <summary>
        /// オペレーションに応じて変わるUniformの設定(透過率等)
        /// </summary>
        private void SetOperationUniform()
        {
            SetUniformTimer(RenderSystem.GetTimer() / 1000.0f);
            SetUniformWindowWH(RenderSystem.viewport[2], RenderSystem.viewport[3]);
            //投影行列のセット
            SetUniformProjMatrix(m_ProjView);
            //カメラ行列のセット
            m_Camera.SetUniformCameraMatrix(m_uniform.CameraMatrix);
            //カメラ位置のセット
            m_Camera.SetUniformCameraPos(m_uniform.CameraPos);
            //フォグ値のセット
            SetUniformFog(RenderSystem.m_Near, RenderSystem.m_Far, RenderSystem.m_FogStart, RenderSystem.m_FogEnd);
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        public override void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(m_program);

            
            
            SetUniform2DTexture(7);
            SetUniformCubeTexture(6);


            SetOperationUniform();
            SetUniformLight();


            //m_CubeMap.SetUniformModelMatrix(m_uniform.ModelMatrix,m_uniform.NormalMatrix);
            //m_CubeMap.SetUniformPrimitive(m_uniform.primitive);
            //SetUniformMVP(m_CubeMap.GetModelMatrix() * m_Camera.GetMatrix() * m_ProjView);
            //SetUniformOnTexture(EText.TexCube);
            //SetUniformCubeTexture(m_CubeMap.GetTexActiveId());
            //GL.ActiveTexture(TextureUnit.Texture0 + m_CubeMap.GetTexActiveId());
            //GL.BindTexture(TextureTarget.TextureCubeMap, m_CubeMap.GetTexBindId());
            //m_CubeMap.Render(m_positionId, m_normalId, m_colorId, m_texcoordId);


            m_Light.SetUniformProjectorMatrix(m_uniform.projectorMatrix);
            SetUniformMVP(m_Light.GetModelMatrix() * m_Camera.GetModelMatrix() * m_ProjView);
            m_Light.Render(m_positionId, m_normalId, m_colorId, m_texcoordId);
            //GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D,m_Light.GetTexBindId());
            //SetUniform2DTexture(2);



            //m_CubeMap.SetUniformModelMatrix(m_uniform.ModelMatrix, m_uniform.NormalMatrix);
            //m_CubeMap.SetUniformPrimitive(m_uniform.primitive);
            SetUniformMVP(m_CubeMap.GetModelMatrix() * m_Camera.GetMatrix() * m_ProjView);
            m_CubeMap.SetUniformModelMatrix(m_uniform.ModelMatrix,m_uniform.NormalMatrix);
            //SetUniformOnTexture(EText.TexCube);
            //SetUniformCubeTexture(m_CubeMap.GetTexActiveId());
            //GL.ActiveTexture(TextureUnit.Texture0 + m_CubeMap.GetTexActiveId());
            //GL.BindTexture(TextureTarget.TextureCubeMap, m_CubeMap.GetTexBindId());
            m_CubeMap.Render(m_positionId, m_normalId, m_colorId, m_texcoordId);



            m_Axis.SetUniformPrimitive(m_uniform.primitive);
            SetUniformOnTexture(EText.Non);
            m_Axis.SetUniformModelMatrix(m_uniform.ModelMatrix, m_uniform.NormalMatrix);
            m_Axis.Render(m_positionId, m_normalId, m_colorId, m_texcoordId);

            CModel model;
            for (int modelIndex = 0; modelIndex < RenderSystem.m_model.Count; modelIndex++)
            {
                model = RenderSystem.m_model[modelIndex];
                GL.ActiveTexture(TextureUnit.Texture0 + model.GetTexActiveId());
                SetUniformOnTexture(EText.Tex2D);
                GL.BindTexture(TextureTarget.Texture2D, model.GetTexBindId());
                SetUniform2DTexture(model.GetTexActiveId());
                model.SetUniformPrimitive(m_uniform.primitive);
                model.SetUniformModelMatrix(m_uniform.ModelMatrix, m_uniform.NormalMatrix);
                SetUniformMVP(model.GetModelMatrix() * m_Camera.GetMatrix() * m_ProjView);
                model.Render(m_positionId, m_normalId, m_colorId, m_texcoordId);
            }

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }
        #endregion
        #region [全体処理]
        /// <summary>
        /// すべてのSteel形状を削除
        /// </summary>
        public void DeleteModel()
        {
            foreach (CModel loop in RenderSystem.m_model)
            {
                loop.DeleteBuffer();
            }
            GL.DeleteProgram(m_program);
        }
        #endregion
    }
}
