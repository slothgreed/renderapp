using System.Windows.Forms.Integration;
using KI.Gfx.GLUtil;
using KI.UI.ViewModel;

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

        public ViewportViewModel(ViewModelBase parent)
            : base(parent)
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
