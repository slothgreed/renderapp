using System.Collections.ObjectModel;
using System.Linq;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Asset.Technique;
using KI.Foundation.Tree;
using KI.Gfx.GLUtil;
using KI.Tool.Command;
using KI.UI.ViewModel;
using OpenTK;
using RenderApp.Model;
using RenderApp.Tool;
using RenderApp.Tool.Command;

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
                RendererViewModel.Model = workspace.Renderer;
            }
            else if (e.PropertyName == "Resize")
            {
                workspace.MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
                workspace.Renderer.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
            }
            else if (e.PropertyName == "Renderer")
            {
                workspace.Renderer.Render();
            }
        }

        private void SceneNodeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SceneNodeViewModel.ActiveNode))
            {
                UpdateSelectNode(SceneNodeViewModel.ActiveNode.Model);
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

        private void RenderObjectViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }

        private void PropertyGridViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }



        public void UpdateSelectNode(KINode node)
        {
            if (node.KIObject == null)
            {
                return;
            }

            DockWindowViewModel vm = null;
            if (node.KIObject is RenderObject)
            {
                vm = new RenderObjectViewModel(this, node.KIObject as RenderObject);
                vm.PropertyChanged += RenderObjectViewModel_PropertyChanged;
                workspace.MainScene.SelectNode = (SceneNode)node.KIObject;
            }
            else if (node.KIObject is Light)
            {
                vm = new LightViewModel(this, node.KIObject as Light);
                vm.PropertyChanged += RenderObjectViewModel_PropertyChanged;
                workspace.MainScene.SelectNode = (SceneNode)node.KIObject;
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

            mainScene.MainCamera = AssetFactory.Instance.CreateCamera("MainCamera");
            mainScene.SunLight = RenderObjectFactory.Instance.CreateDirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            var sphere = AssetFactory.Instance.CreateSphere("sphere", 0.1f, 32, 32, true);
            mainScene.SunLight.Model = RenderObjectFactory.Instance.CreateRenderObject("SunLight", sphere);
            mainScene.AddObject(mainScene.MainCamera);
            mainScene.AddObject(mainScene.SunLight);

            var axis = AssetFactory.Instance.CreateAxis("axis", Vector3.Zero, mainScene.WorldMax);
            var axisObject = RenderObjectFactory.Instance.CreateRenderObject(axis.ToString(), axis);
            mainScene.AddObject(axisObject);

            //var sponzas = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/crytek-sponza/sponza.obj");
            //var sponzaObject = RenderObjectFactory.Instance.CreateRenderObjects("sponza",sponzas);
            //foreach (var sponza in sponzaObject)
            //{
            //    AddObject(sponza);
            //}

            //List<RenderObject> ducks = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/duck/duck.obj");
            //foreach (var duck in ducks)
            //{
            //    duck.RotateX(-90);
            //    duck.RotateY(0);
            //    ActiveScene.AddObject(duck);
            //}

            CreateEnvironmentCube(mainScene.WorldMin * 5, mainScene.WorldMax * 5);

            // bunny
            {
                var moai = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/moai.half");
                var renderBunny = RenderObjectFactory.Instance.CreateRenderObject("moai", moai);
                renderBunny.Shader = ShaderCreater.Instance.CreateShader(GBufferType.PointNormalColor);
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

                CommandManager.Instance.Execute(new CreateWireFrameCommand(new WireFrameCommandArgs(renderBunny, mainScene, Vector3.Zero)), false);
                CommandManager.Instance.Execute(new VertexCurvatureCommand(new VertexCurvatureCommandArgs(renderBunny, mainScene)));

                var vectorFiledAttribute = new VectorFieldAttribute(
                    renderBunny.Name + ": VectorField",
                    renderBunny.VertexBuffer.ShallowCopy(),
                    ShaderCreater.Instance.CreateShader(ShaderType.VectorField),
                    renderBunny.Attributes.OfType<VertexDirectionAttribute>().First().Direction,
                    renderBunny.Type);
                renderBunny.Attributes.Add(vectorFiledAttribute);

                //var icosahedron = AssetFactory.Instance.CreateIcosahedron("Icosahedron", 0.5f, 1);
                //var renderIcosahedron = RenderObjectFactory.Instance.CreateRenderObject("Icosahedron", icosahedron);
                //mainScene.AddObject(renderIcosahedron);

                //CommandManager.Instance.Execute(new CreateWireFrameCommand(new WireFrameCommandArgs(renderIcosahedron, mainScene, Vector3.Zero)), false);
            }

            // plane
            {
                //var plane = AssetFactory.Instance.CreatePlane("plane").Polygons.First();
                //var renderPlane = RenderObjectFactory.Instance.CreateRenderObject("plane", plane);
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

            Rectangle front = new Rectangle("Front", v2, v3, v0, v1);
            front.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, nzTexture);

            Rectangle left = new Rectangle("Left", v3, v7, v4, v0);
            left.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, pxTexture);

            Rectangle back = new Rectangle("Back", v7, v6, v5, v4);
            back.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, pzTexture);

            Rectangle right = new Rectangle("Right", v6, v2, v1, v5);
            right.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, nxTexture);

            Rectangle top = new Rectangle("Top", v3, v2, v6, v7);
            top.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, pyTexture);

            Rectangle bottom = new Rectangle("Bottom", v0, v4, v5, v1);
            bottom.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, nyTexture);

            RenderObject renderFront = RenderObjectFactory.Instance.CreateRenderObject(front.Name, front);
            RenderObject renderLeft = RenderObjectFactory.Instance.CreateRenderObject(left.Name, left);
            RenderObject renderBack = RenderObjectFactory.Instance.CreateRenderObject(back.Name, back);
            RenderObject renderRight = RenderObjectFactory.Instance.CreateRenderObject(right.Name, right);
            RenderObject renderTop = RenderObjectFactory.Instance.CreateRenderObject(top.Name, top);
            RenderObject renderBottom = RenderObjectFactory.Instance.CreateRenderObject(bottom.Name, bottom);

            KINode cubeMapNode = new KINode("CubeMap");

            Scene mainScene = workspace.MainScene;

            mainScene.AddObject(cubeMapNode);
            mainScene.AddObject(renderFront, cubeMapNode);
            mainScene.AddObject(renderLeft, cubeMapNode);
            mainScene.AddObject(renderBack, cubeMapNode);
            mainScene.AddObject(renderRight, cubeMapNode);
            mainScene.AddObject(renderTop, cubeMapNode);
            mainScene.AddObject(renderBottom, cubeMapNode);

            EnvironmentProbe cubeMap = AssetFactory.Instance.CreateEnvironmentMap("CubeMapObject");
            cubeMap.GenCubemap(cubemap[0], cubemap[1], cubemap[2], cubemap[3], cubemap[4], cubemap[5]);
            mainScene.AddObject(cubeMap);
        }

        public void InitializeRenderer(int width, int height)
        {
            var renderer = workspace.Renderer;
            RenderTechniqueFactory.Instance.Renderer = renderer;
            //renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Shadow));
            renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));

            var gBufferTexture = renderer.RenderQueue.OutputTexture<GBuffer>();

            //renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL));
            renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            //renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Selection));

            renderer.OutputBuffer = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            renderer.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];

            //Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            //bloom.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            //renderer.PostEffect.AddTechnique(bloom);

            //Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            //sobel.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            //renderer.PostEffect.AddTechnique(sobel);

            //SSAO ssao = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSAO) as SSAO;
            //ssao.uPosition = gBufferTexture[(int)GBuffer.OutputTextureType.Posit];
            //ssao.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            //renderer.PostEffect.AddTechnique(ssao);

            //SSLIC sslic = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSLIC) as SSLIC;
            //renderer.PostEffect.AddTechnique(sslic);

            var probe = workspace.MainScene.FindObject("CubeMapObject") as EnvironmentProbe;
            ImageBasedLighting ibl = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL) as ImageBasedLighting;
            renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL));
            ibl.uCubeMap = probe.Cubemap;
        }

    }
}
