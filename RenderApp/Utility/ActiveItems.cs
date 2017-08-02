using System.Collections.Generic;

namespace RenderApp
{
    public abstract class ActiveItems<T>
    {
        private List<T> select = new List<T>();

        private List<T> lastSelect = new List<T>();

        public void Select(T item)
        {
            select.Add(item);
        }

        public void Select(List<T> items)
        {
            select = items;
        }

        public List<T> Select()
        {
            return select;
        }
    }
}
