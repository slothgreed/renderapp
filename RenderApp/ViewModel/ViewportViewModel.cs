using System.Windows.Forms.Integration;
using KI.Gfx.GLUtil;

namespace RenderApp.ViewModel
{
    public class ViewportViewModel : TabItemViewModel
    {
        private WindowsFormsHost glContext;

        public override string Title
        {
            get
            {
                return "RenderApp";
            }
        }

        public ViewportViewModel()
        {
        }

        public WindowsFormsHost GLContext
        {
            get
            {
                if (glContext == null)
                {
                    glContext = new WindowsFormsHost()
                    {
                        Child = Viewport.Instance.GLControl
                    };
                }

                return glContext;
            }
        }

        public override void UpdateProperty()
        {
        }
    }
}
