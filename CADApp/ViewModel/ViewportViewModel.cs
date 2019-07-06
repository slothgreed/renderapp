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
using KI.Foundation.Controller;
using KI.Presentation.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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


        /// <summary>
        /// ノード挿入イベント
        /// </summary>
        public EventHandler OnInitialized;


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
            Viewport.Instance.OnKeyDown += OnKeyDownEvent;

            Controller.Add(ControllerType.Select, new SelectController());
            Controller.Add(ControllerType.SketchLine, new SketchLineController());
            Controller.Add(ControllerType.SketchPrimitive, new SketchPrimitiveController());
            Controller.Add(ControllerType.SketchRectangle, new SketchRectangleController());
            Controller.Add(ControllerType.SketchCurvature, new SketchCurveController());
            Controller.Add(ControllerType.BuildCube, new BuildCubeController());
            Controller.Add(ControllerType.BuildIcosahedron, new BuildIcosahedronController());

            ChangeController(ControllerType.Select, null);

        }

        /// <summary>
        /// カメラコントローラ
        /// </summary>
        private ControllerBase cameraController = new CameraController();

        public Dictionary<ControllerType, ControllerBase> Controller = new Dictionary<ControllerType, ControllerBase>();

        ControllerType currentControllerType;

        public ControllerType CurrentControllerType
        {
            get { return currentControllerType; }
        }

        public ControllerBase CurrentController
        {
            get { return Controller[currentControllerType]; }
        }

        public void ChangeController(ControllerType type, IControllerArgs args)
        {
            Controller[currentControllerType].UnBinding();
            currentControllerType = type;
            Controller[currentControllerType].Binding(args);
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

            InitializeScene();
            InitializeRenderer();

            OnInitialized?.Invoke(this, null);
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

        private void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            ProcessKeyInput(e);
        }

        private void InitializeScene()
        {
            MainScene.MainCamera = AssetFactory.Instance.CreateCamera("MainCamera");
            var light = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            var sphere = AssetFactory.Instance.CreateSphere("sphere", 0.1f, 32, 32, true);
            MainScene.MainLight = new LightNode("SunLight", light, SceneNodeFactory.Instance.CreatePolygonNode("SunLight", sphere, null));
            //MainScene.AddObject(MainScene.MainCamera);
            MainScene.AddObject(MainScene.MainLight);

            var axis = new Axis(Vector3.Zero, MainScene.WorldMax);
            var axisObject = SceneNodeFactory.Instance.CreatePolygonNode("Axis", axis.Vertex, axis.Color, axis.Index, KIPrimitiveType.Lines);
            MainScene.AddObject(axisObject);

            var grid = new GridPlane(1, 0.1f, new Vector3(0.8f));
            var girdObject = SceneNodeFactory.Instance.CreatePolygonNode("GridPlane", grid.Position, grid.Color, grid.Index, KIPrimitiveType.Lines);
            MainScene.AddObject(girdObject);
        }

        private void InitializeRenderer()
        {
            RenderTechniqueFactory.Instance.RendererSystem = RenderSystem;
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));
            var gBufferTexture = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            RenderSystem.OutputBuffer  = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            //RenderSystem.OutputTexture = RenderSystem.RenderQueue.OutputTexture<DeferredRendering>()[0];
            RenderSystem.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];

            GL.LineWidth(2);
        }

        private void ProcessKeyInput(KeyEventArgs keyEvent)
        {
            Controller[currentControllerType].KeyDown(keyEvent);
            cameraController.KeyDown(keyEvent);
        }

        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        /// <param name="state">状態</param>
        private void ProcessMouseInput(KIMouseEventArgs mouse, MOUSE_STATE state)
        {
            switch (state)
            {
                case MOUSE_STATE.DOWN:
                    Controller[CurrentControllerType].Down(mouse);
                    cameraController.Down(mouse);
                    break;
                case MOUSE_STATE.CLICK:
                    Controller[CurrentControllerType].Click(mouse);
                    cameraController.Click(mouse);
                    break;
                case MOUSE_STATE.DOUBLECLICK:
                    Controller[CurrentControllerType].DoubleClick(mouse);
                    cameraController.DoubleClick(mouse);
                    break;
                case MOUSE_STATE.MOVE:
                    Controller[CurrentControllerType].Move(mouse);
                    cameraController.Move(mouse);
                    break;
                case MOUSE_STATE.UP:
                    Controller[CurrentControllerType].Up(mouse);
                    cameraController.Up(mouse);
                    break;
                case MOUSE_STATE.WHEEL:
                    Controller[CurrentControllerType].Wheel(mouse);
                    cameraController.Wheel(mouse);
                    break;
            }
        }
    }
}
