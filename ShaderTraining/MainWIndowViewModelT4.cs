using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShaderTraining.ViewModel
{
	public partial class MainWindowViewModel
	{
		private ICommand _PlaneObject;
		public ICommand PlaneObject
		{
			get
			{
				if (_PlaneObject == null)
				{
					return _PlaneObject = CreateCommand(PlaneObjectCommand);						
				}

				return _PlaneObject;
			}
		}
		private ICommand _SphereObject;
		public ICommand SphereObject
		{
			get
			{
				if (_SphereObject == null)
				{
					return _SphereObject = CreateCommand(SphereObjectCommand);						
				}

				return _SphereObject;
			}
		}
	}
}