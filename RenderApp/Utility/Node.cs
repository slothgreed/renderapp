﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp;
namespace RenderApp.Utility
{

    public class Node 
    {
        public delegate void InsertNodeEventHandler(object sender, NotifyNodeChangedEventArgs e);
        public InsertNodeEventHandler InsertNodeEvent;

        public delegate void RemoveNodeEventHandler(object sender, NotifyNodeChangedEventArgs e);
        public RemoveNodeEventHandler RemoveNodeEvent;

        public bool IsVisible
        {
            get;
            set;
        }
        public List<Node> Children
        {
            get;
            private set;
        }
        private string emptyName;
        public string Name
        {
            get
            {
                if (MyObject == null)
                {
                    return emptyName;
                }
                return MyObject.Key;
            }
        }
        public MyObject MyObject;
        private Node Parent;

        public Node(MyObject _MyObject)
        {
            MyObject = _MyObject;
            Children = new List<Node>();
        }
        public Node(string name)
        {
            emptyName = name;
            Children = new List<Node>();
        }
        #region [add child]
        internal void AddChild(Node node)
        {
            if (FindChild(node.Name) == null)
            {
                node.Parent = this;
                Children.Add(node);
                if(InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, Children.Count));
                }
            }
        }
        public void AddChild(MyObject child)
        {
            if (FindChild(child.Key) == null)
            {
                Node node = new Node(child);
                node.Parent = this;
                Children.Add(node);
                if (InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, Children.Count));
                }
            }
        }
        public void AddChild(string name)
        {
            if (FindChild(name) == null)
            {
                Node node = new Node(name);
                node.Parent = this;
                Children.Add(node);
                if (InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, Children.Count));
                }

            }
        }
        public void Insert(int index, string name)
        {
            if (Children.Count < index)
            {
                AddChild(name);
                return;
            }
            if (FindChild(name) == null)
            {
                var node = new Node(name);
                Children.Insert(index, node);
                if(InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, index));
                }
            }
        }
        public void Insert(int index, MyObject child)
        {
            if (Children.Count < index)
            {
                AddChild(child);
                return;
            }
            if (FindChild(child.Key) == null)
            {
                var node = new Node(child.Key);
                Children.Insert(index, node);
                if (InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, index));
                }

            }
        }
        #endregion
        #region [remove method]
        private void RemoveChild(Node child)
        {
            if(Children.Contains(child))
            {
                Children.Remove(child);
                if(RemoveNodeEvent != null)
                {
                    RemoveNodeEvent(child, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Remove, child));
                }
            }
        }
        public void RemoveChild(MyObject child)
        {
            var remove = FindChild(child.Key);
            if(remove != null)
            {
                RemoveChild(remove);
            }
        }
        public void RemoveChild(string key)
        {
            var remove = FindChild(key);
            if(remove != null)
            {
                RemoveChild(remove);
            }
        }
        public void RemoveRecursiveChild(string key)
        {
            foreach (var child in Children)
            {
                var item = FindChild(key);
                if (item == null)
                {
                    child.FindRecursiveChild(key);
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
        public bool ExistChild(MyObject child)
        {
            if (FindChild(child.Key) != null)
            {
                return true;
            }
            return false;
        }
        public Node FindChild(string key)
        {
           return Children.Where(p => p.Name == key).FirstOrDefault();
        }
        public Node FindRecursiveChild(string key)
        {
            foreach(var child in Children)
            {
                var item = FindChild(key);
                if (item == null)
                {
                    child.FindRecursiveChild(key);
                }
                else
                {
                    return item;
                }
            }
            return null;
        }

        #endregion
        public override string ToString()
        {
            if (MyObject != null)
                return MyObject.Key;
            return emptyName;
        }

        internal void Dispose()
        {
            MyObject.Dispose();
            foreach(var child in Children)
            {
                child.Dispose();
            }
        }

        internal IEnumerable<Node> AllChildren()
        {
            foreach(var child in Children)
            {
                foreach(var grand in child.Children )
                {
                    yield return grand;
                }
            }
        }


    }
}