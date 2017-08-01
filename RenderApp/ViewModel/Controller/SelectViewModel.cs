using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.ViewModel
{
    public partial class SelectViewModel : TabItemViewModel
    {
        public override string Title
        {
            get
            {
                return "Select";
            }
        }

        private void ExecuteCommand()
        {
            throw new NotImplementedException();
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
