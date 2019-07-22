using System.Collections.ObjectModel;
using System.Linq;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Foundation.Tree;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;
using KI.Renderer;
using KI.Renderer.Technique;
using KI.Foundation.Command;
using KI.Presenter.ViewModel;
using OpenTK;
using RenderApp.Model;
using RenderApp.Tool;
using RenderApp.Tool.Command;
using KI.Asset.Primitive;

namespace RenderApp.ViewModel
{
    public partial class WorkspaceViewModel : ViewModelBase
    {
        public Workspace workspace;
        public RootNodeViewModel SceneNodeViewModel;
        public RootNodeViewModel FactoryNodeViewModel;
        public RendererViewModel RendererViewModel;
        public ViewportViewModel ViewportViewModel;
        public PropertyGridViewModel PropertyGridViewModel;

        public WorkspaceViewModel(ViewModelBase parent, Workspace workspace)
            : base(parent, workspace)
        {
            SceneNodeViewModel = new RootNodeViewModel(this, workspace.MainScene.RootNode, "Scene");
            FactoryNodeViewModel = new RootNodeViewModel(this, new KINode("Library"), "Library");
            ViewportViewModel = new ViewportViewModel(this);
            RendererViewModel = new RendererViewModel(this);
            PropertyGridViewModel = new PropertyGridViewModel(this, null);

            RendererViewModel.PropertyChanged += RendererViewModel_PropertyChanged;
            SceneNodeViewModel.PropertyChanged += SceneNodeViewModel_PropertyChanged;
            FactoryNodeViewModel.PropertyChanged += LibraryNodeViewModel_PropertyChanged;
            ViewportViewModel.PropertyChanged += ViewportViewModel_PropertyChanged;
            ViewportViewModel.ItemSelected += ViewportViewModel_ItemSelected;
            PropertyGridViewModel.PropertyChanged += PropertyGridViewModel_PropertyChanged;

            AnchorablesSources = new ObservableCollection<ViewModelBase>();
            DocumentsSources = new ObservableCollection<ViewModelBase>();
            AnchorablesSources.Add(SceneNodeViewModel);
            AnchorablesSources.Add(FactoryNodeViewModel);
            AnchorablesSources.Add(RendererViewModel);
            AnchorablesSources.Add(PropertyGridViewModel);
            DocumentsSources.Add(ViewportViewModel);

            this.workspace = workspace;
        }

        private void ViewportViewModel_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            PropertyGridViewModel.Model = e.SelectItem;
        }

