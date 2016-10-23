using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp;
namespace RenderApp.ViewModel
{
    public abstract class DockWindowViewModel : ViewModelBase
    {
        private string _title;
        public virtual string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetValue<string>(ref _title, value);
            }
        }

    }
}
