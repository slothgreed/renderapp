using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation.Core
{
    public abstract class FactoryBase<T1, T2> where T2 : KIObject
    {
        Dictionary<T1, T2> Items = new Dictionary<T1, T2>();

        public FactoryBase()
        {
            Items = new Dictionary<T1, T2>();
        }

        public void Add(T1 key, T2 value)
        {
            Items.Add(key, value);
        }

        public void RemoveByKey(T1 key)
        {
            if(Items.ContainsKey(key))
            {
                Items[key].Dispose();
                Items.Remove(key);
            }
        }

        public T2 FindByKey(T1 key)
        {
            if(Items.ContainsKey(key))
            {
                return Items[key];
            }
            return null;
        }

        public T1 FindByValue(T2 value)
        {
            if(Items.ContainsValue(value))
            {
                return Items.FirstOrDefault(x => x.Value == value).Key;
            }
            return default(T1);
        }

        public void Clear()
        {
            foreach(var v in Items.Values)
            {
                v.Dispose();
            }
            Items.Clear();
        }

    }
}
