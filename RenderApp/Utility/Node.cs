using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp;
namespace RenderApp.Utility
{

    public class RANode 
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
        public List<RANode> Children
        {
            get;
            private set;
        }
        private string emptyName;
        public string Name
        {
            get
            {
                if (RAObject == null)
                {
                    return emptyName;
                }
                return RAObject.Key;
            }
        }
        public RAObject RAObject;
        private RANode Parent;

        public RANode(RAObject _MyObject)
        {
            RAObject = _MyObject;
            Children = new List<RANode>();
        }
        public RANode(string name)
        {
            emptyName = name;
            Children = new List<RANode>();
        }
        #region [add child]
        internal void AddChild(RANode node)
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
        public void AddChild(RAObject child)
        {
            if (FindChild(child.Key) == null)
            {
                RANode node = new RANode(child);
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
                RANode node = new RANode(name);
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
                var node = new RANode(name);
                Children.Insert(index, node);
                if(InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, index));
                }
            }
        }
        public void Insert(int index, RAObject child)
        {
            if (Children.Count < index)
            {
                AddChild(child);
                return;
            }
            if (FindChild(child.Key) == null)
            {
                var node = new RANode(child.Key);
                Children.Insert(index, node);
                if (InsertNodeEvent != null)
                {
                    InsertNodeEvent(node, new NotifyNodeChangedEventArgs(NotifyNodeChangedAction.Add, node, index));
                }

            }
        }
        #endregion
        #region [remove method]
        private void RemoveChild(RANode child)
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
        public void RemoveChild(RAObject child)
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
        public bool ExistChild(RAObject child)
        {
            if (FindChild(child.Key) != null)
            {
                return true;
            }
            return false;
        }
        public RANode FindChild(string key)
        {
           return Children.Where(p => p.Name == key).FirstOrDefault();
        }
        public RANode FindRecursiveChild(string key)
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
            if (RAObject != null)
                return RAObject.Key;
            return emptyName;
        }

        internal void Dispose()
        {
            RAObject.Dispose();
            foreach(var child in Children)
            {
                child.Dispose();
            }
        }

        internal IEnumerable<RANode> AllChildren()
        {
            foreach(var child in Children)
            {
                yield return child;

                foreach(var grand in child.Children )
                {
                    yield return grand;
                }
            }
        }


    }
}
