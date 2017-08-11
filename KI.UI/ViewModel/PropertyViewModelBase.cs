using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.UI.ViewModel
{
    public abstract class PropertyViewModelBase : ViewModelBase
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
