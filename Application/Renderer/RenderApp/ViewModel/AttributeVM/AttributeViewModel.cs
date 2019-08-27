using KI.Asset.Attribute;
using KI.Presenter.ViewModel;

namespace RenderApp.ViewModel
{
    public class AttributeViewModel : ViewModelBase
    {
        private AttributeBase Model;

        public AttributeViewModel(ViewModelBase parent, AttributeBase model)
            : base(parent, model)
        {
            Model = model;
        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
        }

        public bool Visible
        {
            get { return Model.Visible; }
            set
            {
                Model.Visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }
    }
}
