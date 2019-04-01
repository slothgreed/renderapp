using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.Tool
{
    /// <summary>
    /// コントローラのモード
    /// </summary>
    public enum CONTROL_MODE
    {
        SelectTriangle,
        SelectLine,
        SelectPoint,
        EdgeFlips,
        Dijkstra,
        Geodesic
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
}
