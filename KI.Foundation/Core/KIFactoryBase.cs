using System.Collections.Generic;
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
            Items = new List<T>();
        }

        /// <summary>
        /// アイテムリスト
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// アイテムの追加
        /// </summary>
        /// <param name="value">アイテム</param>
        public void AddItem(T value)
        {
            if (value != null)
            {
                Items.Add(value);
            }
        }

        /// <summary>
        /// 一致key全削除
        /// </summary>
        /// <param name="name">名前</param>
        public void RemoveByKey(string name)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == name)
                {
                    Items.RemoveAt(i);
                    i--;// 要素の++を無視するため、2番目を削除したら、次の探索は2番目をする
                }
            }
        }

        /// <summary>
        /// 同一名の探索
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>オブジェクト</returns>
        public T FindByName(string name)
        {
            return Items.Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 同値の探索
        /// </summary>
        /// <param name="value">オブジェクト</param>
        /// <returns>名前</returns>
        public string FindByValue(T value)
        {
            var item = Items.Where(p => p == value).FirstOrDefault();

            if (item == null)
            {
                return string.Empty;
            }
            else
            {
                return item.Name;
            }
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            foreach (var item in Items)
            {
                item.Dispose();
            }

            Items.Clear();
        }
    }
}
