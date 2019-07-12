using KI.Gfx.GLUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using KI.Foundation.Controller;

namespace KI.Presenter.ViewModel
{

    /// <summary>
    /// 初期化イベント
    /// </summary>
    public delegate void OnInitializedHandler(object sender, EventArgs e);

    public class ViewportViewModelBase : ViewModelBase
    {
        public event OnInitializedHandler OnInitialized;

        private WindowsFormsHost _glContext;
        public WindowsFormsHost GLContext
        {
            get
            {
                if (_glContext == null)
                {
                    _glContext = new WindowsFormsHost()
                    {
                        Child = Viewport.Instance.GLControl
                    };
                }

                return _glContext;
            }
        }

        public ViewportViewModelBase(ViewModelBase parent)
            : base(parent)
        {
            Viewport.Instance.OnLoaded += OnLoadedEvent;
            Viewport.Instance.OnMouseDown += OnMouseDownEvent;
            Viewport.Instance.OnMouseMove += OnMouseMoveEvent;
            Viewport.Instance.OnMouseUp += OnMouseMoveUpEvent;
            Viewport.Instance.OnMouseWheel += OnMouseWheelEvent;
            Viewport.Instance.OnKeyDown += OnKeyDownEvent;
            Viewport.Instance.OnRender += OnRenderEvent;
            Viewport.Instance.OnResize += OnResizeEvent;
        }

        /// <summary>
        /// 読み込みイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnLoadedEvent(object sender, EventArgs e)
        {
            InitializeDeviceContext();
            InitializeScene();
            InitializeRenderer();

            OnInitialized?.Invoke(this, null);
        }

        /// <summary>
        /// レンダリングイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        protected virtual void OnRenderEvent(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// リサイズイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        protected virtual void OnResizeEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// デバイスコンテキストの初期化
        /// </summary>
        protected virtual void InitializeDeviceContext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// レンダラの初期化
        /// </summary>
        protected virtual void InitializeRenderer()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// シーンの初期化
        /// </summary>
        protected virtual void InitializeScene()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        protected virtual void ProcessMouseInput(KIMouseEventArgs mouse)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// キー入力
        /// </summary>
        /// <param name="e">キー情報</param>
        protected virtual void ProcessKeyInput(KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// キー押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            ProcessKeyInput(e);
        }

        /// <summary>
        /// マウスホイール
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnMouseWheelEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e);
        }

        /// <summary>
        /// マウス押上げ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnMouseMoveUpEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e);
        }

        /// <summary>
        /// マウス移動
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnMouseMoveEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e);
        }

        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnMouseDownEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e);
        }
    }
}
