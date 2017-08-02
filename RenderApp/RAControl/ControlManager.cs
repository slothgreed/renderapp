using System.Collections.Generic;
using System.Windows.Forms;
namespace RenderApp.RAControl
{
    public class ControlManager : IControl
    {
        public Dictionary<CONTROL_MODE, IControl> Controllers = new Dictionary<CONTROL_MODE, IControl>();

        private IControl cameraController = new CameraControl();

        public enum CONTROL_MODE
        {
            SelectTriangle,
            Dijkstra,
            SelectPoint
        }

        public enum MOUSE_STATE
        {
            DOWN,
            MOVE,
            UP,
            CLICK,
            WHEEL,
        }

        CONTROL_MODE mode = CONTROL_MODE.SelectTriangle;

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

        public static ControlManager Instance = new ControlManager();

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
