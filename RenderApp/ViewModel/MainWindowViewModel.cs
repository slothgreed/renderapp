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
        private DockWindowViewModel _LUSelectItem;
        public DockWindowViewModel LUSelectItem
        {
            get
            {
                return _LUSelectItem;
            }
            set
            {
                SetValue(ref _LUSelectItem, value);
            }
        }
        #endregion

        #region [constructor]
 

        public MainWindowViewModel()
        {

            _LeftUpItemsSource.Add( new RootNodeViewModel(Project.ActiveProject.RootNode, "Project"));
            _RightUpItemsSource.Add( new MaterialViewModel());
            _RightDownItemsSource.Add(new ShaderProgramViewModel(null));
            _CenterItemsSource.Add( new ViewportViewModel());

            Viewport.Instance.OnCreateViewportEvent += OnCreateViewportEvent;
            _instance = this;
        }

        void OnCreateViewportEvent()
        {
            _LeftUpItemsSource.Add(new RootNodeViewModel(Scene.ActiveScene.RootNode, "Scene"));
            _LeftDownItemsSource.Add(new RenderSystemViewModel(Viewport.Instance.RenderSystem));
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
        public void UpdateSelectNode(RANode node)
        {
            if(node.RAObject == null)
            {
                return;
            }
            if(node.RAObject is Geometry)
            {
                _LeftDownItemsSource.Add(new GeometryViewModel((Geometry)node.RAObject));
            }
            else if(node.RAObject is Material)
            {
                _RightUpItemsSource.Add(new MaterialViewModel((Material)node.RAObject));
            }
            else if (node.RAObject is ShaderProgram)
            {
                _RightDownItemsSource.Add(new ShaderProgramViewModel((ShaderProgram)node.RAObject));
            }
        }


        public override void UpdateProperty()
        {
            Viewport.Instance.glControl_Paint(null, null);
        }
        #endregion


        internal void CloseTabWindow(DockWindowViewModel dockVM)
        {
            if(LeftDownItemsSource.Contains(dockVM))
            {
                LeftDownItemsSource.Remove(dockVM);
            }
            if (LeftUpItemsSource.Contains(dockVM))
            {
                LeftUpItemsSource.Remove(dockVM);
            }
            if (RightUpItemsSource.Contains(dockVM))
            {
                RightUpItemsSource.Remove(dockVM);
            }
            if (RightDownItemsSource.Contains(dockVM))
            {
                RightDownItemsSource.Remove(dockVM);
            }
            if (CenterItemsSource.Contains(dockVM))
            {
                CenterItemsSource.Remove(dockVM);
            }
        }
    }
}
