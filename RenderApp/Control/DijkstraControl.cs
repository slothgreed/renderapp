using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.Control
{
    class DijkstraControl : IControl
    {
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            base.Down(mouse);
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                RenderApp.Scene.ActiveScene.Picking(LeftMouse.Click);
            }
            return true;
        }
    }
}
