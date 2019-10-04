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
using KI.Gfx.Buffer;
using KI.Renderer;
using KI.Renderer.Technique;
using KI.Foundation.Controller;
using KI.Presenter.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIMaterial;

namespace CADApp.ViewModel
{
    public class ViewportViewModel : ViewportViewModelBase
    {

        public Viewport Viewport
        {
            get
            {
                return Viewport.Instance;
            }
        }

        public ViewportViewModel(ViewModelBase parent)
            : base(parent)
        {
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

        protected override void InitializeDeviceContext()
        {
            DeviceContext.Instance.SetClearColor(1, 1, 1, 1);
            MainScene = new Scene("MainScene", new AppRootNode("Root"));
            RenderSystem = new RenderSystem();
            RenderSystem.ActiveScene = MainScene;

            Workspace.Instance.MainScene = MainScene;
            Workspace.Instance.RenderSystem = RenderSystem;
        }


        protected override void OnResizeEvent(object sender, EventArgs e)
        {
            if (MainScene != null)
            {
                MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
            }
            
            RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        protected override void OnRenderEvent(object sender, PaintEventArgs e)
        {
            RenderSystem.Render();
        }

        protected override void InitializeScene()
        {
            MainScene.MainCamera = new Camera("MainCamera");
            MainScene.MainLight = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);

            var axis = new Axis(Vector3.Zero, MainScene.WorldMax);
            var axisObject = PolygonUtility.CreatePolygon("Axis", axis.Vertex, axis.Color, axis.Index, KIPrimitiveType.Lines);
            MainScene.AddObject(new PolygonNode(axisObject));

            var grid = new GridPlane(1, 0.1f, new Vector3(0.8f));
            Material lineMaterial = new LineMaterial(null, 1);
            var girdObject = PolygonUtility.CreatePolygon("GridPlane", grid.Position, grid.Color, grid.Index, KIPrimitiveType.Lines, lineMaterial);
            MainScene.AddObject(new PolygonNode(girdObject));
        }

        protected override void InitializeRenderer()
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

        protected override void ProcessKeyInput(KeyEventArgs keyEvent)
        {
            Controller[currentControllerType].KeyDown(keyEvent);
            cameraController.KeyDown(keyEvent);
        }

        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        /// <param name="state">状態</param>
        protected override void ProcessMouseInput(KIMouseEventArgs mouse)
        {
            switch (mouse.MouseState)
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
