using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using KI.Gfx.GLUtil;
using KI.Tool.Control;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    /// <summary>
    /// Viewportのヴューモデル
    /// </summary>
    public class ViewportViewModel : DockWindowViewModel
    {
        /// <summary>
        /// glContext
        /// </summary>
        private WindowsFormsHost glContext;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public ViewportViewModel(ViewModelBase parent)
            : base(parent, null, "Render App", Place.Floating)
        {
        }

        /// <summary>
        /// glContext
        /// </summary>
        public WindowsFormsHost GLContext
        {
            get
            {
                return glContext;
            }
            private set
            {
                SetValue(ref glContext, value);
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Intialize()
        {
            Viewport.Instance.OnLoaded += OnLoadedEvent;
            Viewport.Instance.OnMouseDown += OnMouseDownEvent;
            Viewport.Instance.OnMouseMove += OnMouseMoveEvent;
            Viewport.Instance.OnMouseUp += OnMouseMoveUpEvent;
            Viewport.Instance.OnMouseWheel += OnMouseWheelEvent;
            Viewport.Instance.OnKeyDown += OnKeyDownEvent;
            Viewport.Instance.OnRender += OnRenderEvent;
            Viewport.Instance.OnResize += OnResizeEvent;
            GLContext = new WindowsFormsHost()
            {
                Child = Viewport.Instance.GLControl
            };
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        public void Invalidate()
        {
            Viewport.Instance.GLControl_Paint(null, null);
        }

        #region [Viewport Method]

        /// <summary>
        /// 読み込みイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLoadedEvent(object sender, EventArgs e)
        {

            OnPropertyChanged("Loaded");
        }

        /// <summary>
        /// リサイズイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnResizeEvent(object sender, EventArgs e)
        {
            OnPropertyChanged("Resize");
        }

        /// <summary>
        /// レンダリングイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnRenderEvent(object sender, PaintEventArgs e)
        {
            OnPropertyChanged("Renderer");
        }

        /// <summary>
        /// キー押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            ControlManager.Instance.ProcessKeyInput(e);
        }

        /// <summary>
        /// マウスホイール
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnMouseWheelEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessMouseInput(e, ControlManager.MOUSE_STATE.WHEEL);
        }

        /// <summary>
        /// マウス押上げ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnMouseMoveUpEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessMouseInput(e, ControlManager.MOUSE_STATE.UP);
        }

        /// <summary>
        /// マウス移動
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnMouseMoveEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessMouseInput(e, ControlManager.MOUSE_STATE.MOVE);
        }

        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnMouseDownEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessMouseInput(e, ControlManager.MOUSE_STATE.DOWN);
        }

        #endregion
    }
}
