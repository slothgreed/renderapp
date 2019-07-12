using KI.Asset;
using KI.Asset.Primitive;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using KI.Renderer;
using KI.Renderer.Technique;
using KI.Presenter.ViewModel;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Linq;

namespace ShaderTraining.ViewModel
{

    public enum VisibleItem
    {
        Plane,
        Sphere
    }

    /// <summary>
    /// 初期化イベント
    /// </summary>
    public delegate void OnInitializedHandler(object sender, EventArgs e);

    public class ViewportViewModel
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

        public event OnInitializedHandler OnInitialized;

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
        {
            Viewport.Instance.OnLoaded += OnLoadedEvent;
            Viewport.Instance.OnResize += OnResizeEvent;
            Viewport.Instance.OnRender += OnRenderEvent;
        }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene MainScene { get; set; }

        /// <summary>
        /// レンダラー
        /// </summary>
        public RenderSystem RenderSystem { get; set; }

        private CameraMode cameraMode;
        public CameraMode CameraMode
        {
            get
            {
                return cameraMode;
            }
            private set
            {
                cameraMode = value;
                MainScene.MainCamera.InitCamera(value);
            }
        }

        public void OnLoadedEvent(object sender, EventArgs e)
        {
            DeviceContext.Instance.SetClearColor(1, 1, 1, 1);
            MainScene = new Scene("MainScene", new EmptyNode("Root"));
            MainScene.MainCamera = new Camera("Camera");
            MainScene.MainLight = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            RenderSystem = new RenderSystem();
            RenderSystem.ActiveScene = MainScene;

            var mainTexture = TextureFactory.Instance.CreateTexture("E:\\cgModel\\Image\\Contact_Cover.jpg");

            var rectangle = new Rectangle();
            var rectangleObject = SceneNodeFactory.Instance.CreatePolygonNode("Rectangle", rectangle.Position, rectangle.Position, rectangle.Texcoord, rectangle.Index, KIPrimitiveType.Quads);
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.Albedo);
            var textures = new Dictionary<TextureKind, Texture>();
            textures.Add(TextureKind.Albedo, mainTexture);

            rectangleObject.Polygon.Material = new Material(shader, textures);
            MainScene.AddObject(rectangleObject);
            CameraMode = CameraMode.Ortho;


            var icosahedron = new Icosahedron(0.5f, 1, Vector3.Zero);
            var sphereObject = SceneNodeFactory.Instance.CreatePolygonNode("Sphere", icosahedron.Position, icosahedron.Normal, icosahedron.Index, KIPrimitiveType.Triangles);
            sphereObject.Visible = false;
            var sphereShader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            sphereObject.Polygon.Material = new Material(sphereShader);
            MainScene.AddObject(sphereObject);


            InitializeRenderer();

            OnInitialized?.Invoke(this, null);
        }


        private void InitializeRenderer()
        {
            RenderTechniqueFactory.Instance.RendererSystem = RenderSystem;
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));
            var gBufferTexture = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            RenderSystem.OutputBuffer = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            //RenderSystem.OutputTexture = RenderSystem.RenderQueue.OutputTexture<DeferredRendering>()[0];
            RenderSystem.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];

            Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            bloom.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            RenderSystem.PostEffect.AddTechnique(bloom);

            Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            sobel.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            RenderSystem.PostEffect.AddTechnique(sobel);

            GrayScale grayScale = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GrayScale) as GrayScale;
            grayScale.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            RenderSystem.PostEffect.AddTechnique(grayScale);
        }

        private void OnResizeEvent(object sender, EventArgs e)
        {
            if (MainScene != null)
            {
                if (CameraMode == CameraMode.Perspective)
                {
                    MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
                }
            }

            RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        private void OnRenderEvent(object sender, PaintEventArgs e)
        {
            RenderSystem.Render();
        }

        public void Invalidate()
        {
            Viewport.Instance.GLControl_Paint(this, null);
        }

        public void ChangeVisible(VisibleItem item)
        {
            foreach (var node in MainScene.RootNode.AllChildren())
            {
                if(node is SceneNode)
                {
                    var sceneNode = (SceneNode)node;
                    sceneNode.Visible = false;
                }
            }

            if (item == VisibleItem.Plane)
            {
                var sceneNode = MainScene.FindNode("Rectangle");
                sceneNode.Visible = true;
                CameraMode = CameraMode.Ortho;
            }
            else if (item == VisibleItem.Sphere)
            {
                var sceneNode = MainScene.FindNode("Sphere");
                sceneNode.Visible = true;
                CameraMode = CameraMode.Perspective;
            }

            Invalidate();
        }
    }
}
