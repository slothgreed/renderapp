using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace KI.Foundation.ViewModel
{
    public abstract class MathViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetValue<string>(ref _name, value);
            }
        }
        protected object Owner
        {
            get;
            set;
        }

    }
}
