using KI.Asset;
using KI.Foundation.Command;
using KI.Renderer;
using KI.Tool.Command;
using OpenTK;

namespace RenderApp.Globals
{
    /// <summary>
    /// メインのシーン
    /// </summary>
    public class MainScene : IScene
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainScene()
            : base("MainScene")
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            var axis = AssetFactory.Instance.CreateAxis("axis", Vector3.Zero, WorldMax);
            var axisObject = RenderObjectFactory.Instance.CreateRenderObject(axis.ToString(), axis);
            AddObject(axisObject);

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

            var bunny = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/bunny.half");
            //var bunny = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/maxplanck.half");
            //var bunny = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/fandisk.half");
            var renderBunny = RenderObjectFactory.Instance.CreateRenderObject("bunny", bunny);
            //renderBunny.RotateX(-90);
            AddObject(renderBunny);

            //var bunny = AssetFactory.Instance.CreateLoad3DModel(Global.KIDirectory + @"\renderapp\resource\model\armadillo.half");
            ////var bunny = AssetFactory.Instance.CreateLoad3DModel(Global.KIDirectory + @"\renderapp\resource\model\bunny.half");
            ////List<RenderObject> bunny = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/Sphere.stl");
            //var renderBunny = RenderObjectFactory.Instance.CreateRenderObject("bunny", bunny);
            ////renderBunny.RotateX(-90);
            ////renderBunny.Scale = new OpenTK.Vector3(100);
            //AddObject(renderBunny);

            CommandManager.Instance.Execute(new CreateWireFrameCommand(renderBunny), null, false);
            CommandManager.Instance.Execute(new CalculateVertexCurvatureCommand(renderBunny), null, false);
        }
    }
}
