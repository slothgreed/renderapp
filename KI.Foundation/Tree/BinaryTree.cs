using System;

namespace KI.Foundation.Tree
{
    /// <summary>
    /// バイナリツリークラス
    /// </summary>
    /// <typeparam name="T">テンプレート</typeparam>
    public class BinaryTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// ルートノード
        /// </summary>
        private Node root;

        /// <summary>
        /// バイナリツリー
        /// </summary>
        /// <param name="data">データ</param>
        public BinaryTree(T[] data)
        {
            foreach (var d in data)
            {
                Insert(d);
            }
        }

        /// <summary>
        /// 最小値を返します。
        /// </summary>
        public T Min
        {
            get
            {
                return this.root.Min.Value;
            }
        }

        /// <summary>
        /// データを挿入します。
        /// </summary>
        /// <param name="data">データ</param>
        public void Insert(T data)
        {
            if (root == null)
            {
                root = new Node(data);
                return;
            }

            // 末端ノードの探索
            Node node = this.root;
            Node parent = null;
            while (node != null)
            {
                parent = node;
                if (data.CompareTo(node.Value) < 0)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }

            var newNode = new Node(data);

            // 親ノードの子に新しいノードを設定
            newNode.Parent = parent;
            if (data.CompareTo(parent.Value) < 0)
            {
                parent.Left = newNode;
            }
            else
            {
                parent.Right = newNode;
            }
        }

        /// <summary>
        /// データを削除します。
        /// </summary>
        /// <param name="data">データ</param>
        public void Remove(T data)
        {
            var removeNode = Find(data);
            Remove(removeNode);
        }

        /// <summary>
        /// データからノードの取得
        /// </summary>
        /// <param name="data">データ</param>
        /// <returns>ノード</returns>
        private Node Find(T data)
        {
            Node node = this.root;
            while (node != null)
            {
                if (data.CompareTo(node.Value) == 0)
                {
                    return node;
                }
                else if (data.CompareTo(node.Value) < 0)
                {
                    node = node.Left;
                }
                else if (data.CompareTo(node.Value) > 0)
                {
                    node = node.Right;
                }
            }

            return null;
        }

        /// <summary>
        /// データを削除します。
        /// </summary>
        /// <param name="node">ノード</param>
        private void Remove(Node node)
        {
            // 末端ノードなら、親ノードの持つ自身のノードを消す
            if (node.Left == null && node.Right == null)
            {
                if (node == this.root)
                {
                    this.root = null;
                    return;
                }

                if (node.Parent.Left == node)
                {
                    node.Parent.Left = null;
                }
                else if (node.Parent.Right == node)
                {
                    node.Parent.Right = null;
                }

                return;
            }

            if (node.Left == null)
            {
                node.Value = node.Right.Value;
                node.Right = null;
                return;
            }

            if (node.Right == null)
            {
                node.Value = node.Left.Value;
                node.Left = null;
                return;
            }

            var targetNode = node.Right.Min;
            if (targetNode.Parent.Left == targetNode)
            {
                targetNode.Parent.Left = null;
            }
            else if (targetNode.Parent.Right == targetNode)
            {
                targetNode.Parent.Right = null;
            }

            node.Value = targetNode.Value;
        }

        /// <summary>
        /// ノードクラス
        /// </summary>
        private class Node
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="value">値</param>
            public Node(T value)
            {
                Value = value;
            }

            /// <summary>
            /// 左側のノードです。
            /// </summary>
            public Node Left { get; set; }

            /// <summary>
            /// 右側のノードです。
            /// </summary>
            public Node Right { get; set; }

            /// <summary>
            /// 親ノードです。
            /// </summary>
            public Node Parent { get; set; }

            /// <summary>
            /// 値です。
            /// </summary>
            public T Value { get; set; }

            /// <summary>
            /// 最小ノードを取得します。
            /// </summary>
            public Node Min
            {
                get
                {
                    var node = this;
                    while (node != null)
                    {
                        if (node.Left == null)
                        {
                            return node;
                        }

                        node = node.Left;
                    }

                    return null;
                }
            }
        }
    }
}
