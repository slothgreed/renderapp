using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.KIRenderSystem;
using RenderApp.AssetModel;
using KI.Gfx.KIAsset;
using KI.Gfx.GLUtil;
namespace RenderApp
{
    public class SceneManager
    {
        private static SceneManager _Instance = new SceneManager();
        public static SceneManager Instance
        {
            get
            {
                return _Instance;
            }
        }
        public List<Scene> SceneList = new List<Scene>();

        public Scene ActiveScene
        {
            get;
            set;
        }

        private RenderSystem _renderSystem;
        public RenderSystem RenderSystem
        {
            get
            {
                if (_renderSystem == null)
                {
                    _renderSystem = new RenderSystem();
                }
                return _renderSystem;
            }
        }

        public void Create(string name)
        {
            var scene = new Scene(name);
            SceneList.Add(scene);
            ActiveScene = scene;
            Initialize();
            CreateMainCamera();
            CreateSceneLight();
        }
        public void Initialize()
        {
            //List<RenderObject> sponzas = AssetFactory.Instance.CreateLoad3DModel(@"C:/Users/ido/Documents/KIProject/renderapp/RenderApp/Resource/Model/crytek-sponza/sponza.obj");
            //foreach (var sponza in sponzas)
            //{
            //    ActiveScene.AddObject(sponza);
            //}
            //List<RenderObject> ducks = AssetFactory.Instance.CreateLoad3DModel("C:/Users/ido/Documents/KIProject/renderapp/RenderApp/Resource/Model/duck/duck.obj");
            //foreach (var duck in ducks)
            //{
            //    duck.RotateX(-90);
            //    duck.RotateY(0);
            //    ActiveScene.AddObject(duck);
            //}

            List<RenderObject> worlds = AssetFactory.Instance.CreateWorld("World",ActiveScene.WorldMin, ActiveScene.WorldMax);
            string[] paths = new string[]{
                    ProjectInfo.TextureDirectory + @"\cubemap\posx.jpg",
                    ProjectInfo.TextureDirectory + @"\cubemap\posy.jpg",
                    ProjectInfo.TextureDirectory + @"\cubemap\posz.jpg",
                    ProjectInfo.TextureDirectory + @"\cubemap\negx.jpg",
                    ProjectInfo.TextureDirectory + @"\cubemap\negy.jpg",
                    ProjectInfo.TextureDirectory + @"\cubemap\negz.jpg"
            };

            for (int i = 0; i < 6; i++)
            {
                var texture = TextureFactory.Instance.CreateTexture(paths[i]);
                worlds[i].AddTexture(TextureKind.Albedo, texture);
                ActiveScene.AddObject(worlds[i]);
            }
            EnvironmentProbe cubeMap = AssetFactory.Instance.CreateEnvironmentMap("world");
            cubeMap.GenCubemap(paths);
            ActiveScene.AddObject(cubeMap);

            List<RenderObject> bunny = AssetFactory.Instance.CreateLoad3DModel(@"C:/Users/ido/Documents/KIProject/renderapp/RenderApp/Resource/Model/bunny.half");
            //List<RenderObject> bunny = AssetFactory.Instance.CreateLoad3DModel(@"C:/Users/ido/Documents/KIProject/renderapp/RenderApp/Resource/Model/Sphere.stl");
            foreach (var b in bunny)
            {
                b.RotateX(-90);
                ActiveScene.AddObject(b);
            }
        }
        public void CreateMainCamera()
        {
            ActiveScene.MainCamera = AssetFactory.Instance.CreateCamera("MainCamera");
            ActiveScene.AddObject(ActiveScene.MainCamera);
        }
        public void CreateSceneLight()
        {
            ActiveScene.SunLight = AssetFactory.Instance.CreateLight("SunLight");
            ActiveScene.AddObject(ActiveScene.SunLight);
        }
        public void AddObject(string filePath)
        {
            var model = AssetFactory.Instance.CreateLoad3DModel(filePath);
            ActiveScene.AddObject(model.FirstOrDefault());
        }

    }
}
