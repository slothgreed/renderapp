using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp;
namespace RenderApp.ViewModel
{
    public abstract class AvalonWindowViewModel : ViewModelBase
    {
        public enum AvalonWindow
        {
            LeftUp = 0,
            LeftDown,
            RightUp,
            RightDown,
        }
        public virtual AvalonWindow WindowPosition
        {
            get;
            set;
        }
        public virtual string Title
        {
            get;
            set;
        }

        public abstract void SizeChanged();
    }
}
