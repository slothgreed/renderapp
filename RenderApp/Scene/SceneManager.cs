using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Render_System;
using RenderApp.GLUtil;
using RenderApp.AssetModel;
using KI.Gfx.KIAsset;
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
                    _renderSystem = new RenderSystem(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
                }
                return _renderSystem;
            }
        }

        public void Create(string name)
        {
            var scene = new Scene(name);
            scene.Initialize();
            SceneList.Add(scene);
            ActiveScene = scene;
        }
        public void CreateMainCamera()
        {
            ActiveScene.MainCamera = AssetFactory.Instance.CreateMainCamera();
            SceneManager.Instance.ActiveScene.AddObject(ActiveScene.MainCamera);
        }
        public void CreateSceneLight()
        {
            ActiveScene.SunLight = AssetFactory.Instance.CreateSunLight();
            SceneManager.Instance.ActiveScene.AddObject(ActiveScene.SunLight);
        }
        public void AddObject(string filePath)
        {
            var model = AssetFactory.Instance.CreateLoad3DModel(filePath);
            ActiveScene.AddObject(model.FirstOrDefault());
        }
    }
}
