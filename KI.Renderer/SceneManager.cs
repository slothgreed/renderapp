using System.Collections.Generic;
using KI.Asset;
using KI.Renderer;

namespace RenderApp
{
    /// <summary>
    /// シーンマネージャ
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// シーンリスト
        /// </summary>
        public List<Scene> Scenes = new List<Scene>();

        /// <summary>
        /// アクティブなシーン
        /// </summary>
        public Scene ActiveScene { get; set; }

        /// <summary>
        /// シーンの作成
        /// </summary>
        /// <param name="name">名前</param>
        public Scene Create(string name)
        {
            var scene = new Scene(name);
            Scenes.Add(scene);
            Global.Scene = scene;
            ActiveScene = scene;
            Initialize();
            return scene;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //List<RenderObject> axis = AssetFactory.Instance.CreateAxis("axis", Vector3.Zero, ActiveScene.WorldMax);
            //foreach(var a in axis)
            //{
            //    ActiveScene.AddObject(a);
            //}
            //List<RenderObject> sponzas = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/crytek-sponza/sponza.obj");
            //foreach (var sponza in sponzas)
            //{
            //    ActiveScene.AddObject(sponza);
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

            //List<RenderObject> bunny = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/bunny.half");
            //List<RenderObject> bunny = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/duck.half");
            //foreach (var b in bunny)
            //{
            //    b.RotateX(-90);
            //    ActiveScene.AddObject(b);
            //}

            var bunny = AssetFactory.Instance.CreateLoad3DModel(Global.KIDirectory + @"\renderapp\resource\model\bunny.half");
            //List<RenderObject> bunny = AssetFactory.Instance.CreateLoad3DModel(ProjectInfo.ModelDirectory + @"/Sphere.stl");

            var renderBunny = RenderObjectFactory.Instance.CreateRenderObject("bunny", bunny);
            ActiveScene.AddObject(renderBunny);
        }

        public void AddObject(string filePath)
        {
            var model = AssetFactory.Instance.CreateLoad3DModel(filePath);
            var renderObject = RenderObjectFactory.Instance.CreateRenderObjects(filePath, model);
            foreach (var obj in renderObject)
            {
                ActiveScene.AddObject(obj);
            }
        }
    }
}
