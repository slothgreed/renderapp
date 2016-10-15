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
        private Dictionary<CONTROL_MODE,IControl> Controllers = new Dictionary<CONTROL_MODE, IControl>();
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
                _mode = value;
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
