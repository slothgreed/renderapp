using KI.Asset;
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

namespace ImageProcessor.ViewModel
{
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

        public void OnLoadedEvent(object sender, EventArgs e)
        {
            DeviceContext.Instance.SetClearColor(1, 1, 1, 1);
            MainScene = new Scene("MainScene", new EmptyNode("Root"));
            MainScene.MainCamera = new Camera("Camera");
            MainScene.MainLight = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            RenderSystem = new RenderSystem();
            RenderSystem.ActiveScene = MainScene;

            var rectangle = new Rectangle();
            var rectangleObject = SceneNodeFactory.Instance.CreatePolygonNode("Axis", rectangle.Position, rectangle.Position, rectangle.Texcoord, rectangle.Index, KIPrimitiveType.Quads);
            var mainTexture = TextureFactory.Instance.CreateTexture("E:\\cgModel\\Image\\Contact_Cover.jpg");
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.Albedo);
            var textures = new Dictionary<TextureKind, Texture>();
            textures.Add(TextureKind.Albedo, mainTexture);

            rectangleObject.Polygon.Material = new Material(shader, textures);
            MainScene.AddObject(rectangleObject);

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
    }
}
