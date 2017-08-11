using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;

namespace KI.Foundation.Tree
{
    /// <summary>
    /// ノード
    /// </summary>
    public class KINode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="kiobject">ノードの中身</param>
        public KINode(KIObject kiobject)
        {
            KIObject = kiobject;
            Name = kiobject.Name;
            Children = new List<KINode>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ノードの名前</param>
        public KINode(string name)
        {
            Name = name;
            Children = new List<KINode>();
        }

        /// <summary>
        /// ノード挿入イベント
        /// </summary>
        public EventHandler<NotifyNodeChangedEventArgs> NodeInserted { get; set; }

        /// <summary>
        /// ノード削除イベント
        /// </summary>
        public EventHandler<NotifyNodeChangedEventArgs> NodeRemoved { get; set; }

        /// <summary>
        /// ノードの名前
        /// </summary>
        public string Name { get; private set; }

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
            if (FindChild(node.Name) == null)
            {
                node.Parent = this;
                Children.Add(node);
                OnNodeInserted(Children.Count, node);
            }
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

            if (FindChild(child.Name) == null)
            {
                KINode node = new KINode(child);
                node.Parent = this;
                Children.Add(node);
                OnNodeInserted(Children.Count, node);
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
            var remove = FindChild(name);
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
            foreach (var child in Children)
            {
                var item = FindChild(name);
                if (item == null)
                {
                    child.FindRecursiveChild(name);
                }
                else
                {
                    RemoveChild(item);
                    return;
                }
            }
        }
        #endregion
        #region [check child]

        /// <summary>
        /// 子供がいるか確認
        /// </summary>
        /// <param name="child">子供</param>
        /// <returns>いるか</returns>
        public bool ExistChild(KIObject child)
        {
            if (FindChild(child.Name) != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ノードを取得
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>ノード</returns>
        public KINode FindChild(string name)
        {
            return Children.Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// ノードを取得
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>ノード</returns>
        public KINode FindRecursiveChild(string name)
        {
            foreach (var child in Children)
            {
                var item = FindChild(name);
                if (item == null)
                {
                    child.FindRecursiveChild(name);
                }
                else
                {
                    return item;
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            if (KIObject != null)
            {
                KIObject.Dispose();
            }

            foreach (var child in Children)
            {
                child.Dispose();
            }
        }
        #region [getter]
        /// <summary>
        /// 全てのノードを取得
        /// </summary>
        /// <returns>全てのノード</returns>
        public IEnumerable<KINode> AllChildren()
        {
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
        /// オブジェクトを表す文字列
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// ノード削除イベント
        /// </summary>
        /// <param name="removeNode">削除したノード</param>
        private void OnNodeRemoved(KINode removeNode)
        {
            NodeRemoved?.Invoke(this, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Remove, removeNode));
        }

        /// <summary>
        /// ノード挿入イベント
        /// </summary>
        /// <param name="index">追加した番号</param>
        /// <param name="insertNode">追加したノード</param>
        private void OnNodeInserted(int index, KINode insertNode)
        {
            NodeInserted?.Invoke(insertNode, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, insertNode, index));
        }
    }
}
