using System.Linq;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Asset.Technique;
using KI.Foundation.Command;
using KI.Tool.Command;
using OpenTK;

namespace RenderApp.Globals
{
    /// <summary>
    /// ワークスペース
    /// </summary>
    public class Workspace
    {
        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static Workspace instance;

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static Workspace Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Workspace();
                    instance.Initialize();
                }

                return instance;
            }
        }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene MainScene { get; set; }

        /// <summary>
        /// レンダラー
        /// </summary>
        public Renderer Renderer { get; set; }

        public void Initialize()
        {
            MainScene = new Scene("MainScene");
            Renderer = new Renderer();
            Global.Renderer = Renderer;
            Global.Renderer.ActiveScene = MainScene;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void InitializeScene()
        {
            MainScene.Initialize();
            var axis = AssetFactory.Instance.CreateAxis("axis", Vector3.Zero, MainScene.WorldMax);
            var axisObject = RenderObjectFactory.Instance.CreateRenderObject(axis.ToString(), axis);
            MainScene.AddObject(axisObject);

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

            //List<RenderObject> worlds = AssetFactory.Instance.CreateWorld("World", ActiveScene.WorldMin, ActiveScene.WorldMax);
            //string[] paths = new string[]{
            //        ProjectInfo.TextureDirectory + @"\cubemap\posx.jpg",
            //        ProjectInfo.TextureDirectory + @"\cubemap\posy.jpg",
            //        ProjectInfo.TextureDirectory + @"\cubemap\posz.jpg",
            //        ProjectInfo.TextureDirectory + @"\cubemap\negx.jpg",
            //        ProjectInfo.TextureDirectory + @"\cubemap\negy.jpg",
            //        ProjectInfo.TextureDirectory + @"\cubemap\negz.jpg"
            //};

            //for (int i = 0; i < 6; i++)
            //{
            //    var texture = TextureFactory.Instance.CreateTexture(paths[i]);
            //    worlds[i].AddTexture(TextureKind.Albedo, texture);
            //    ActiveScene.AddObject(worlds[i]);
            //}
            //EnvironmentProbe cubeMap = AssetFactory.Instance.CreateEnvironmentMap("world");
            //cubeMap.GenCubemap(paths);
            //ActiveScene.AddObject(cubeMap);

            // bunny
            {
                var bunny = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/moai.half");
                var renderBunny = RenderObjectFactory.Instance.CreateRenderObject("bunny", bunny);
                renderBunny.Shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                //renderBunny.RotateX(-90);
                MainScene.AddObject(renderBunny);
                var parentNode = MainScene.FindNode(renderBunny);

                // bunny attribute
                //var attribute = new KI.Asset.Attribute.OutlineAttribute(renderBunny.Name + "Outline",
                //renderBunny.VertexBuffer.ShallowCopy(),
                //renderBunny.Polygon.Type,
                //ShaderCreater.Instance.CreateShader(ShaderType.Outline));
                //renderBunny.Attributes.Add(attribute);
                //MainScene.AddObject(attribute, parentNode);

                //var splitAttribute = new KI.Asset.Attribute.SplitAttribute(
                //    renderBunny.Name + "Split",
                //    renderBunny.VertexBuffer.ShallowCopy(),
                //    ShaderCreater.Instance.CreateShader(ShaderType.Split));
                //renderBunny.Attributes.Add(splitAttribute);
                //MainScene.AddObject(splitAttribute, parentNode);

                CommandManager.Instance.Execute(new CreateWireFrameCommand(new WireFrameCommandArgs(renderBunny, MainScene, Vector3.Zero)), false);
                CommandManager.Instance.Execute(new VertexCurvatureCommand(new VertexCurvatureCommandArgs(renderBunny, MainScene)));

                var vectorFiledAttribute = new VectorFieldAttribute(
                    renderBunny.Name + ": VectorField",
                    renderBunny.VertexBuffer.ShallowCopy(),
                    ShaderCreater.Instance.CreateShader(ShaderType.VectorField),
                    renderBunny.Attributes.OfType<VertexDirectionAttribute>().First().Direction,
                    renderBunny.Polygon.Type);
                renderBunny.Attributes.Add(vectorFiledAttribute);

                //var icosahedron = AssetFactory.Instance.CreateIcosahedron("Icosahedron", 0.5f, 1);
                //var renderIcosahedron = RenderObjectFactory.Instance.CreateRenderObject("Icosahedron", icosahedron);
                //MainScene.AddObject(renderIcosahedron);

                //CommandManager.Instance.Execute(new CreateWireFrameCommand(new WireFrameCommandArgs(renderIcosahedron, MainScene, Vector3.Zero)), false);
            }

            // plane
            {
                //var plane = AssetFactory.Instance.CreatePlane("plane").Polygons.First();
                //var renderPlane = RenderObjectFactory.Instance.CreateRenderObject("plane", plane);
                //MainScene.AddObject(renderPlane);

                //var normal = TextureFactory.Instance.CreateTexture(@"E:\MyProgram\KIProject\renderapp\Resource\Texture\Displacement\Normal.png");
                //var height = TextureFactory.Instance.CreateTexture(@"E:\MyProgram\KIProject\renderapp\Resource\Texture\Displacement\Height.png");
                //plane.AddTexture(KI.Gfx.KITexture.TextureKind.Normal, normal);
                //plane.AddTexture(KI.Gfx.KITexture.TextureKind.Height, height);
                //var splitAttribute = new KI.Renderer.Attribute.DisplacementAttribute(
                //    renderPlane.Name + "Split",
                //    renderPlane.VertexBuffer.ShallowCopy(),
                //    ShaderCreater.Instance.CreateShader(ShaderType.Split));
                //renderPlane.Attributes.Add(splitAttribute);

                //var parentNode = MainScene.FindNode(renderPlane);
                //MainScene.AddObject(splitAttribute, parentNode);

            }
            //CommandManager.Instance.Execute(new CalculateVertexCurvatureCommand(MainScene, renderBunny), null, false);
        }

        public void InitializeRenderer(int width, int height)
        {
            //RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Shadow));
            Renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));

            var gBufferTexture = Renderer.RenderQueue.OutputTexture<GBuffer>();

            //RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL));
            Renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            //RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Selection));

            Renderer.OutputBuffer = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            Renderer.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];

            //Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            //bloom.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            //Renderer.PostEffect.AddTechnique(bloom);

            //Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            //sobel.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            //Renderer.PostEffect.AddTechnique(sobel);

            //SSAO ssao = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSAO) as SSAO;
            //ssao.uPosition = gBufferTexture[(int)GBuffer.OutputTextureType.Posit];
            //ssao.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            //Renderer.PostEffect.AddTechnique(ssao);

            //SSLIC sslic = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSLIC) as SSLIC;
            //Renderer.PostEffect.AddTechnique(sslic);
        }
    }
}
