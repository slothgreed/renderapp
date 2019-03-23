namespace KI.Tool
{
    public class ItemSelectedEventArgs
    {
        public object SelectItem { get; private set; }

        public ItemSelectedEventArgs(object item)
        {
            SelectItem = item;
        }
    }
}