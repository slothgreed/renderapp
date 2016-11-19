using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RenderApp.RA_Control
{
    public class ControlManager : IControl
    {
        private Dictionary<CONTROL_MODE,IControl> Controllers = new Dictionary<CONTROL_MODE, IControl>();

        private IControl CameraController = new CameraControl();
        public enum CONTROL_MODE
        {
            Default,
            Dijkstra
        }
        public enum MOUSE_STATE
        {
            DOWN,
            MOVE,
            UP,
            CLICK,
            WHEEL,
        }

        CONTROL_MODE _mode = CONTROL_MODE.Default;
        public CONTROL_MODE Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                Controllers[_mode].UnBinding();
                _mode = value;
                Controllers[_mode].Binding();
            }
        }

        private static ControlManager _instance = new ControlManager();
        public static ControlManager Instance
        {
            get
            {
                return _instance;
            }
        }
        private ControlManager()
        {
            Controllers.Add(CONTROL_MODE.Default,new DefaultControl());
            Controllers.Add(CONTROL_MODE.Dijkstra,new DijkstraControl());
            CameraController = new CameraControl();
        }

        public void ProcessInput(MouseEventArgs mouse, MOUSE_STATE state)
        {
            

            switch(state)
            {
                case MOUSE_STATE.DOWN:
                    CameraController.Down(mouse);
                    Controllers[Mode].Down(mouse);
                    break;
                case MOUSE_STATE.CLICK:
                    CameraController.Click(mouse);
                    Controllers[Mode].Click(mouse);
                    break;
                case MOUSE_STATE.MOVE:
                    CameraController.Move(mouse);
                    Controllers[Mode].Move(mouse);
                    break;
                case MOUSE_STATE.UP:
                    CameraController.Up(mouse);
                    Controllers[Mode].Up(mouse);
                    break;
                case MOUSE_STATE.WHEEL:
                    CameraController.Wheel(mouse);
                    Controllers[Mode].Wheel(mouse);
                    break;

            }
        }
    }
}
