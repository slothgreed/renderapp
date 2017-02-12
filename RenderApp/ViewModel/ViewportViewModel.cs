using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using KI.Gfx.GLUtil;
using RenderApp.GLUtil;
namespace RenderApp.ViewModel
{
    public class ViewportViewModel : TabItemViewModel
    {
        public override string Title
        {
            get
            {
                return "RenderApp";
            }
        }
        private WindowsFormsHost _glContext;
        public WindowsFormsHost GLContext
        {
            get
            {
                if (_glContext == null)
                {
                    _glContext = new WindowsFormsHost()
                    {
                        Child = Viewport.Instance.glControl
                    };
                }
                return _glContext;
            }
        }
        public ViewportViewModel()
        {
        }

        public override void UpdateProperty()
        {

        }
    }
}
