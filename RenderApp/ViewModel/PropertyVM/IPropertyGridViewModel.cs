using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.ViewModel
{
    public interface IPropertyGridViewModel
    {
        Dictionary<string, object> PropertyItem
        {
            get;
            set;
        }
    }
}
