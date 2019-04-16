using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using CADApp.Model;
using CADApp.Model.Node;
using CADApp.Tool.Controller;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Renderer.Technique;
using KI.Tool.Controller;
using KI.UI.ViewModel;
using OpenTK;

namespace CADApp.ViewModel
{
    public class ViewportViewModel : ViewModelBase
    {
        private WindowsFormsHost _glContext;
        public WindowsFormsHost GLContext
        {
            get
            {
                if (_glContext == null)
                {
                    _glContext = new WindowsFormsHost()
                    {
                        Child = Viewport.Instance.GLControl
                    };
                }

                return _glContext;
            }
        }

        public Viewport Viewport
        {
            get
            {
                return Viewport.Instance;
            }
        }

        public string FolderPath
        {
            get
            {
                return "AAA";
            }
        }

        public ViewportViewModel(ViewModelBase parent)
            : base(parent)
        {
            Viewport.Instance.OnLoaded += OnLoadedEvent;
            Viewport.Instance.OnResize += OnResizeEvent;
            Viewport.Instance.OnRender += OnRenderEvent;
            Viewport.Instance.OnMouseWheel += OnMouseWheelEvent;
            Viewport.Instance.OnMouseUp += OnMouseMoveUpEvent;
            Viewport.Instance.OnMouseMove += OnMouseMoveEvent;
            Viewport.Instance.OnMouseDown += OnMouseDownEvent;
            Viewport.Instance.OnMouseClick += OnMouseClickEvent;
            Viewport.Instance.OnMouseDoubleClick += OnMouseDoubleClickEvent;
        }

        /// <summary>
        /// カメラコントローラ
        /// </summary>
        private ControllerBase cameraController = new CameraController();

        private Dictionary<ControllerType, ControllerBase> Controller = new Dictionary<ControllerType, ControllerBase>();

        ControllerType currentController;

        public ControllerType CurrentController
        {
            get { return currentController; }
            set
            {
                Controller[currentController].UnBinding();
                currentController = value;
                Controller[currentController].Binding();
            }
        }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene MainScene { get; set; }

        /// <summary>
        /// レンダラー
        /// </summary>
        public RenderSystem RenderSystem { get; set; }

        public void OnLoadedEvent(object sender, EventArgs e)
        {
            DeviceContext.Instance.SetClearColor(1, 1, 1, 1);
            MainScene = new Scene("MainScene", new AppRootNode("Root"));
            RenderSystem = new RenderSystem();
            RenderSystem.ActiveScene = MainScene;

            Workspace.Instance.MainScene = MainScene;
            Workspace.Instance.RenderSystem = RenderSystem;

            Controller.Add(ControllerType.Select, new SelectController());
            Controller.Add(ControllerType.SketchLine, new SketchLineController());
            Controller.Add(ControllerType.SketchRectangle, new SketchRectangleController());
            Controller.Add(ControllerType.SketchSplineCurvature, new SketchSplineController());
            Controller.Add(ControllerType.BuildCube, new BuildCubeController());

            CurrentController = ControllerType.Select;

            InitializeScene();
            InitializeRenderer();

        }

        private void OnResizeEvent(object sender, EventArgs e)
        {
            if (MainScene != null)
            {
                MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
            }
            
            RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        private void OnRenderEvent(object sender, PaintEventArgs e)
        {
            RenderSystem.Render();
        }

        private void OnMouseWheelEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.WHEEL);
        }

        private void OnMouseMoveUpEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.UP);
        }

        private void OnMouseMoveEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.MOVE);
        }

        private void OnMouseDownEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.DOWN);
        }

        private void OnMouseClickEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.CLICK);
        }

        private void OnMouseDoubleClickEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.DOUBLECLICK);
        }



        private void InitializeScene()
        {
            MainScene.MainCamera = AssetFactory.Instance.CreateCamera("MainCamera");
            var light = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            var sphere = AssetFactory.Instance.CreateSphere("sphere", 0.1f, 32, 32, true);
            MainScene.MainLight = new LightNode("SunLight", light, SceneNodeFactory.Instance.CreatePolygonNode("SunLight", sphere));
            //MainScene.AddObject(MainScene.MainCamera);
            MainScene.AddObject(MainScene.MainLight);

            var axis = new Axis(Vector3.Zero, MainScene.WorldMax);
            var axisObject = SceneNodeFactory.Instance.CreatePolygonNode("Axis", axis.Vertex, axis.Color, axis.Index, PolygonType.Lines);
            MainScene.AddObject(axisObject);

            var grid = new GridPlane(1, 0.1f, Vector3.Zero);
            var girdObject = SceneNodeFactory.Instance.CreatePolygonNode("GridPlane", grid.Position, grid.Color, grid.Index, PolygonType.Lines);
            MainScene.AddObject(girdObject);
        }

        private void InitializeRenderer()
        {
            RenderTechniqueFactory.Instance.RendererSystem = RenderSystem;
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));
            var gBufferTexture = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            RenderSystem.OutputBuffer  = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            RenderSystem.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
        }

        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        /// <param name="state">状態</param>
        public void ProcessMouseInput(KIMouseEventArgs mouse, MOUSE_STATE state)
        {
            switch (state)
            {
                case MOUSE_STATE.DOWN:
                    Controller[CurrentController].Down(mouse);
                    cameraController.Down(mouse);
                    break;
                case MOUSE_STATE.CLICK:
                    Controller[CurrentController].Click(mouse);
                    cameraController.Click(mouse);
                    break;
                case MOUSE_STATE.DOUBLECLICK:
                    Controller[CurrentController].DoubleClick(mouse);
                    cameraController.DoubleClick(mouse);
                    break;
                case MOUSE_STATE.MOVE:
                    Controller[CurrentController].Move(mouse);
                    cameraController.Move(mouse);
                    break;
                case MOUSE_STATE.UP:
                    Controller[CurrentController].Up(mouse);
                    cameraController.Up(mouse);
                    break;
                case MOUSE_STATE.WHEEL:
                    Controller[CurrentController].Wheel(mouse);
                    cameraController.Wheel(mouse);
                    break;
            }
        }
    }
}
