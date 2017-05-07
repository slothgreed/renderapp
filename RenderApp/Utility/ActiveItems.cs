using System.Collections.Generic;

namespace RenderApp
{
    public abstract class ActiveItems <T>
    {
        private List<T> _select = new List<T>();
        
        private List<T> _lastSelect = new List<T>();

        public void Select(T item)
        {
            _select.Add(item);
        }
        public void Select(List<T> items)
        {
            _select = items;
        }
        public List<T> Select()
        {
            return _select;
        }
        
    }
}
