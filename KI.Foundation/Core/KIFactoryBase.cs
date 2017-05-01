using System.Collections.Generic;

namespace KI.Foundation.Core
{
    public abstract class KIFactoryBase<T> where T : KIObject
    {
        List<T> Items;

        public KIFactoryBase()
        {
            Items = new List<T>();
        }

        public void AddItem(T value)
        {
            if(value != null)
            {
                Items.Add(value);
            }
        }

        /// <summary>
        /// 一致key全削除
        /// </summary>
        public void RemoveByKey(string name)
        {
            for(int i = 0; i < Items.Count; i++)
            {
                if(Items[i].Name == name)
                {
                    Items.RemoveAt(i);
                    i--;// 要素の++を無視するため、2番目を削除したら、次の探索は2番目をする
                }
            }
        }

        public T FindByKey(string name)
        {
            foreach(var item in Items)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        public string FindByValue(T value)
        {
            foreach (var item in Items)
            {
                if (item == value)
                {
                    return item.Name;
                }
            }
            return string.Empty;
        }

        public IEnumerable<T> AllItem
        {
            get
            {
                return Items;
            }
        }

        public void Dispose()
        {
            foreach(var v in Items)
            {
                v.Dispose();
            }
            Items.Clear();
        }

    }
}
