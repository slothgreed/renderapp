using RenderApp.Tool;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    /// <summary>
    /// 点を選択したイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnItemSelectedEventHandler(object sender, ItemSelectedEventArgs e);

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

        /// <summary>
        /// 選択イベント
        /// </summary>
        public event OnItemSelectedEventHandler ItemSelected;

        public DockWindowViewModel(ViewModelBase parent, object model, string title, Place place)
            : base(parent, model)
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


        /// <summary>
        /// 選択イベント
        /// </summary>
        /// <param name="アイテム"></param>
        protected virtual void OnItemSelected(ItemSelectedEventArgs eventArgs)
        {
            if (ItemSelected != null)
            {
                ItemSelected(this, eventArgs);
            }
        }

    }
}
