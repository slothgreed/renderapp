using KI.Asset;
using KI.UI.ViewModel;
using OpenTK;

namespace RenderApp.ViewModel
{
    public class SceneNodeViewModel : DockWindowViewModel
    {
        public SceneNodeViewModel(ViewModelBase parent, SceneNode scene, string name, DockWindowViewModel.Place place)
            : base(parent, scene, name, place)
        {
            Model = scene;
        }

        public Vector3 Rotate
        {
            get
            {
                return Model.Rotate;
            }

            set
            {
                Model.Rotate = value;
                OnPropertyChanged(nameof(Rotate));
            }
        }

        public Vector3 Scale
        {
            get
            {
                return Model.Scale;
            }

            set
            {
                Model.Scale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }

        public Vector3 Translate
        {
            get
            {
                return Model.Translate;
            }

            set
            {
                Model.Translate = value;
                OnPropertyChanged(nameof(Translate));
            }
        }

        private SceneNode _model;
        public SceneNode Model
        {
            get
            {
                return _model;
            }

            protected set
            {
                _model = value;
                Title = Model.ToString();
            }
        }

    }
}
