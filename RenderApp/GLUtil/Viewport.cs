using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using RenderApp.AssetModel;
using RenderApp.Utility;
using System.IO;
using System.Windows.Forms;
using System.Timers;
using RenderApp.Control;
namespace RenderApp.GLUtil
{
    public delegate void CreateViewportHandler();
    /// <summary>
    /// 描画用のGlobal変数を保持するクラス
    /// </summary>
    public class Viewport
    {
        private static Viewport m_Instance;
        public static Viewport Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new Viewport();
                }
                return m_Instance;
            }
        }
        private RenderSystem _renderSystem;
        public RenderSystem RenderSystem
        {
            get
            {
                if (_renderSystem == null)
                {
                    _renderSystem = new RenderSystem(Width, Height);
                }
                return _renderSystem;
            }
        }
        #region [member]
        /// <summary>
        /// タイマー変数
        /// </summary>
        public CTimer m_AnimationTimer = null;
        /// <summary>
        /// glControl
        /// </summary>
        private GLControl m_glControl;
        /// <summary>
        /// レンダリング中か
        /// </summary>
        private bool m_NowRender;
        /// <summary>
        /// 起動後か
        /// </summary>
        private bool m_AppstartUp = false;
        /// <summary>
        /// ストップウォッチ
        /// </summary>
        private Stopwatch m_StopWatch = null;
        /// <summary>
        /// レンダリング時間(mm)
        /// </summary>
        public int RenderingMillSec = 0;
        /// <summary>
        /// glControlのゲッタ
        /// </summary>
        public GLControl glControl { get { return m_glControl; } }

        public event CreateViewportHandler OnCreateViewportEvent;
        #endregion

        public int Width
        {
            get
            {
               return glControl.Width;
            }
        }
        public int Height
        {
            get
            {
                return glControl.Height;
            }
        }
        #region [initialize method]
        private Viewport()
        {
            CreateGLWindow();
        }
        private void CreateGLWindow()
        {
            GraphicsMode mode = new GraphicsMode(
                //ColorFormat構造体を用いて、各色のピクセル当たりのビット数(カラーバッファのサイズ)
                                                 GraphicsMode.Default.ColorFormat,
                //デプスバッファのサイズ
                                                 GraphicsMode.Default.Depth,
                //ステンシルバッファのサイズ
                                                 8,//GraphicsMode.Default.Stencil,
                //AA(AntiAliasing)のサイズ x4 x8などの数字
                                                 8,//GraphicsMode.Default.Samples,
                //ColorFormat構造体を用いて、アキュムレーションバッファのサイズ
                                                 GraphicsMode.Default.AccumulatorFormat,
                //バッファリングに使うフレームバッファの数 1(シングルバッファリング),2(ダブル-),3(トリプル-)
                                                 GraphicsMode.Default.Buffers,
                //ステレオ投影をするかどうか
                                                 GraphicsMode.Default.Stereo
                                                 );
            m_glControl = new GLControl(mode);
            m_glControl.Load += glControl_Load;
            m_glControl.MouseDown += glControl_MouseDown;
            m_glControl.MouseMove += glControl_MouseMove;
            m_glControl.MouseUp += glControl_MouseUp;
            m_glControl.MouseWheel += glControl_MouseWheel;
            m_glControl.Paint += glControl_Paint;
            m_glControl.Resize += glControl_Resize;

        }


        public void Initialize()
        {
            //CFrameBufferManager.Initialize(m_glControl.Width, m_glControl.Height);
            //ShaderManager.Initialize();
            m_StopWatch = new Stopwatch();
            
        }
        #endregion
        #region [context event]
        //glControlの起動時に実行される。
        private void glControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0,0,0,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.AlphaTest);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.FrontFace(FrontFaceDirection.Ccw);//反時計回り
            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.Enable(EnableCap.Texture2D);
            GL.PolygonOffset(1.0f, 1.0f);
            GL.CullFace(CullFaceMode.Back);
            GL.Viewport(0, 0, m_glControl.Size.Width, m_glControl.Size.Height);
            Scene.Create("MainScene");
            Scene.ActiveScene.Initialize();
            m_AppstartUp = true;
            Output.GLError();
            OnCreateViewportEvent();
        }
        //Loadより先に呼ばれる
        //glControlのサイズ変更時に実行される。
        private void glControl_Resize(object sender, EventArgs e)
        {
            if (m_AppstartUp)
            {
                Scene.ActiveScene.MainCamera.SetProjMatrix((float)m_glControl.Size.Width / m_glControl.Size.Height);
                GL.Viewport(0, 0, m_glControl.Size.Width, m_glControl.Size.Height);
                RenderSystem.SizeChanged(m_glControl.Size.Width, m_glControl.Size.Height);
                Output.GLError();
                glControl_Paint(null, null);
            }

        }

        //glControlの描画時に実行される。
        public void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!m_AppstartUp)
            {
                return;
            }
            if (m_NowRender)
            {
                return;
            }
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            RenderSystem.Render();
            m_NowRender = true;
            m_glControl.SwapBuffers();
            m_NowRender = false;

        }
        #endregion
        #region [mouse event]


        private void glControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessInput(e, ControlManager.MOUSE_STATE.WHEEL);
            glControl_Paint(null, null);
        }

        private void glControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessInput(e, ControlManager.MOUSE_STATE.DOWN);
            glControl_Paint(null, null);
        }
        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            ControlManager.Instance.ProcessInput(e, ControlManager.MOUSE_STATE.UP);
            glControl_Paint(null, null);
        }
        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!m_AppstartUp)
            {
                return;
            }
            m_glControl.Focus();
            ControlManager.Instance.ProcessInput(e, ControlManager.MOUSE_STATE.MOVE);
            if (e.Button != MouseButtons.None)
            {
                glControl_Paint(null, null);
            }
        }
        #endregion
        #region [timer event]
        public void StartTimer()
        {
            if(m_AnimationTimer==null)
            {
                m_AnimationTimer = new CTimer(1, OnAnimationTimer);

            }
            m_AnimationTimer.Start();

        }
        public void StopTimer()
        {
            if(m_AnimationTimer == null)
            {
                return;
            }
            m_AnimationTimer.Stop();
            glControl_Paint(null, null);
        }

        public void OnAnimationTimer(object source, EventArgs e)
        {
            
            m_AnimationTimer.AddTimer();
            float Length = 20;
            float angle = m_AnimationTimer.RadianCount;
            Vector3 lightPos = new Vector3((float)Math.Sin(angle) * Length,25, (float)Math.Cos(angle) * Length);
            //GlobalModel.Light.SetPosition(lightPos);
            glControl_Paint(null, null);
        }
        #endregion
        #region [Main Window Event]
       
       
        public void Closed()
        {
            if(m_AnimationTimer != null)
            {
                m_AnimationTimer.Stop();

            }
        }
        
     
       

        #endregion

    }
}