        private void ViewportViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Loaded")
            {
                Initialize();
                RendererViewModel.Model = workspace.RenderSystem;
            }
            else if (e.PropertyName == "Resize")
            {
                workspace.MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
                workspace.RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
            }
            else if (e.PropertyName == "Renderer")
            {
                workspace.RenderSystem.Render();
            }
        }

        private void SceneNodeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SceneNodeViewModel.ActiveNode))
            {
                UpdateSelectNode(SceneNodeViewModel.ActiveNode.Model as SceneNode);
            }

            ViewportViewModel.Invalidate();
        }

        private void LibraryNodeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }

        private void RendererViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }

        private void PolygonNodeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }

        private void PropertyGridViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }



        public void UpdateSelectNode(SceneNode node)
        {
            if (node == null)
            {
                return;
            }

            DockWindowViewModel vm = null;
            if (node is AnalyzePolygonNode)
            {
                vm = new AnalyzePolygonNodeViewModel(this, node as AnalyzePolygonNode);
                vm.PropertyChanged += PolygonNodeViewModel_PropertyChanged;
                workspace.MainScene.SelectNode = node;
            }
            else if (node is LightNode)
            {
                vm = new LightViewModel(this, node as LightNode);
                vm.PropertyChanged += PolygonNodeViewModel_PropertyChanged;
                workspace.MainScene.SelectNode = node;
            }

            ReplaceTabWindow(vm);
        }

        public void ReplaceTabWindow(DockWindowViewModel window)
        {
            if (window is SceneNodeViewModel)
            {
                var oldItem = AnchorablesSources.FirstOrDefault(p => p is SceneNodeViewModel);
                AnchorablesSources.Add(window);
                AnchorablesSources.Remove(oldItem);
            }
        }

        public void OpenWindow(AppWindow windowType)
        {
        }

        private void Initialize()
        {
            InitializeScene();
            InitializeRenderer(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void InitializeScene()
        {
            Scene mainScene = workspace.MainScene;

            mainScene.MainCamera = new Camera("MainCamera");
            mainScene.MainLight = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);

            var axis = new Axis(Vector3.Zero, mainScene.WorldMax);
            var axisObject = SceneNodeFactory.Instance.CreatePolygonNode(axis.ToString(), axis.Vertex, axis.Color, axis.Index, KIPrimitiveType.Lines);
            mainScene.AddObject(axisObject);

            //var sponzas = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/crytek-sponza/sponza.obj");
            //var sponzaObject = SceneNodeFactory.Instance.CreatePolygonNodes("sponza",sponzas);
            //foreach (var sponza in sponzaObject)
            //{
            //    AddObject(sponza);
            //}

            //List<PolygonNode> ducks = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/duck/duck.obj");
            //foreach (var duck in ducks)
            //{
            //    duck.RotateX(-90);
            //    duck.RotateY(0);
            //    ActiveScene.AddObject(duck);
            //}

            CreateEnvironmentCube(mainScene.WorldMin * 5, mainScene.WorldMax * 5);

            // bunny
            {
                //var moai = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/cube.half");
                var moai = AssetFactory.Instance.CreateLoad3DModel(@"E:\develop\cgal\build\test\Surface_mesh_segmentation\Debug\data\cactus.off");
                var renderBunny = CreateAnalyzePolygonNode("cube", moai);

                renderBunny.Polygon.Material.Shader = ShaderCreater.Instance.CreateShader(GBufferType.PointNormalColor);
                //renderBunny.RotateX(-90);
                mainScene.AddObject(renderBunny);
                var parentNode = mainScene.FindNode(renderBunny);

                // bunny attribute
                //var attribute = new KI.Asset.Attribute.OutlineAttribute(renderBunny.Name + "Outline",
                //renderBunny.VertexBuffer.ShallowCopy(),
                //renderBunny.Polygon.Type,
                //ShaderCreater.Instance.CreateShader(ShaderType.Outline));
                //renderBunny.Attributes.Add(attribute);
                //mainScene.AddObject(attribute, parentNode);

                //var splitAttribute = new KI.Asset.Attribute.SplitAttribute(
                //    renderBunny.Name + "Split",
                //    renderBunny.VertexBuffer.ShallowCopy(),
                //    ShaderCreater.Instance.CreateShader(ShaderType.Split));
                //renderBunny.Attributes.Add(splitAttribute);
                //mainScene.AddObject(splitAttribute, parentNode);

                //MainWindowViewModel.Instance.CommandManager.Execute(new CreateWireFrameCommand(new WireFrameCommandArgs(renderBunny, mainScene, Vector4.Zero)), false);
                //MainWindowViewModel.Instance.CommandManager.Execute(new VertexCurvatureCommand(new VertexCurvatureCommandArgs(renderBunny, mainScene)));

                //var vectorFiledAttribute = new VectorFieldAttribute(
                //    renderBunny.Name + ": VectorField",
                //    renderBunny.VertexBuffer.ShallowCopy(),
                //    new Material(ShaderCreater.Instance.CreateShader(SHADER_TYPE.VectorField)),
                //    renderBunny.Attributes.OfType<VertexDirectionAttribute>().First().Direction,
                //    renderBunny.Type);
                //renderBunny.Attributes.Add(vectorFiledAttribute);

                //var icosahedron = AssetFactory.Instance.CreateIcosahedron("Icosahedron", 0.5f, 1);
                //var renderIcosahedron = PolygonNodeFactory.Instance.CreatePolygonNode("Icosahedron", icosahedron);
                //mainScene.AddObject(renderIcosahedron);

                //CommandManager.Instance.Execute(new CreateWireFrameCommand(new WireFrameCommandArgs(renderIcosahedron, mainScene, Vector3.Zero)), false);
            }

            // plane
            {
                //var plane = AssetFactory.Instance.CreatePlane("plane").Polygons.First();
                //var renderPlane = PolygonNodeFactory.Instance.CreatePolygonNode("plane", plane);
                //mainScene.AddObject(renderPlane);

                //var normal = TextureFactory.Instance.CreateTexture(@"E:\MyProgram\KIProject\renderapp\Resource\Texture\Displacement\Normal.png");
                //var height = TextureFactory.Instance.CreateTexture(@"E:\MyProgram\KIProject\renderapp\Resource\Texture\Displacement\Height.png");
                //plane.AddTexture(KI.Gfx.KITexture.TextureKind.Normal, normal);
                //plane.AddTexture(KI.Gfx.KITexture.TextureKind.Height, height);
                //var splitAttribute = new KI.Renderer.Attribute.DisplacementAttribute(
                //    renderPlane.Name + "Split",
                //    renderPlane.VertexBuffer.ShallowCopy(),
                //    ShaderCreater.Instance.CreateShader(ShaderType.Split));
                //renderPlane.Attributes.Add(splitAttribute);

                //var parentNode = mainScene.FindNode(renderPlane);
                //mainScene.AddObject(splitAttribute, parentNode);

            }
            //CommandManager.Instance.Execute(new CalculateVertexCurvatureCommand(mainScene, renderBunny), null, false);
        }

        private AnalyzePolygonNode CreateAnalyzePolygonNode(string name, ICreateModel model)
        {
            string vert = ShaderCreater.Instance.GetVertexShader(model.Model.Type, null);
            string frag = ShaderCreater.Instance.GetFragShaderFilePath(model.Model.Type, null);
            var shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
            Material material = new Material(shader);
            model.Model.Material = material;
            AnalyzePolygonNode node = new AnalyzePolygonNode(name, model.Model);
            return node;
        }

        private void CreateEnvironmentCube(Vector3 min, Vector3 max)
        {
            string[] cubemap = new string[6];
            cubemap[0] = ProjectInfo.TextureDirectory + @"\cubemap\posx.jpg";
            cubemap[1] = ProjectInfo.TextureDirectory + @"\cubemap\posy.jpg";
            cubemap[2] = ProjectInfo.TextureDirectory + @"\cubemap\posz.jpg";
            cubemap[3] = ProjectInfo.TextureDirectory + @"\cubemap\negx.jpg";
            cubemap[4] = ProjectInfo.TextureDirectory + @"\cubemap\negy.jpg";
            cubemap[5] = ProjectInfo.TextureDirectory + @"\cubemap\negz.jpg";

            var pxTexture = TextureFactory.Instance.CreateTexture(cubemap[0]);
            var pyTexture = TextureFactory.Instance.CreateTexture(cubemap[1]);
            var pzTexture = TextureFactory.Instance.CreateTexture(cubemap[2]);
            var nxTexture = TextureFactory.Instance.CreateTexture(cubemap[3]);
            var nyTexture = TextureFactory.Instance.CreateTexture(cubemap[4]);
            var nzTexture = TextureFactory.Instance.CreateTexture(cubemap[5]);

            Vector3 v0 = new Vector3(min.X, min.Y, min.Z);
            Vector3 v1 = new Vector3(max.X, min.Y, min.Z);
            Vector3 v2 = new Vector3(max.X, max.Y, min.Z);
            Vector3 v3 = new Vector3(min.X, max.Y, min.Z);
            Vector3 v4 = new Vector3(min.X, min.Y, max.Z);
            Vector3 v5 = new Vector3(max.X, min.Y, max.Z);
            Vector3 v6 = new Vector3(max.X, max.Y, max.Z);
            Vector3 v7 = new Vector3(min.X, max.Y, max.Z);

            Rectangle front = new Rectangle(v2, v3, v0, v1);
            Rectangle left = new Rectangle(v3, v7, v4, v0);
            Rectangle back = new Rectangle(v7, v6, v5, v4);
            Rectangle right = new Rectangle(v6, v2, v1, v5);
            Rectangle top = new Rectangle(v3, v2, v6, v7);
            Rectangle bottom = new Rectangle( v0, v4, v5, v1);
            var frontMaterial = new Material();
            frontMaterial.AddTexture(TextureKind.Albedo, nzTexture);
            var leftMaterial = new Material();
            leftMaterial.AddTexture(TextureKind.Albedo, pxTexture);
            var backMaterial = new Material();
            backMaterial.AddTexture(TextureKind.Albedo, pzTexture);
            var rightMaterial = new Material();
            rightMaterial.AddTexture(TextureKind.Albedo, nxTexture);
            var topMaterial = new Material();
            topMaterial.AddTexture(TextureKind.Albedo, pyTexture);
            var bottomMaterial = new Material();
            bottomMaterial.AddTexture(TextureKind.Albedo, nyTexture);

            PolygonNode renderFront = SceneNodeFactory.Instance.CreatePolygonNode("front", front, frontMaterial);
            PolygonNode renderLeft = SceneNodeFactory.Instance.CreatePolygonNode("left", left, leftMaterial);
            PolygonNode renderBack = SceneNodeFactory.Instance.CreatePolygonNode("back", back, backMaterial);
            PolygonNode renderRight = SceneNodeFactory.Instance.CreatePolygonNode("right", right, rightMaterial);
            PolygonNode renderTop = SceneNodeFactory.Instance.CreatePolygonNode("top", top, topMaterial);
            PolygonNode renderBottom = SceneNodeFactory.Instance.CreatePolygonNode("bottom", bottom, bottomMaterial);

            EmptyNode cubeMapNode = new EmptyNode("CubeMap");

            Scene mainScene = workspace.MainScene;

            mainScene.AddObject(cubeMapNode);
            mainScene.AddObject(renderFront, cubeMapNode);
            mainScene.AddObject(renderLeft, cubeMapNode);
            mainScene.AddObject(renderBack, cubeMapNode);
            mainScene.AddObject(renderRight, cubeMapNode);
            mainScene.AddObject(renderTop, cubeMapNode);
            mainScene.AddObject(renderBottom, cubeMapNode);

            EnvironmentProbe cubeMap = new EnvironmentProbe("CubeMapObject");
            cubeMap.GenCubemap(cubemap[0], cubemap[1], cubemap[2], cubemap[3], cubemap[4], cubemap[5]);
            mainScene.AddObject(cubeMap);
        }

        public void InitializeRenderer(int width, int height)
        {
            var renderer = workspace.RenderSystem;
            RenderTechniqueFactory.Instance.RendererSystem = renderer;
            //renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Shadow));
            renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));

            var gBufferTexture = renderer.RenderQueue.OutputTexture<GBuffer>();

            //renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL));
            renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            //renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Selection));

            renderer.OutputBuffer = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            renderer.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];

            Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            bloom.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            renderer.PostEffect.AddTechnique(bloom);

            Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            sobel.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            renderer.PostEffect.AddTechnique(sobel);

            //SSAO ssao = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSAO) as SSAO;
            //ssao.uPosition = gBufferTexture[(int)GBuffer.OutputTextureType.Posit];
            //ssao.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            //renderer.PostEffect.AddTechnique(ssao);

            //SSLIC sslic = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSLIC) as SSLIC;
            //renderer.PostEffect.AddTechnique(sslic);

            var probe = workspace.MainScene.FindNode("CubeMapObject") as EnvironmentProbe;
            ImageBasedLighting ibl = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL) as ImageBasedLighting;
            renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL));
            ibl.uCubeMap = probe.Cubemap;
        }

    }
}
