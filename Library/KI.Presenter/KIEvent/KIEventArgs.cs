using KI.Foundation.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Presenter.KIEvent
{
    /// <summary>
    /// KINode の変更通知イベント
    /// </summary>
    public class NodeCollectionChangeEventArgs : EventArgs
    {
        /// <summary>
        /// ノードの変更状態
        /// </summary>
        public CollectionChangeAction Action { get; private set; }

        /// <summary>
        /// 追加された親ノード
        /// </summary>
        public KINode Parent { get; private set; }

        /// <summary>
        /// 追加したノード
        /// </summary>
        public KINode Item { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="action">変更アクション</param>
        /// <param name="parent">親ノード</param>
        /// <param name="item">追加ノード</param>
        public NodeCollectionChangeEventArgs(CollectionChangeAction action, KINode parent, KINode item)
        {
            Action = action;
            Parent = parent;
            Item = item;
        }
    }
}
