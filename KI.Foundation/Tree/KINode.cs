using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using KI.Foundation.Core;

namespace KI.Foundation.Tree
{
    /// <summary>
    /// ノード
    /// </summary>
    public class KINode : KIObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="kiobject">ノードの中身</param>
        public KINode(KIObject kiobject)
            : base(kiobject.Name)
        {
            KIObject = kiobject;
            Children = new List<KINode>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ノードの名前</param>
        public KINode(string name)
            : base(name)
        {
            Children = new List<KINode>();
        }

        /// <summary>
        /// ノード挿入イベント
        /// </summary>
        public EventHandler<NotifyCollectionChangedEventArgs> NodeInserted { get; set; }

        /// <summary>
        /// ノード削除イベント
        /// </summary>
        public EventHandler<NotifyCollectionChangedEventArgs> NodeRemoved { get; set; }

        /// <summary>
        /// 親ノード
        /// </summary>
        public KINode Parent { get; private set; }

        /// <summary>
        /// 子供
        /// </summary>
        public List<KINode> Children { get; private set; }

        /// <summary>
        /// ノードの中身
        /// </summary>
        public KIObject KIObject { get; private set; }

        #region [add child]

        /// <summary>
        /// 子供の追加
        /// </summary>
        /// <param name="node">ノード</param>
        public void AddChild(KINode node)
        {
            node.Parent = this;
            Children.Add(node);
            OnNodeInserted(Children.Count, node);
        }

        /// <summary>
        /// 子供の追加
        /// </summary>
        /// <param name="child">ノード</param>
        public void AddChild(KIObject child)
        {
            if (child == null)
            {
                return;
            }

            //if (FindChild(child.Name) == null)
            {
                KINode node = new KINode(child);
                AddChild(node);
            }
        }

        #endregion
        #region [remove method]

        /// <summary>
        /// 子供の削除
        /// </summary>
        /// <param name="child">子供</param>
        public void RemoveChild(KINode child)
        {
            if (Children.Contains(child))
            {
                child.Dispose();
                Children.Remove(child);
                OnNodeRemoved(child);
            }
        }

        /// <summary>
        /// 子供の削除
        /// </summary>
        /// <param name="name">名前</param>
        public void RemoveChild(string name)
        {
            var remove = Children.Where(p => p.Name == name).FirstOrDefault();
            if (remove != null)
            {
                RemoveChild(remove);
            }
        }

        /// <summary>
        /// 再帰的に子供を削除
        /// </summary>
        /// <param name="name">名前</param>
        public void RemoveRecursiveChild(string name)
        {
            var removeList = new List<KINode>();
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                if (child.Children.Count > 0)
                {
                    child.RemoveRecursiveChild(name);
                }
                else
                {
                    if (child.Name == name)
                    {
                        RemoveChild(child);
                        i--;
                    }
                }
            }
        }
        #endregion
        #region [check child]

        /// <summary>
        /// ノードを取得
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>ノード</returns>
        public KINode FindRecursiveChild(string name)
        {
            foreach (var child in Children)
            {
                if (child.Name == name)
                {
                    return child;
                }

                var result = child.FindRecursiveChild(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            if (KIObject != null)
            {
                KIObject.Dispose();
            }

            foreach (var child in Children)
            {
                child.Dispose();
            }

            base.Dispose();
        }
        #region [getter]
        /// <summary>
        /// 全てのノードを取得
        /// </summary>
        /// <returns>全てのノード</returns>
        public IEnumerable<KINode> AllChildren()
        {
            yield return this;

            foreach (var child in Children)
            {
                yield return child;

                foreach (var grand in child.Children)
                {
                    yield return grand;
                }
            }
        }

        #endregion

        /// <summary>
        /// ノード削除イベント
        /// </summary>
        /// <param name="removeNode">削除したノード</param>
        private void OnNodeRemoved(KINode removeNode)
        {
            NodeRemoved?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removeNode));
        }

        /// <summary>
        /// ノード挿入イベント
        /// </summary>
        /// <param name="index">追加した番号</param>
        /// <param name="insertNode">追加したノード</param>
        private void OnNodeInserted(int index, KINode insertNode)
        {
            NodeInserted?.Invoke(insertNode, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, insertNode, index));
        }
    }
}
