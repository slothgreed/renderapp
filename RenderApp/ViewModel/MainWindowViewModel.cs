using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RenderApp.AssetModel;
using System.IO;
using System.Collections.ObjectModel;
using RenderApp.GLUtil;
using RenderApp.Utility;
using RenderApp.GLUtil.ShaderModel;
using System.Windows.Forms;
using RenderApp.AssetModel.LightModel;
using RenderApp.View.Dialog;
using RenderApp.ViewModel.Dialog;
using RenderApp.Control;
using RenderApp.Globals;
namespace RenderApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region [property method]
        private readonly ObservableCollection<DockWindowViewModel> _centerItemsSource = new ObservableCollection<DockWindowViewModel>();
        public ObservableCollection<DockWindowViewModel> CenterItemsSource
        {
            get
            {
                return _centerItemsSource;
            }
        }

        private readonly ObservableCollection<DockWindowViewModel> _leftUpItemsSource = new ObservableCollection<DockWindowViewModel>();
        public ObservableCollection<DockWindowViewModel> LeftUpItemsSource
        {
            get
            {
                return _leftUpItemsSource;
            }
        }
        private readonly ObservableCollection<DockWindowViewModel> _leftDownItemsSource = new ObservableCollection<DockWindowViewModel>();
        public ObservableCollection<DockWindowViewModel> LeftDownItemsSource
        {
            get
            {
                return _leftDownItemsSource;
            }
        }

        private readonly ObservableCollection<DockWindowViewModel> _rightUpItemsSource = new ObservableCollection<DockWindowViewModel>();
        public ObservableCollection<DockWindowViewModel> RightUpItemsSource
        {
            get
            {
                return _rightUpItemsSource;
            }
        }
        private readonly ObservableCollection<DockWindowViewModel> _rightDownItemsSource = new ObservableCollection<DockWindowViewModel>();
        public ObservableCollection<DockWindowViewModel> RightDownItemsSource
        {
            get
            {
                return _rightDownItemsSource;
            }
        }



        private DockWindowViewModel _activePane = null;
        public DockWindowViewModel ActivePane
        {
            get
            {
                return _activePane;
            }
            set
            {
                if (_activePane != value)
                {
                    SetValue<DockWindowViewModel>(ref _activePane, value);
                }
            }
        }
        public DockWindowViewModel ActiveWindow
        {
            get;
            set;
        }

        public bool PostProcessMode
        {
            get
            {
                if(m_Viewport == null)
                {
                    return false;
                }
                if(m_Viewport.RenderSystem == null)
                {
                    return false;
                }
                return m_Viewport.RenderSystem.PostProcessMode;
            }
        }
        #endregion

        #region [Member]

        private static MainWindowViewModel _instance;
        public static MainWindowViewModel Instance
        {
            get
            {
                return _instance;
            }
        }

        private Viewport m_Viewport;
        public RootNodeViewModel ProjectWindow
        {
            get;
            set;
        }
        public RootNodeViewModel SceneWindow
        {
            get;
            set;
        }
        public GeometryViewModel GeometryWindow
        {
            get;
            set;
        }
        public ViewportViewModel ViewportWindow
        {
            get;
            set;
        }
        public MaterialViewModel MaterialWindow
        {
            get;
            set;
        }
        public RenderSystemViewModel RenderSystemWindow
        {
            get;
            set;
        }
        #endregion

        #region [constructor]
 

        public MainWindowViewModel()
        {
            ProjectWindow = new RootNodeViewModel(Project.ActiveProject.RootNode, "Project");
            ViewportWindow = new ViewportViewModel();
            MaterialWindow = new MaterialViewModel();

            _leftUpItemsSource.Add(ProjectWindow);
            _rightDownItemsSource.Add(MaterialWindow);
            _rightUpItemsSource.Add(new ShaderProgramViewModel(null));

            _centerItemsSource.Add(ViewportWindow);

            Viewport.Instance.OnCreateViewportEvent += OnCreateViewportEvent;
            _instance = this;
        }

        void OnCreateViewportEvent()
        {
            SceneWindow = new RootNodeViewModel(Scene.ActiveScene.RootNode, "Scene");
            _leftUpItemsSource.Add(SceneWindow);
            RenderSystemWindow = new RenderSystemViewModel(Viewport.Instance.RenderSystem);
            _leftDownItemsSource.Add(RenderSystemWindow);
        }
        #endregion

        #region [Project Menu Command]
        private void NewProjectCommand()
        {
            if (!ProjectInfo.IsOpen)
            {
                ProjectInfo.IsOpen = true;
            }
        }
        private void OpenProjectCommand()
        {
            ProjectInfo.IsOpen = true;
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ProjectDirectory;
            dlg.Filter = "projファイル(*.@proj)|*.proj;";
            dlg.FilterIndex = 1;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }
        private void SaveProjectCommand()
        {

        }
        private void SaveAsProjectCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ProjectDirectory;
            dlg.Filter = "projファイル(*.@proj)|*.proj;";
            dlg.FilterIndex = 1;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }
        #endregion

        #region [Asset Menu Command]
        private void LoadAssetCommand(object loadAssetMenuParam)
        {
            RAAsset menuParam = (RAAsset)loadAssetMenuParam;

            switch (menuParam)
            {
                case RAAsset.Model:
                    Load3DModelCommand();
                    break;
                case RAAsset.Texture:
                    LoadTextureCommand();
                    break;
                case RAAsset.Shader:
                    LoadShaderCommand();
                    break;
            }
        }
        private void Load3DModelCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ProjectDirectory;
            dlg.Filter = "objファイル(*.obj)|*.obj;|stlファイル(*.stl)|*.stl;|すべてのファイル(*.*)|*.*";
            dlg.Multiselect = true;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    Geometry geometry = AssetFactory.Instance.CreateLoad3DModel(filename);
                    if (geometry != null)
                    {
                        Project.ActiveProject.AddChild(AssetFactory.Instance.CreateGeometry(geometry));
                    }
                }
            }
        }
        private void LoadTextureCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ProjectDirectory;
            dlg.Filter = "画像ファイル(*.bmp;*.jpg;*png)|*.bmp;*.jpg;*.png;";
            dlg.Title = "開くファイルを選択してください。";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    string extension = Path.GetExtension(filename);
                    if (extension == ".bmp" || extension == ".png" || extension == ".jpg")
                    {
                        TextureFactory.Instance.CreateTexture(Path.GetFileName(filename), filename);
                    }
                }
            }
        }
        private void LoadShaderCommand()
        {
            //System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            //dlg.InitialDirectory = Project.ProjectDirectory;
            //dlg.Filter = "シェーダファイル(*.vert;*.frag;*.geom;*.tes;*.tcs)|*.vert;*.frag;*.geom;*.tes;*.tcs;";
            //dlg.Title = "開くファイルを選択してください。";
            //dlg.Multiselect = true;

            //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    foreach (var filename in dlg.FileNames)
            //    {
            //        string extension = Path.GetExtension(filename);

            //        switch (extension)
            //        {
            //            case ".vert":
            //            case ".frag":
            //            case ".geom":
            //            case ".tcs":
            //            case ".tes":
            //                AssetFactory.Instance.CreateShaderProgram(filename);
            //                break;
            //        }
            //    }
            //}
        }
        #endregion
        
        #region [Model Menu Command]
        private void CreateObjectCommand(object createObjectMenu)
        {
            RAGeometry menuParam = (RAGeometry)createObjectMenu;

            switch (menuParam)
            {
                case RAGeometry.Cube:
                    CreateCubeCommand();
                    break;
                case RAGeometry.Sphere:
                    CreateSphereCommand();
                    break;
                case RAGeometry.Plane:
                    CreatePlaneCommand();
                    break;
                case RAGeometry.WireFrame:
                    CreateWireFrameCommand();
                    break;
                case RAGeometry.Polygon:
                    CreatePolygonCommand();
                    break;
                default:
                    break;
            }
        }
        private void CreateCubeCommand()
        {
            Cube cube = new Cube(Asset.GetNameFromType(EAssetType.Geometry), Scene.ActiveScene.WorldMin, Scene.ActiveScene.WorldMax);
            AssetFactory.Instance.CreateGeometry(cube);
        }
        private void CreateSphereCommand()
        {
            Sphere sphere = new Sphere(Asset.GetNameFromType(EAssetType.Geometry), 5, 5, 5, true, OpenTK.Vector3.UnitY);
            AssetFactory.Instance.CreateGeometry(sphere);
        }
        private void CreatePlaneCommand()
        {
            Plane plane = new Plane(Asset.GetNameFromType(EAssetType.Geometry));
            AssetFactory.Instance.CreateGeometry(plane);
        }
        private void CreateWireFrameCommand()
        {
            if (!AssetFactory.Instance.CreateWireFrame(Scene.ActiveScene.SelectAsset))
            {
                MessageBox.Show("Trianglesのポリゴンモデルのみで作成できます。");
            }
        }
        private void CreatePolygonCommand()
        {
            if (!AssetFactory.Instance.CreatePolygon(Scene.ActiveScene.SelectAsset))
            {
                MessageBox.Show("Trianglesのポリゴンモデルのみで作成できます。");
            }
        }
        #endregion

        #region [swintch controller command]
        public ControlManager.CONTROL_MODE ControlMode
        {
            get
            {
                return ControlManager.Instance.Mode;
            }
        }
        private void ControllerCommand(object controllerMenu)
        {
            RAController menuParam = (RAController)controllerMenu;
            OnPropertyChanging("ControlMode");
            switch(menuParam)
            {
                case RAController.Default:
                    ControlManager.Instance.Mode = ControlManager.CONTROL_MODE.Default;
                    break;
                case RAController.Dijkstra:
                    ControlManager.Instance.Mode = ControlManager.CONTROL_MODE.Dijkstra;
                    break;
            }
            OnPropertyChanged("ControlMode");
        }
        #endregion

        #region [MainWindow Event Command]
        private void WindowCloseCommand()
        {
            Scene.AllDispose();
            Project.ActiveProject.Dispose();
            ShaderFactory.Instance.Dispose();
            RenderPassFactory.Instance.Dispose();
        }
        private void SizeChangedCommand()
        {

        }
        public void LoadedCommand()
        {
            m_Viewport = Viewport.Instance;

            if (m_Viewport != null)
            {
                m_Viewport.Initialize();
            }
        }
        public void ClosedCommand()
        {
            if (m_Viewport != null)
            {
                m_Viewport.Closed();
            }
            GC.Collect();
        }
        #endregion

        #region [Rendering Menu Command]
        public void TogglePostProcessCommand()
        {
            m_Viewport.RenderSystem.TogglePostProcess();
            OnPropertyChanged("PostProcessMode");
        }
        #endregion
        
        #region [Analyze Menu Command]
        private void VoxelizeCommand()
        {
            
            var dvm = new VoxelDialogViewModel();
            var dlg = new VoxelDialogView(dvm);
            dlg.ShowDialog();
            if(!(bool)dlg.DialogResult)
            {
                return;
            }
            if(!AssetFactory.Instance.CreateVoxel(Scene.ActiveScene.SelectAsset,dvm.PartitionNum))
            {
                MessageBox.Show("Trianglesのポリゴンモデルのみで作成できます。");
            }
        }
        private void OctreeCommand()
        {
            if (!AssetFactory.Instance.CreateOctree(Scene.ActiveScene.SelectAsset))
            {
                MessageBox.Show("Trianglesのポリゴンモデルのみで作成できます。");
            }
        }
        #endregion

        #region [Update Method]
        public void UpdateSelectNode(NodeItemViewModel node)
        {
            //if(node == null)
            //{
            //    return;
            //}
            //if(node.Model is Asset)
            //{
            //    Scene.ActiveScene.SelectAsset = node.Model;
            //}
            //switch (node.AssetType)
            //{
            //    case EAssetType.Geometry:
            //        if (node.Model is Camera)
            //        {

            //        }
            //        else if (node.Model is Light)
            //        {

            //        }
            //        else
            //        {
            //            AddWindow(new GeometryViewModel((Geometry)node.Model));
            //        }
            //        break;
            //    case EAssetType.Materials:
            //        AddWindow(new MaterialViewModel((Material)node.Model));
            //        break;
            //    case EAssetType.Textures:
            //        AddWindow(new TextureViewModel((Texture)node.Model));
            //        break;
            //    default:
            //        break;
            //}
        }


        public override void UpdateProperty()
        {
            Viewport.Instance.glControl_Paint(null, null);
        }
        #endregion
        
    }
}
