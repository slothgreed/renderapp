using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace RenderApp.ViewModel
{
	public partial class WorkspaceViewModel
	{

		private TabControlViewModel _LeftUpDockPanel;
		public TabControlViewModel LeftUpDockPanel
		{
			get
			{
				return _LeftUpDockPanel;
			}

			set
			{
				SetValue(ref _LeftUpDockPanel, value);
			}
		}

		private TabControlViewModel _LeftDownDockPanel;
		public TabControlViewModel LeftDownDockPanel
		{
			get
			{
				return _LeftDownDockPanel;
			}

			set
			{
				SetValue(ref _LeftDownDockPanel, value);
			}
		}

		private TabControlViewModel _RightUpDockPanel;
		public TabControlViewModel RightUpDockPanel
		{
			get
			{
				return _RightUpDockPanel;
			}

			set
			{
				SetValue(ref _RightUpDockPanel, value);
			}
		}

		private TabControlViewModel _RightDownDockPanel;
		public TabControlViewModel RightDownDockPanel
		{
			get
			{
				return _RightDownDockPanel;
			}

			set
			{
				SetValue(ref _RightDownDockPanel, value);
			}
		}

		private TabControlViewModel _CenterDockPanel;
		public TabControlViewModel CenterDockPanel
		{
			get
			{
				return _CenterDockPanel;
			}

			set
			{
				SetValue(ref _CenterDockPanel, value);
			}
		}
	}
}