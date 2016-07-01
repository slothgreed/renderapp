using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RenderApp.Assets;
using RenderApp.GLUtil;
using RenderApp.ViewModel;
namespace RenderApp
{
    public class Scene
    {

        /// <summary>
        /// 空間の最大値
        /// </summary>
        public readonly static Vector3 WorldMax = new Vector3(50, 75, 50);
        /// <summary>
        /// 空間の最小値
        /// </summary>
        public readonly static Vector3 WorldMin = new Vector3(-50, 0, -50);

        #region [scene dictionary]
        private static Dictionary<string, Scene> SceneList = new Dictionary<string, Scene>();
        /// <summary>
        /// アセットリスト TODO実装
        /// </summary>
        private List<Dictionary<string, Asset>> AssetList = new List<Dictionary<string, Asset>>();
        /// <summary>
        /// モデルリスト
        /// </summary>
        private Dictionary<string, Geometry> GeometryList = new Dictionary<string, Geometry>();
        /// <summary>
        /// 光源オブジェクト
        /// </summary>
        private Dictionary<string, Light> LightList = new Dictionary<string, Light>();
        /// <summary>
        /// カメラオブジェクト
        /// </summary>
        private Dictionary<string, Camera> CameraList = new Dictionary<string, Camera>();
        /// <summary>
        /// Textureオブジェクト
        /// </summary>
        private Dictionary<string, Texture> TextureList = new Dictionary<string, Texture>();
        /// <summary>
        /// シェーダプログラムオブジェクト
        /// </summary>
        private Dictionary<string, ShaderProgram> ShaderProgramList = new Dictionary<string, ShaderProgram>();
        /// <summary>
        /// シェーダオブジェクト
        /// </summary>
        private Dictionary<string, Shader> ShaderList = new Dictionary<string, Shader>();
        /// <summary>
        /// マテリアルオブジェクト
        /// </summary>
        private Dictionary<string, Material> MaterialList = new Dictionary<string, Material>();
        /// <summary>
        /// 環境プローブオブジェクト
        /// </summary>
        private Dictionary<string, EnvironmentProbe> EnvProbeList = new Dictionary<string, EnvironmentProbe>();
        /// <summary>
        /// フレームバッファオブジェクト
        /// </summary>
        private Dictionary<string, FrameBuffer> FrameBufferList = new Dictionary<string, FrameBuffer>();

        #endregion
        #region [default property]
        /// <summary>
        /// カメラ
        /// </summary>
        private Camera _mainCamera;
        public Camera MainCamera
        {
            get
            {
                return _mainCamera;
            }
            set
            {
                _mainCamera = value;
            }
        }
        private Light _sunLight;
        public Light SunLight
        {
            get
            {
                return _sunLight;
            }
            set
            {
                _sunLight = value;
            }
        }
        private static Shader _defaultShader;
        public static Shader DefaultShader
        {
            get
            {
                if (_defaultShader == null)
                {
                    string path = Project.ShaderDirectory;
                    ShaderProgram diffuseV = new ShaderProgram(path + @"\Diffuse.vert");
                    ShaderProgram diffuseF = new ShaderProgram(path + @"\Diffuse.frag");
                    Shader diffuse = new Shader(diffuseV, diffuseF);
                    _defaultShader = diffuse;
                }
                return _defaultShader;
            }

        }
        #endregion
        #region [static member]
        /// <summary>
        /// Activeシーン
        /// </summary>
        private static Scene _activeScene;
        public static Scene ActiveScene
        {
            get
            {
                if (_activeScene == null)
                {

                    if (SceneList.Count == 0)
                    {
                        SceneList.Add("MainScene", new Scene());
                        _activeScene = SceneList["MainScene"];
                    }
                }
                return _activeScene;

            }
        }

