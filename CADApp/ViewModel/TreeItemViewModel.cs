using KI.Renderer;
using KI.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADApp.ViewModel
{
    public class TreeItemViewModel : ViewModelBase
    {
        private SceneNode _model;
        public SceneNode Model
        {
            get
            {
                return _model;
            }
            set
            {
                SetValue(ref _model, value);
            }
        }

        public TreeItemViewModel(ViewModelBase parent, SceneNode node)
            : base(parent)
        {
            Model = node;
        }
    }
}
