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
namespace RenderApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region [property method]
        private readonly ObservableCollection<AvalonWindowViewModel> _documents = new ObservableCollection<AvalonWindowViewModel>();
        ReadOnlyObservableCollection<AvalonWindowViewModel> _readDocument;
        public ReadOnlyObservableCollection<AvalonWindowViewModel> Documents
        {
            get
            {
                if (_readDocument == null)
                {
                    _readDocument = new ReadOnlyObservableCollection<AvalonWindowViewModel>(_documents);
                }
                return _readDocument;
            }
        }

        private readonly ObservableCollection<AvalonWindowViewModel> _anchorables = new ObservableCollection<AvalonWindowViewModel>();
        private ReadOnlyObservableCollection<AvalonWindowViewModel> _readAnchorable;
        public ReadOnlyObservableCollection<AvalonWindowViewModel> Anchorables
        {
            get
            {
                if (_readAnchorable == null)
                {
                    _readAnchorable = new ReadOnlyObservableCollection<AvalonWindowViewModel>(_anchorables);
                }
                return _readAnchorable;
            }
        }
        private AvalonWindowViewModel _activePane = null;
        public AvalonWindowViewModel ActivePane
        {
            get
            {
                return _activePane;
            }
            set
            {
                if (_activePane != value)
                {
                    SetValue<AvalonWindowViewModel>(ref _activePane, value);
                }
            }
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
        public AssetTreeViewModel AssetWindow;
        #endregion

        #region [constructor]
 

        public MainWindowViewModel()
        {
            AssetWindow = new AssetTreeViewModel(null, "Asset");
            _anchorables.Add(AssetWindow);
            _anchorables.Add(new GeometryViewModel());
            _anchorables.Add(new MaterialViewModel());
            _anchorables.Add(new ShaderProgramViewModel(null));
            _documents.Add(new ViewportViewModel());
            Viewport.Instance.OnCreateViewportEvent += OnCreateViewportEvent;
            _instance = this;
        }

        void OnCreateViewportEvent()
        {
            _anchorables.Add(new RenderSystemViewModel(Viewport.Instance.RenderSystem));
        }
        #endregion

        #region [Project Menu Command]
        private void NewProjectCommand()
        {
            if (!Project.IsOpen)
            {
                Project.IsOpen = true;
            }
        }
        private void OpenProjectCommand()
        {
            Project.IsOpen = true;
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = Project.ProjectDirectory;
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
            dlg.InitialDirectory = Project.ProjectDirectory;
            dlg.Filter = "projファイル(*.@proj)|*.proj;";
            dlg.FilterIndex = 1;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }
        #endregion

        #region [Asset Menu Command]
        private void Load3DModelCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = Project.ProjectDirectory;
            dlg.Filter = "objファイル(*.obj)|*.obj;|stlファイル(*.stl)|*.stl;|すべてのファイル(*.*)|*.*";
            dlg.Multiselect = true;
            dlg.Title = "開くファイルを選択してください。";
            Geometry geometry = null;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    string extension = Path.GetExtension(filename);
                    switch (extension)
                    {
                        case ".obj":
                            geometry = new CObjFile(Asset.GetNameFromPath(filename), filename);
                            break;
                        case ".stl":
                            geometry = new StlFile(Asset.GetNameFromPath(filename), filename);
                            break;
                    }
                    if (geometry != null)
                    {
                        AssetFactory.Instance.CreateGeometry(geometry);
                    }
                }
            }
        }
        private void LoadTextureCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = Project.ProjectDirectory;
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
                        TextureFactory.Instance.CreateTexture(filename);
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

        #region [MainWindow Event Command]
        private void WindowCloseCommand()
        {
            Scene.AllDispose();
            AssetFactory.Instance.Dispose();
            ShaderFactory.Instance.Dispose();
            TextureFactory.Instance.Dispose();
            RenderPassFactory.Instance.Dispose();
        }
        private void SizeChangedCommand()
        {
            foreach (var loop in Documents)
            {
                loop.SizeChanged();
            }
            foreach (var loop in Anchorables)
            {
                loop.SizeChanged();
            }
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

        #region [AvalonWindow method]
        private void AddWindow(AvalonWindowViewModel newWindow)
        {
            var oldWindow = _anchorables.Where(x => x.WindowPosition == newWindow.WindowPosition).First();
            _anchorables.Add(newWindow);

            if (oldWindow != null)
            {
                _anchorables.Remove(oldWindow);
            }
        }

        #endregion

        #region [Update Method]
        public void UpdateSelectNode(TreeItemViewModel node)
        {
            if(node == null)
            {
                return;
            }
            if(node.Model is Asset)
            {
                Scene.ActiveScene.SelectAsset = node.Model;
            }
            switch (node.AssetType)
            {
                case EAssetType.Geometry:
                    if (node.Model is Camera)
                    {

                    }
                    else if (node.Model is Light)
                    {

                    }
                    else
                    {
                        AddWindow(new GeometryViewModel((Geometry)node.Model));
                    }
                    break;
                case EAssetType.Materials:
                    AddWindow(new MaterialViewModel((Material)node.Model));
                    AddWindow(new ShaderViewModel((Material)node.Model));
                    break;
                case EAssetType.Textures:
                    AddWindow(new TextureViewModel((Texture)node.Model));
                    break;
                default:
                    break;
            }
        }


        public override void UpdateProperty()
        {

        }
        #endregion
        
    }
}
