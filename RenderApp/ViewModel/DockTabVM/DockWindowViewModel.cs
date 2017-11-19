using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public abstract class DockWindowViewModel : ViewModelBase
    {
        public enum Place
        {
            LeftUp,
            LeftDown,
            RightUp,
            RightDown,
            Floating
        }

        public DockWindowViewModel(ViewModelBase parent, string title, Place place)
            : base(parent)
        {
            Title = title;
            InitPlace = place;
        }

        protected string _title;
        public virtual string Title
        {
            get
            {
                return _title;
            }
            protected set
            {
                SetValue<string>(ref _title, value);
            }
        }

        public Place InitPlace
        {
            get; private set;
        }
    }
}
