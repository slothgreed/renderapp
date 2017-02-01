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
using RenderApp.RA_Control;
using RenderApp.Render_System;
using KI.Foundation.Utility;
using KI.Foundation.Core;
using System.Drawing;

namespace RenderApp.GLUtil
{
    public delegate void OnLoadedHandler(object sender,EventArgs e);
    public delegate void OnMouseDownHandler(object sender, MouseEventArgs e);
    public delegate void OnMouseMoveHandler(object sender, MouseEventArgs e);
    public delegate void OnMouseUpHandler(object sender, MouseEventArgs e);
    public delegate void OnMouseWheelHandler(object sender, MouseEventArgs e);
    public delegate void OnRenderHandler(object sender, PaintEventArgs e);
    public delegate void OnResizeHandler(object sender,EventArgs e);

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

        #region [member]
        /// <summary>
        /// タイマー変数
        /// </summary>
        public KITimer m_AnimationTimer = null;
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

        public event OnLoadedHandler OnLoaded;
        public event OnMouseDownHandler OnMouseDown;
        public event OnMouseMoveHandler OnMouseMove;
        public event OnMouseUpHandler OnMouseUp;
        public event OnMouseWheelHandler OnMouseWheel;
        public event OnRenderHandler OnRender;
        public event OnResizeHandler OnResize;
        
        #endregion
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
            m_StopWatch = new Stopwatch();
        }
        #endregion
        #region [context event]
        //glControlの起動時に実行される。
        private void glControl_Load(object sender, EventArgs e)
        {
            DeviceContext.Instance.Initialize(m_glControl.Size.Width,m_glControl.Size.Height);
            m_AppstartUp = true;
            Logger.GLLog(Logger.LogLevel.Error);
            if(OnLoaded != null)
            {
                OnLoaded(sender, e);
            }
        }
        //Loadより先に呼ばれる
        //glControlのサイズ変更時に実行される。
        private void glControl_Resize(object sender, EventArgs e)
        {
            if(m_glControl.Size.Width == 0 || m_glControl.Size.Height == 0)
            {
                m_glControl.Size = new Size(128, 128);
            }
            if (m_AppstartUp)
            {
                DeviceContext.Instance.SizeChanged(m_glControl.Size.Width, m_glControl.Size.Height);
                if (OnResize != null)
                {
                    OnResize(sender, e);
                }
                Logger.GLLog(Logger.LogLevel.Error);
                glControl_Paint(null, null);
            }

        }

        //glControlの描画時に実行される。
        public void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (!m_AppstartUp)
            {
                return;
            }
            if (m_NowRender)
            {
                return;
            }
            DeviceContext.Instance.Clear();

            if(OnRender != null)
            {
                OnRender(sender, e);
            }
            m_NowRender = true;
            m_glControl.SwapBuffers();
            m_NowRender = false;

        }
        #endregion
        #region [mouse event]
        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if(OnMouseWheel != null)
            {
                OnMouseWheel(sender, e);
            }
            glControl_Paint(null, null);
        }

        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (OnMouseDown != null)
            {
                OnMouseDown(sender, e);
            } 
            glControl_Paint(null, null);
        }
        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (OnMouseUp != null)
            {
                OnMouseUp(sender, e);
            } 
            glControl_Paint(null, null);
        }
        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_AppstartUp)
            {
                return;
            }
            m_glControl.Focus();
            if(OnMouseMove != null)
            {
                OnMouseMove(sender, e);
            }
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
                m_AnimationTimer = new KITimer(1, OnAnimationTimer);

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

        /// <summary>
        /// スクリーンショット
        /// </summary>
        /// <returns></returns>
        private Bitmap GetColorBufferData()
        {
            if (GraphicsContext.CurrentContext == null)
                throw new GraphicsContextMissingException();

            System.Drawing.Rectangle r = new System.Drawing.Rectangle(glControl.Location, glControl.Size);
            Bitmap bmp = new Bitmap(glControl.Width, glControl.Height);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(r, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            DeviceContext.Instance.ReadPixel(data);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }
        #endregion

    }
}
