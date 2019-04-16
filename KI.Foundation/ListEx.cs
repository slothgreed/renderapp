using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation
{
    /// <summary>
    /// Listの拡張
    /// </summary>
    public static class ListEx
    {
        /// <summary>
        /// 初期化
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="list">リスト</param>
        /// <param name="value">初期価値</param>
        public static void Initialize<T>(this List<T> list, T value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = value;
            }
        }
    }
}
