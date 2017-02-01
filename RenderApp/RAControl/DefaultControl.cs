using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
namespace RenderApp.RA_Control
{
    class DefaultControl : IControl
    {
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            if(mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                SceneManager.Instance.RenderSystem.Picking((int)LeftMouse.Click.X, (int)LeftMouse.Click.Y);
            }
            return true;
        }
    }
}
