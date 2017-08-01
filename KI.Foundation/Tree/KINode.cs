using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;

namespace KI.Foundation.Tree
{

    public class KINode
    {
        public delegate void InsertNodeEventHandler(object sender, NotifyNodeChangedEventArgs e);
        public InsertNodeEventHandler InsertNodeEvent;

        public delegate void RemoveNodeEventHandler(object sender, NotifyNodeChangedEventArgs e);
        public RemoveNodeEventHandler RemoveNodeEvent;

        public List<KINode> Children
        {
            get;
            private set;
        }
        private string emptyName;
        public string Name
        {
            get
            {
                if (KIObject == null)
                {
                    return emptyName;
                }
                return KIObject.Name;
            }
        }
        public KIObject KIObject
        {
            get;
            private set;
        }
        private KINode Parent;

        public KINode(KIObject _kiobject)
        {
            KIObject = _kiobject;
            Children = new List<KINode>();
        }
        public KINode(string name)
        {
            emptyName = name;
            Children = new List<KINode>();
        }
        #region [add child]
        public void AddChild(KINode node)
        {
            if (FindChild(node.Name) == null)
            {
                node.Parent = this;
                Children.Add(node);
                if (InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, Children.Count));
                }
            }
        }
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
                KINode node = new KINode(name);
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
                var node = new KINode(name);
                Children.Insert(index, node);
                if (InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, index));
                }
            }
        }
        public void Insert(int index, KIObject child)
        {
            if (Children.Count < index)
            {
                AddChild(child);
                return;
            }
            if (FindChild(child.Name) == null)
            {
                var node = new KINode(child.Name);
                Children.Insert(index, node);
                if (InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, index));
                }

            }
        }
        #endregion
        #region [remove method]
        public void RemoveChild(KINode child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                if (RemoveNodeEvent != null)
                {
                    RemoveNodeEvent(child, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Remove, child));
                }
            }
        }
        public void RemoveChild(KIObject child)
        {
            var remove = FindChild(child.Name);
            if (remove != null)
            {
                RemoveChild(remove);
            }
        }
        public void RemoveChild(string key)
        {
            var remove = FindChild(key);
            if (remove != null)
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
        public bool ExistChild(KIObject child)
        {
            if (FindChild(child.Name) != null)
            {
                return true;
            }
            return false;
        }
        public KINode FindChild(string key)
        {
            return Children.Where(p => p.Name == key).FirstOrDefault();
        }
        public KINode FindRecursiveChild(string key)
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
                    return item;
                }
            }
            return null;
        }

        #endregion
        public override string ToString()
        {
            if (KIObject != null)
                return KIObject.Name;
            return emptyName;
        }

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

        public IEnumerable<KIObject> AllChildrenObject()
        {
            foreach (var child in Children)
            {
                yield return child.KIObject;

                foreach (var grand in child.Children)
                {
                    yield return grand.KIObject;
                }
            }
        }
        #endregion


    }
}
