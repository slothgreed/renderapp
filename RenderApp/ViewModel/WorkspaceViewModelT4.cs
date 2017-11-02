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

		private TabControlViewModel _RightDockPanel;
		public TabControlViewModel RightDockPanel
		{
			get
			{
				return _RightDockPanel;
			}

			set
			{
				SetValue(ref _RightDockPanel, value);
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