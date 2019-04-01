using System.Linq;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Asset.Technique;
using KI.Foundation.Tree;
using RenderApp.Tool.Command;
using OpenTK;
using KI.Tool.Command;

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
            MainScene.MainCamera = AssetFactory.Instance.CreateCamera("MainCamera");
            MainScene.SunLight = RenderObjectFactory.Instance.CreateDirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            var sphere = AssetFactory.Instance.CreateSphere("sphere", 0.1f, 32, 32, true);
            MainScene.SunLight.Model = RenderObjectFactory.Instance.CreateRenderObject("SunLight", sphere);
            MainScene.AddObject(MainScene.MainCamera);
            MainScene.AddObject(MainScene.SunLight);

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

            CreateEnvironmentCube(MainScene.WorldMin * 5, MainScene.WorldMax * 5);

            // bunny
            {
                var moai = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/moai.half");
                var renderBunny = RenderObjectFactory.Instance.CreateRenderObject("moai", moai);
                renderBunny.Shader = ShaderCreater.Instance.CreateShader(GBufferType.PointNormalColor);
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

            Rectangle front  = new Rectangle("Front", v2, v3, v0, v1);
            front.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, nzTexture);

            Rectangle left   = new Rectangle("Left", v3, v7, v4, v0);
            left.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, pxTexture);

            Rectangle back   = new Rectangle("Back", v7, v6, v5, v4);
            back.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, pzTexture);

            Rectangle right  = new Rectangle("Right", v6, v2, v1, v5);
            right.Model.AddTexture(KI.Gfx.KITexture.TextureKind.Albedo, nxTexture);

            Rectangle top    = new Rectangle("Top", v3, v2, v6, v7);
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
            MainScene.AddObject(cubeMapNode);
            MainScene.AddObject(renderFront, cubeMapNode);
            MainScene.AddObject(renderLeft, cubeMapNode);
            MainScene.AddObject(renderBack, cubeMapNode);
            MainScene.AddObject(renderRight, cubeMapNode);
            MainScene.AddObject(renderTop, cubeMapNode);
            MainScene.AddObject(renderBottom, cubeMapNode);

            EnvironmentProbe cubeMap = AssetFactory.Instance.CreateEnvironmentMap("CubeMapObject");
            cubeMap.GenCubemap(cubemap[0], cubemap[1], cubemap[2], cubemap[3], cubemap[4], cubemap[5]);
            MainScene.AddObject(cubeMap);
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

            var probe = MainScene.FindObject("CubeMapObject") as EnvironmentProbe;
            ImageBasedLighting ibl = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL) as ImageBasedLighting;
            Renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL));
            ibl.uCubeMap = probe.Cubemap;
        }
    }
}
