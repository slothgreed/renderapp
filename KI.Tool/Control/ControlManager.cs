using System.Collections.Generic;
using System.Windows.Forms;

namespace KI.Tool.Control
{
    /// <summary>
    /// コントロールマネージャ
    /// </summary>
    public class ControlManager : IControl
    {
        /// <summary>
        /// カメラコントローラ
        /// </summary>
        private IControl cameraController = new CameraControl();

        /// <summary>
        /// コントローラの現在のモード
        /// </summary>
        private CONTROL_MODE mode = CONTROL_MODE.SelectTriangle;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private ControlManager()
        {
            Controllers.Add(CONTROL_MODE.SelectTriangle, new SelectTriangleControl());
            Controllers.Add(CONTROL_MODE.Dijkstra, new DijkstraControl());
            Controllers.Add(CONTROL_MODE.SelectPoint, new SelectPointControl());
            cameraController = new CameraControl();
        }

        /// <summary>
        /// コントローラのモード
        /// </summary>
        public enum CONTROL_MODE
        {
            SelectTriangle,
            Dijkstra,
            SelectPoint
        }

        /// <summary>
        /// マウスの状態
        /// </summary>
        public enum MOUSE_STATE
        {
            DOWN,
            MOVE,
            UP,
            CLICK,
            WHEEL,
        }

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static ControlManager Instance { get; } = new ControlManager();

        /// <summary>
        /// コントロールリスト
        /// </summary>
        public Dictionary<CONTROL_MODE, IControl> Controllers { get; set; } = new Dictionary<CONTROL_MODE, IControl>();

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
                Controllers[mode].Binding();
            }
        }

        /// <summary>
        /// 入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        /// <param name="state">状態</param>
        public void ProcessInput(MouseEventArgs mouse, MOUSE_STATE state)
        {
            switch (state)
            {
                case MOUSE_STATE.DOWN:
                    cameraController.Down(mouse);
                    Controllers[Mode].Down(mouse);
                    break;
                case MOUSE_STATE.CLICK:
                    cameraController.Click(mouse);
                    Controllers[Mode].Click(mouse);
                    break;
                case MOUSE_STATE.MOVE:
                    cameraController.Move(mouse);
                    Controllers[Mode].Move(mouse);
                    break;
                case MOUSE_STATE.UP:
                    cameraController.Up(mouse);
                    Controllers[Mode].Up(mouse);
                    break;
                case MOUSE_STATE.WHEEL:
                    cameraController.Wheel(mouse);
                    Controllers[Mode].Wheel(mouse);
                    break;
            }
        }
    }
}
