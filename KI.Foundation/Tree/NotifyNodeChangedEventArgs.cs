
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace KI.Foundation.Tree
{
	public enum NotifyNodeChangedAction
	{
		Add,
		Remove,
		Replace,
		Clear,
	}

	public class NotifyNodeChangedEventArgs
	{
		public NotifyNodeChangedAction NodeAction;
		public KINode NewItems;
		public int NewIndex;
		public KINode OldItems;
		public int OldIndex;

		public NotifyNodeChangedEventArgs(NotifyNodeChangedAction _NodeAction)
		{
			NodeAction = _NodeAction;
		}
		public NotifyNodeChangedEventArgs(NotifyNodeChangedAction _NodeAction,KINode _NewItems)
		{
			NodeAction = _NodeAction;
			NewItems = _NewItems;
		}
		public NotifyNodeChangedEventArgs(NotifyNodeChangedAction _NodeAction,KINode _NewItems,int _NewIndex)
		{
			NodeAction = _NodeAction;
			NewItems = _NewItems;
			NewIndex = _NewIndex;
		}
		public NotifyNodeChangedEventArgs(NotifyNodeChangedAction _NodeAction,KINode _NewItems,KINode _OldItems)
		{
			NodeAction = _NodeAction;
			NewItems = _NewItems;
			OldItems = _OldItems;
		}
		public NotifyNodeChangedEventArgs(NotifyNodeChangedAction _NodeAction,KINode _NewItems,int _NewIndex,KINode _OldItems,int _OldIndex)
		{
			NodeAction = _NodeAction;
			NewItems = _NewItems;
			NewIndex = _NewIndex;
			OldItems = _OldItems;
			OldIndex = _OldIndex;
		}
	}
}
