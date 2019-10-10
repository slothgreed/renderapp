using System;
using System.Drawing;
using System.Windows.Forms;
using KI.Foundation.Core;
using OpenTK;
using OpenTK.Graphics;
using KI.Foundation.Controller;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx
{
    /// <summary>
    /// 読み込み後イベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnLoadedHandler(object sender, EventArgs e);

    /// <summary>
    /// マウス押下イベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnMouseDownHandler(object sender, KIMouseEventArgs e);

    /// <summary>
    /// マウス移動イベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnMouseMoveHandler(object sender, KIMouseEventArgs e);

    /// <summary>
    /// マウス押上げイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnMouseUpHandler(object sender, KIMouseEventArgs e);

    /// <summary>
    /// マウスホイールイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnMouseWheelHandler(object sender, KIMouseEventArgs e);

    /// <summary>
    /// マウスクリックイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnMouseClickHandler(object sender, KIMouseEventArgs e);
    /// <summary>
    /// マウスダブルクリックイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnMouseDoubleClickHandler(object sender, KIMouseEventArgs e);

    /// <summary>
    /// レンダリングイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnRenderHandler(object sender, PaintEventArgs e);

    /// <summary>
    /// リサイズイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnResizeHandler(object sender, EventArgs e);

    /// <summary>
    /// ドラッグドロップイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnDragDropHandler(object sender, DragEventArgs e);

    /// <summary>
    /// ドラッグ中にマウスがViewportに入ってきたイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnDragEnterHandler(object sender, DragEventArgs e);

    /// <summary>
    /// ドラッグ中にマウスがViewportから出たイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnDragLeaveHandler(object sender, EventArgs e);

    /// <summary>
    /// ドラッグ中にマウスが移動したイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnDragOverHandler(object sender, DragEventArgs e);

    /// <summary>
    /// キーが押された時のイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnKeyDownHandler(object sender, KeyEventArgs e);

    /// <summary>
    /// 描画用のGlobal変数を保持するクラス
    /// </summary>
    public class Viewport
    {
        #region [member]

        /// <summary>
        /// レンダリング中か
        /// </summary>
        private bool nowRender;

        /// <summary>
        /// 起動後か
        /// </summary>
        private bool appstartUp = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Viewport()
        {
            Initialize();
        }

        /// <summary>
        /// 読み込み後イベント
        /// </summary>
        public event OnLoadedHandler OnLoaded;
        
        /// <summary>
        /// マウス押下イベント
        /// </summary>
        public event OnMouseDownHandler OnMouseDown;

        /// <summary>
        /// マウス移動イベント
        /// </summary>
        public event OnMouseMoveHandler OnMouseMove;
        
        /// <summary>
        /// マウス押上げイベント
        /// </summary>
        public event OnMouseUpHandler OnMouseUp;

        /// <summary>
        /// マウスダブルクリックイベント
        /// </summary>
        public event OnMouseClickHandler OnMouseClick;

        /// <summary>
        /// マウスダブルクリックイベント
        /// </summary>
        public event OnMouseDoubleClickHandler OnMouseDoubleClick;

        /// <summary>
        /// マウスホイールイベント
        /// </summary>
        public event OnMouseWheelHandler OnMouseWheel;

        /// <summary>
        /// キーダウンイベント
        /// </summary>
        public event OnKeyDownHandler OnKeyDown;

        /// <summary>
        /// レンダリングイベント
        /// </summary>
        public event OnRenderHandler OnRender;

        /// <summary>
        /// リサイズイベント
        /// </summary>
        public event OnResizeHandler OnResize;

        /// <summary>
        /// ドラッグ中にマウスがViewportに入ってきたイベント
        /// </summary>
        public event OnDragEnterHandler OnDragEnter;

        /// <summary>
        /// ドラッグ中にマウスがViewportから出たイベント
        /// </summary>
        public event OnDragLeaveHandler OnDragLeave;

        /// <summary>
        /// ドラッグ中にマウスが移動したイベント
        /// </summary>
        public event OnDragOverHandler OnDragOver;

        #endregion
        #region [initialize method]

        /// <summary>
        /// インスタンス
        /// </summary>
        public static Viewport Instance { get; } = new Viewport();

        /// <summary>
        /// レンダリング時間(mm)
        /// </summary>
        public int RenderingMillSec { get; private set; }

        /// <summary>
        /// GLControlのゲッタ
        /// </summary>
        public GLControl GLControl { get; private set; }

        public int[] ViewportRect
        {
            get
            {
                return DeviceContext.Instance.ViewportRect;
            }
        }

        #endregion
        #region [context event]

        /// <summary>
        /// GLControlの描画時に実行される
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        public void GLControl_Paint(object sender, PaintEventArgs e)
        {
            if (!appstartUp)
                return;

            if (nowRender)
                return;

            DeviceContext.Instance.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (OnRender != null)
            {
                OnRender(sender, e);
            }

            nowRender = true;
            GLControl.SwapBuffers();
            nowRender = false;
        }
        #endregion
        #region [mouse event]

        /// <summary>
        /// ドラッグイベント
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        public void GLControl_DragOver(object sender, DragEventArgs e)
        {
            if (OnDragOver != null)
            {
                OnDragOver(sender, e);
            }

            GLControl_Paint(null, null);
        }

        /// <summary>
        /// ドラッグ中に離れた
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        public void GLControl_DragLeave(object sender, EventArgs e)
        {
            if (OnDragLeave != null)
            {
                OnDragLeave(sender, e);
            }

            GLControl_Paint(null, null);
        }

        /// <summary>
        /// ドラッグ中にviewportに入った
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        public void GLControl_DragEnter(object sender, DragEventArgs e)
        {
            if (OnDragEnter != null)
            {
                OnDragEnter(sender, e);
            }

            GLControl_Paint(null, null);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            GLControl.Load -= GLControl_Load;
            GLControl.MouseDown -= GLControl_MouseDown;
            GLControl.MouseMove -= GLControl_MouseMove;
            GLControl.MouseUp -= GLControl_MouseUp;
            GLControl.MouseWheel -= GLControl_MouseWheel;
            GLControl.MouseClick -= GLControl_MouseClick;
            GLControl.MouseDoubleClick -= GLControl_MouseDoubleClick;
            GLControl.KeyDown -= GLControl_KeyDown;
            GLControl.Paint -= GLControl_Paint;
            GLControl.Resize -= GLControl_Resize;
            GLControl = null;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            GraphicsMode mode = new GraphicsMode(
                                                 //ColorFormat構造体を用いて、各色のピクセル当たりのビット数(カラーバッファのサイズ)
                                                 GraphicsMode.Default.ColorFormat,
                                                 //デプスバッファのサイズ
                                                 GraphicsMode.Default.Depth,
                                                 //ステンシルバッファのサイズ
                                                 8, //GraphicsMode.Default.Stencil,
                                                    //AA(AntiAliasing)のサイズ x4 x8などの数字
                                                 8, //GraphicsMode.Default.Samples,
                                                    //ColorFormat構造体を用いて、アキュムレーションバッファのサイズ
                                                 GraphicsMode.Default.AccumulatorFormat,
                                                 //バッファリングに使うフレームバッファの数 1(シングルバッファリング),2(ダブル-),3(トリプル-)
                                                 GraphicsMode.Default.Buffers,
                                                 //ステレオ投影をするかどうか
                                                 GraphicsMode.Default.Stereo);
            GLControl = new GLControl(mode);
            GLControl.Load += GLControl_Load;
            GLControl.MouseDown += GLControl_MouseDown;
            GLControl.MouseMove += GLControl_MouseMove;
            GLControl.MouseUp += GLControl_MouseUp;
            GLControl.MouseWheel += GLControl_MouseWheel;
            GLControl.MouseClick += GLControl_MouseClick;
            GLControl.MouseDoubleClick += GLControl_MouseDoubleClick;
            GLControl.KeyDown += GLControl_KeyDown;
            GLControl.Paint += GLControl_Paint;
            GLControl.Resize += GLControl_Resize;
            GLControl.DragEnter += GLControl_DragEnter;
            GLControl.DragLeave += GLControl_DragLeave;
            GLControl.DragOver += GLControl_DragOver;
            GLControl.AllowDrop = true;
        }


        /// <summary>
        /// コントロールにフォーカスがあるときにキーが押されると発生します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GLControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (OnKeyDown != null)
            {
                OnKeyDown(sender, e);
            }

            GLControl_Paint(null, null);
        }

        /// <summary>
        /// GLControlの起動時に実行される
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void GLControl_Load(object sender, EventArgs e)
        {
            DeviceContext.Instance.Initialize(GLControl.Size.Width, GLControl.Size.Height);
            appstartUp = true;
            Logger.GLLog(Logger.LogLevel.Error);
            if (OnLoaded != null)
            {
                OnLoaded(sender, e);
            }
        }

        /// <summary>
        /// Loadより先に呼ばれる
        /// GLControlのサイズ変更時に実行される
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void GLControl_Resize(object sender, EventArgs e)
        {
            if (GLControl.Size.Width == 0 || GLControl.Size.Height == 0)
            {
                GLControl.Size = new Size(128, 128);
            }

            if (appstartUp)
            {
                DeviceContext.Instance.SizeChanged(GLControl.Size.Width, GLControl.Size.Height);
                if (OnResize != null)
                {
                    OnResize(sender, e);
                }

                Logger.GLLog(Logger.LogLevel.Error);
                GLControl_Paint(null, null);
            }
        }

        /// <summary>
        /// マウスホイール
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void GLControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (OnMouseWheel != null)
            {
                OnMouseWheel(sender, new KIMouseEventArgs(e, MOUSE_STATE.WHEEL));
            }

            GLControl_Paint(null, null);
        }

        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void GLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (OnMouseDown != null)
            {
                OnMouseDown(sender, new KIMouseEventArgs(e, MOUSE_STATE.DOWN));
            }

            Logger.Log(Logger.LogLevel.Debug, "Down");
            GLControl_Paint(null, null);
        }

        /// <summary>
        /// マウスクリック
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void GLControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (OnMouseClick != null)
            {
                OnMouseClick(sender, new KIMouseEventArgs(e, MOUSE_STATE.DOWN));
            }

            Logger.Log(Logger.LogLevel.Debug, "Click");
            GLControl_Paint(null, null);
        }

        /// <summary>
        /// マウスダブルクリック
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void GLControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OnMouseDoubleClick != null)
            {
                OnMouseDoubleClick(sender, new KIMouseEventArgs(e, MOUSE_STATE.DOWN));
            }

            Logger.Log(Logger.LogLevel.Debug, "DoubleClick");
            GLControl_Paint(null, null);
        }

        /// <summary>
        /// マウス押上げ
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void GLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (OnMouseUp != null)
            {
                OnMouseUp(sender, new KIMouseEventArgs(e, MOUSE_STATE.UP));
            }

            GLControl_Paint(null, null);
        }

        /// <summary>
        /// マウス移動
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void GLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!appstartUp)
            {
                return;
            }

            GLControl.Focus();
            if (OnMouseMove != null)
            {
                OnMouseMove(sender, new KIMouseEventArgs(e, MOUSE_STATE.MOVE));
            }

            GLControl_Paint(null, null);
        }
        #endregion

        #region [Main Window Event]

        /// <summary>
        /// スクリーンショット
        /// </summary>
        /// <returns>ビットマップデータ</returns>
        private Bitmap GetColorBufferData()
        {
            if (GraphicsContext.CurrentContext == null)
                throw new GraphicsContextMissingException();

            Rectangle r = new Rectangle(GLControl.Location, GLControl.Size);
            Bitmap bmp = new Bitmap(GLControl.Width, GLControl.Height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(r, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            DeviceContext.Instance.ReadPixel(data);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }
        #endregion
    }
}
