using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RenderApp.Control
{
    public class ControlManager
    {
        CONTROL_MODE Mode = CONTROL_MODE.Selection;
        private Dictionary<CONTROL_MODE,IControl> Controllers = new Dictionary<CONTROL_MODE, IControl>();
        public enum CONTROL_MODE
        {
            Selection,
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
            Controllers.Add(CONTROL_MODE.Selection,new PickingControl());
            Controllers.Add(CONTROL_MODE.Dijkstra,new DijkstraControl());
        }
        public void SetMode(CONTROL_MODE mode)
        {
            Mode = mode;
        }

        public void ProcessInput(MouseEventArgs mouse, MOUSE_STATE state)
        {
            switch(state)
            {
                case MOUSE_STATE.DOWN:
                    Controllers[Mode].Down(mouse);
                    break;
                case MOUSE_STATE.CLICK:
                    Controllers[Mode].Click(mouse);
                    break;
                case MOUSE_STATE.MOVE:
                    Controllers[Mode].Move(mouse);
                    break;
                case MOUSE_STATE.UP:
                    Controllers[Mode].Up(mouse);
                    break;
                case MOUSE_STATE.WHEEL:
                    Controllers[Mode].Wheel(mouse);
                    break;

            }
        }
    }
}
