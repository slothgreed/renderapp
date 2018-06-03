using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace KI.Foundation.Core
{
    /// <summary>
    /// ファクトリーのベースクラス
    /// </summary>
    /// <typeparam name="T">KIObject</typeparam>
    public abstract class KIFactoryBase<T> where T : KIObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KIFactoryBase()
        {
            items = new List<T>();
            Items = items;
        }

        /// <summary>
        /// アイテムリスト
        /// </summary>
        private List<T> items { get; }

        /// <summary>
        /// 読み込み専用
        /// </summary>
        public IReadOnlyList<T> Items { get; }

        public EventHandler<NotifyCollectionChangedEventArgs> ItemChanged { get; set; }

        /// <summary>
        /// アイテムの追加
        /// </summary>
        /// <param name="value">アイテム</param>
        public void AddItem(T value)
        {
            if (value != null)
            {
                items.Add(value);
            }

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value);
            OnFactoryItemChanged(args);
        }

        /// <summary>
        /// 一致key全削除
        /// </summary>
        /// <param name="name">名前</param>
        public void RemoveByName(string name)
        {
            var removeList = new List<T>();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == name)
                {
                    removeList.Add(items[i]);
                    items.RemoveAt(i);
                    i--;// 要素の++を無視するため、2番目を削除したら、次の探索は2番目をする
                }
            }

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removeList);
            OnFactoryItemChanged(args);
        }

        /// <summary>
        /// 同一名の探索
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>オブジェクト</returns>
        public T FindByName(string name)
        {
            return items.Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 同値の探索
        /// </summary>
        /// <param name="value">オブジェクト</param>
        /// <returns>名前</returns>
        public string FindByValue(T value)
        {
            var item = items.Where(p => p == value).FirstOrDefault();

            if (item == null)
            {
                return string.Empty;
            }
            else
            {
                return item.Name;
            }
        }


        public void RemoveByValue(T value)
        {
            if (this.items.Contains(value))
            {
                items.Remove(value);
            }

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value);
            OnFactoryItemChanged(args);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            foreach (var item in items)
            {
                item.Dispose();
            }

            items.Clear();
        }

        private void OnFactoryItemChanged(NotifyCollectionChangedEventArgs e)
        {
            ItemChanged?.Invoke(this, e);
        }
    }
}
