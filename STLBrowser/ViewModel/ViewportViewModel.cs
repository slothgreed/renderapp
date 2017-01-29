using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using RenderApp.GLUtil;
using KI.Foundation.ViewModel;
namespace STLBrowser.ViewModel
{
    public class ViewportViewModel : ViewModelBase
    {
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
