using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class TabItemViewModel : ViewModelBase
    {
        private ICommand closeTabItem;

        protected string title;


        public virtual string Title
        {
            get
            {
                return title;
            }

            set
            {
                SetValue<string>(ref title, value);
            }
        }

        public ICommand CloseTabItem
        {
            get
            {
                if (closeTabItem == null)
                {
                    return closeTabItem = CreateCommand(CloseTabItemCommand);
                }

                return closeTabItem;
            }
        }

        public TabItemViewModel(ViewModelBase parent)
            : base(parent)
        {

        }

        private void CloseTabItemCommand()
        {
            if (Parent != null)
            {
                ((TabControlViewModel)Parent).Remove(this);
            }
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
