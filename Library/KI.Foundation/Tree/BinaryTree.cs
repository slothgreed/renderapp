using System;
using System.Collections.Generic;

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
        public BinaryTree(IEnumerable<T> data)
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
        public Node Find(T data)
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
        /// <param name="removeNode">ノード</param>
        private void Remove(Node removeNode)
        {
            var parentNode = removeNode.Parent;
            // 末端ノードなら、親ノードの持つ自身のノードを消す
            if (removeNode.IsLeaf)
            {
                if (parentNode == null)
                {
                    this.root = null;
                    return;
                }

                if (parentNode.Left == removeNode)
                {
                    parentNode.Left = null;
                }
                else if (parentNode.Right == removeNode)
                {
                    parentNode.Right = null;
                }

                removeNode.Dispose();
                return;
            }

            if (removeNode.Left == null)
            {
                if (parentNode == null)
                {
                    this.root = removeNode.Right;
                }
                else
                {
                    if (parentNode.Left == removeNode)
                    {
                        parentNode.Left = removeNode.Right;
                    }
                    else if (parentNode.Right == removeNode)
                    {
                        parentNode.Right = removeNode.Right;
                    }
                }

                removeNode.Right.Parent = parentNode;
                removeNode.Dispose();
                return;
            }

            if (removeNode.Right == null)
            {
                if (parentNode == null)
                {
                    this.root = removeNode.Left;
                }
                else
                {
                    if (parentNode.Left == removeNode)
                    {
                        parentNode.Left = removeNode.Left;
                    }
                    else if (parentNode.Right == removeNode)
                    {
                        parentNode.Right = removeNode.Left;
                    }
                }

                removeNode.Left.Parent = parentNode;
                removeNode.Dispose();
                return;
            }

            var moveNode = removeNode.Right.Min;
            var moveParent = moveNode.Parent;

            removeNode.Value = moveNode.Value;

            if (moveParent.Right == moveNode)
            {
                moveParent.Right = moveNode.Right;
            }
            else
            {
                moveParent.Left = moveNode.Right;
            }

            if (moveNode.Right != null)
            {
                moveNode.Right.Parent = moveParent;
            }
        }

        /// <summary>
        /// ノードクラス
        /// </summary>
        public class Node
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
            /// 末端ノードか
            /// </summary>
            public bool IsLeaf
            {
                get
                {
                    return Right == null && Left == null;
                }
            }

            /// <summary>
            /// エラーチェック
            /// </summary>
            public bool Error
            {
                get
                {
                    //if (Left != null)
                    //{
                    //    if (Value.CompareTo(Left.Value) < 0)
                    //    {
                    //        return true;
                    //    }

                    //    if (Left.Error)
                    //    {
                    //        return true;
                    //    }
                    //}

                    //if (Right != null)
                    //{
                    //    if (Value.CompareTo(Right.Value) > 0)
                    //    {
                    //        return true;
                    //    }

                    //    if (Right.Error)
                    //    {
                    //        return true;
                    //    }
                    //}

                    return false;
                }
            }

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

            /// <summary>
            /// ノード構成を文字列表記
            /// </summary>
            public void ToStringNode()
            {
                Console.WriteLine("Parent" + Parent?.Value.ToString());
                ToStringAllChild(string.Empty);
            }

            /// <summary>
            /// 解放処理
            /// </summary>
            public void Dispose()
            {
                Parent = null;
                Left = null;
                Right = null;
                Value = default(T);
            }

            /// <summary>
            /// 全ての子を列挙します。
            /// </summary>
            /// <param name="str">添え字</param>
            private void ToStringAllChild(string str)
            {
                Console.WriteLine(str + Value.ToString());
                str += "↓";
                if (Left != null)
                {
                    str += "L";
                    Left.ToStringAllChild(str);
                }

                if (Right != null)
                {
                    str += "R";
                    Right.ToStringAllChild(str);
                }

                if (Left != null)
                {
                    str = str.Remove(str.LastIndexOf("L"), 1);
                }

                if (Right != null)
                {
                    str = str.Remove(str.LastIndexOf("R"), 1);
                }

                str = str.Remove(str.LastIndexOf("↓"), 1);
            }
        }
    }
}
