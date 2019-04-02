using System.Collections.ObjectModel;
using KI.UI.ViewModel;
namespace RenderApp.ViewModel
{
    public partial class WorkspaceViewModel
	{

		private ObservableCollection<ViewModelBase> _AnchorablesSources;
		public ObservableCollection<ViewModelBase> AnchorablesSources
		{
			get
			{
				return _AnchorablesSources;
			}

			set
			{
				SetValue(ref _AnchorablesSources, value);
			}
		}

		private ObservableCollection<ViewModelBase> _DocumentsSources;
		public ObservableCollection<ViewModelBase> DocumentsSources
		{
			get
			{
				return _DocumentsSources;
			}

			set
			{
				SetValue(ref _DocumentsSources, value);
			}
		}
	}
}