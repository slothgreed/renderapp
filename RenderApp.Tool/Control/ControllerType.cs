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
}