        #endregion
        #region [public scene method]
        public IEnumerable<string> GetAssetList(EAssetType assetType)
        {
            switch (assetType)
            {
                case EAssetType.Geometry:
                    foreach (var loop in GeometryList)
                    {
                        yield return loop.Key;
                    }
                    break;
                case EAssetType.Light:
                    foreach (var loop in LightList)
                    {
                        yield return loop.Key;
                    }
                    break;
                case EAssetType.Camera:
                    foreach (var loop in CameraList)
                    {
                        yield return loop.Key;
                    }
                    break;
                case EAssetType.Textures:
                    foreach (var loop in TextureList)
                    {
                        yield return loop.Key;
                    }
                    break;
                case EAssetType.ShaderProgram:
                    foreach (var loop in ShaderProgramList)
                    {
                        yield return loop.Key;
                    }
                    break;
                case EAssetType.Materials:
                    foreach (var loop in MaterialList)
                    {
                        yield return loop.Key;
                    }
                    break;
                case EAssetType.Shader:
                    foreach (var loop in ShaderList)
                    {
                        yield return loop.Key;
                    }
                    break;
                case EAssetType.EnvProbe:
                    foreach (var loop in EnvProbeList)
                    {
                        yield return loop.Key;
                    }
                    break;
                case EAssetType.FrameBuffer:
                    foreach(var loop in FrameBufferList)
                    {
                        yield return loop.Key;
                    }
                    break;
            }
        }
        /// <summary>
        /// シーンのオブジェクトの取得
        /// </summary>
        /// <param name="key"></param>
        /// <param name="assetType"></param>
        /// <returns></returns>
        public object FindObject(string key, EAssetType assetType)
        {
            switch (assetType)
            {
                case EAssetType.Geometry:
                    if (GeometryList.ContainsKey(key))
                    {
                        return GeometryList[key];
                    }
                    break;
                case EAssetType.Light:
                    if (LightList.ContainsKey(key))
                    {
                        return LightList[key];
                    }
                    break;
                case EAssetType.Camera:
                    if (CameraList.ContainsKey(key))
                    {
                        return CameraList[key];
                    }
                    break;
                case EAssetType.Textures:
                    if (TextureList.ContainsKey(key))
                    {
                        return TextureList[key];
                    }
                    break;
                case EAssetType.ShaderProgram:
                    if (ShaderProgramList.ContainsKey(key))
                    {
                        return ShaderProgramList[key];
                    }
                    break;
                case EAssetType.Materials:
                    if (MaterialList.ContainsKey(key))
                    {
                        return MaterialList[key];
                    }
                    break;
                case EAssetType.Shader:
                    if (ShaderList.ContainsKey(key))
                    {
                        return ShaderList[key];
                    }
                    break;
                case EAssetType.EnvProbe:
                    if (EnvProbeList.ContainsKey(key))
                    {
                        return EnvProbeList[key];
                    }
                    break;
                case EAssetType.FrameBuffer:
                    if(FrameBufferList.ContainsKey(key))
                    {
                        return FrameBufferList[key];
                    }
                    break;
            }

            return null;
        }
      

        public void AddSceneObject(string key, object value)
        {
            if (value is Geometry)
            {
                AddAsset<Geometry>(key, value, EAssetType.Geometry, GeometryList);
            }
            else if (value is Light)
            {
                AddAsset<Light>(key, value, EAssetType.Light, LightList);
            }
            else if (value is Camera)
            {
                AddAsset<Camera>(key, value, EAssetType.Camera, CameraList);

            }
            else if (value is Texture)
            {
                AddAsset<Texture>(key, value, EAssetType.Textures, TextureList);

            }
            else if (value is ShaderProgram)
            {
                AddAsset<ShaderProgram>(key, value, EAssetType.ShaderProgram, ShaderProgramList);
            }
            else if (value is Shader)
            {
                AddAsset<Shader>(key, value, EAssetType.Shader, ShaderList);

            }
            else if (value is Material)
            {
                AddAsset<Material>(key, value, EAssetType.Materials, MaterialList);
            }
            else if (value is EnvironmentProbe)
            {
                AddAsset<EnvironmentProbe>(key, value, EAssetType.EnvProbe, EnvProbeList);
            }else if(FrameBufferList is FrameBuffer)
            {
                AddAsset<FrameBuffer>(key, value, EAssetType.FrameBuffer, FrameBufferList);
            }
        }

