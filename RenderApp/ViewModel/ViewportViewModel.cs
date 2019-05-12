using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Tool;
using KI.Tool.Controller;
using KI.UI.ViewModel;
using RenderApp.Tool;
using RenderApp.Tool.Controller;

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
            SelectPointController SelectPointController = new SelectPointController();
            SelectPointController.PointSelected += SelectPointController_PointSelected;

            SelectLineController SelectLineController = new SelectLineController();
            SelectLineController.LineSelected += SelectPointController_PointSelected;

            SelectTriangleController SelectTriangleController = new SelectTriangleController();
            SelectTriangleController.TriangleSelected += SelectPointController_PointSelected;

            DijkstraController DijkstraController = new DijkstraController();
            GeodesicDistanceController GeodesicDistanceController = new GeodesicDistanceController();
            EdgeFlipsController EdgeFlipsController = new EdgeFlipsController();

            Controllers.Add(CONTROL_MODE.SelectPoint, SelectPointController);
            Controllers.Add(CONTROL_MODE.SelectLine, SelectLineController);
            Controllers.Add(CONTROL_MODE.SelectTriangle, SelectTriangleController);

            Controllers.Add(CONTROL_MODE.Dijkstra, DijkstraController);
            Controllers.Add(CONTROL_MODE.Geodesic, GeodesicDistanceController);
            Controllers.Add(CONTROL_MODE.EdgeFlips, EdgeFlipsController);
        }

        /// <summary>
        /// カメラコントローラ
        /// </summary>
        private ControllerBase cameraController = new CameraController();

        /// <summary>
        /// コントローラの現在のモード
        /// </summary>
        private CONTROL_MODE mode = CONTROL_MODE.SelectTriangle;


        /// <summary>
        /// コントロールリスト
        /// </summary>
        public Dictionary<CONTROL_MODE, ControllerBase> Controllers { get; set; } = new Dictionary<CONTROL_MODE, ControllerBase>();

        /// <summary>
        /// コントローラの現在のモード
        /// </summary>
        public CONTROL_MODE Mode
        {
            get
            {
                return mode;
            }

            set
            {
                Controllers[mode].UnBinding();
                mode = value;
                Controllers[mode].Binding(null);
            }
        }

        /// <summary>
        /// キー入力
        /// </summary>
        /// <param name="e"></param>
        public void ProcessKeyInput(KeyEventArgs e)
        {
            cameraController.KeyDown(e);
            if (!Controllers[Mode].KeyDown(e))
            {
                Logger.Log(Logger.LogLevel.Warning, "Failed Command" + Mode.ToString());
            }
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

        private void SelectPointController_PointSelected(object sender, ItemSelectedEventArgs e)
        {
            OnItemSelected(e);
        }

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


        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        public void ProcessMouseInput(KIMouseEventArgs mouse)
        {
            switch (mouse.MouseState)
            {
                case MOUSE_STATE.DOWN:
                    cameraController.Down(mouse);
                    if (!Controllers[Mode].Down(mouse))
                    {
                        Logger.Log(Logger.LogLevel.Warning, "Failed Command" + Mode.ToString());
                    }

                    break;
                case MOUSE_STATE.CLICK:
                    cameraController.Click(mouse);
                    if (!Controllers[Mode].Click(mouse))
                    {
                        Logger.Log(Logger.LogLevel.Warning, "Failed Command" + Mode.ToString());
                    }

                    break;
                case MOUSE_STATE.MOVE:
                    cameraController.Move(mouse);
                    if (!Controllers[Mode].Move(mouse))
                    {
                        Logger.Log(Logger.LogLevel.Warning, "Failed Command" + Mode.ToString());
                    }

                    break;
                case MOUSE_STATE.UP:
                    cameraController.Up(mouse);
                    if (!Controllers[Mode].Up(mouse))
                    {
                        Logger.Log(Logger.LogLevel.Warning, "Failed Command" + Mode.ToString());
                    }

                    break;
                case MOUSE_STATE.WHEEL:
                    cameraController.Wheel(mouse);
                    if (!Controllers[Mode].Wheel(mouse))
                    {
                        Logger.Log(Logger.LogLevel.Warning, "Failed Command" + Mode.ToString());
                    }

                    break;
            }
        }
        #endregion
    }
}