        private bool AddAsset<T>(string key, object value, EAssetType assetType, Dictionary<string, T> AssetList) where T : Asset
        {
            if (!AssetList.ContainsKey(key))
            {
                AssetList.Add(key, (T)value);
                MainWindowViewModel.Instance.AssetWindow.AddAssetTree(new TreeItemViewModel((T)value, assetType));
            }
            else
            {
                key = GetNewKey<T>(key, AssetList);
                AssetList.Add(key, (T)value);
                ((T)value).Key = key;
                MainWindowViewModel.Instance.AssetWindow.AddAssetTree(new TreeItemViewModel((T)value, assetType));
            }
            return true;
        }
        private string GetNewKey<T>(string key, Dictionary<string, T> AssetList) where T : Asset
        {
            string newKey = key;
            int serialNumber = 0;
            bool exist = true;
            while (true)
            {
                serialNumber++;
                newKey = key + serialNumber;
                exist = AssetList.ContainsKey(newKey);
                if(!exist)
                {
                    return newKey;
                }
            }
        }
        internal void DeleteAsset(string key, EAssetType assetType)
        {
            switch (assetType)
            {
                case EAssetType.Geometry:
                    if (GeometryList.ContainsKey(key))
                    {
                        GeometryList[key].Dispose();
                        GeometryList.Remove(key);
                    }
                    break;
                case EAssetType.Light:
                    if (LightList.ContainsKey(key))
                    {
                        LightList[key].Dispose();
                        LightList.Remove(key);
                    }
                    break;
                case EAssetType.Camera:
                    if (CameraList.ContainsKey(key))
                    {
                        CameraList[key].Dispose();
                        CameraList.Remove(key);
                    }
                    break;
                case EAssetType.Textures:
                    if (TextureList.ContainsKey(key))
                    {
                        TextureList[key].Dispose();
                        TextureList.Remove(key);
                    }
                    break;
                case EAssetType.ShaderProgram:
                    if (ShaderProgramList.ContainsKey(key))
                    {
                        ShaderProgramList[key].Dispose();
                        ShaderProgramList.Remove(key);
                    }
                    break;
                case EAssetType.Materials:
                    if (MaterialList.ContainsKey(key))
                    {
                        MaterialList[key].Dispose();
                        MaterialList.Remove(key);
                    }
                    break;
                case EAssetType.Shader:
                    if (ShaderList.ContainsKey(key))
                    {
                        ShaderList[key].Dispose();
                        ShaderList.Remove(key);
                    }
                    break;
                case EAssetType.EnvProbe:
                    if (EnvProbeList.ContainsKey(key))
                    {
                        EnvProbeList[key].Dispose();
                        EnvProbeList.Remove(key);
                    }
                    break;
                case EAssetType.FrameBuffer:
                    if (FrameBufferList.ContainsKey(key))
                    {
                        FrameBufferList[key].Dispose();
                        FrameBufferList.Remove(key);
                    }
                    break;
            }
        }
        #endregion
        #region [initialize]

        public void Initialize()
        {
            Camera camera = new Camera();
            _mainCamera = CameraList["MainCamera"];
            Light light = new PointLight(new Vector3(10), Vector3.Zero);
            SunLight = light;

            Sphere sphere = new Sphere(WorldMax.X * 2, 20, 20, false, Vector3.UnitX);
            sphere.MaterialItem = new Material(new Shader(new ShaderProgram(Global.SphereMapVertexShader), new ShaderProgram(Global.SphereMapFragmentShader)));
            sphere.MaterialItem.AddTexture(TextureKind.Albedo, new Texture(Global.SphereMapAlbedo));
        }
        #endregion
        #region [render]
        /// <summary>
        /// レンダリング
        /// </summary>
        public void Render()
        {
            foreach (Geometry loop in GeometryList.Values)
            {
                loop.Render();
            }
        }
        #endregion
        #region [dispose]
        public void Dispose()
        {
            foreach (var loop in GeometryList.Values)
            {
                loop.Dispose();
            }
            foreach (var loop in LightList.Values)
            {
                loop.Dispose();
            }
            foreach (var loop in CameraList.Values)
            {
                loop.Dispose();
            }
            foreach (var loop in TextureList.Values)
            {
                loop.Dispose();
            }
            foreach (var loop in ShaderProgramList.Values)
            {
                loop.Dispose();
            }
            foreach (var loop in ShaderList.Values)
            {
                loop.Dispose();
            }
            foreach (var loop in MaterialList.Values)
            {
                loop.Dispose();
            }
            foreach (var loop in EnvProbeList.Values)
            {
                loop.Dispose();
            }
            foreach(var loop in FrameBufferList.Values)
            {
                loop.Dispose();
            }
        }
        public static void AllDispose()
        {
            foreach (var scene in SceneList.Values)
            {
                scene.Dispose();

            }
        }
        #endregion


    }
}
