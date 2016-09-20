using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.ViewModel;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.AssetModel.LightModel;
using RenderApp.Utility;
namespace RenderApp
{
    public class Scene
    {

        /// <summary>
        /// 空間の最大値
        /// </summary>
        public readonly Vector3 WorldMax = new Vector3(50, 75, 50);
        /// <summary>
        /// 空間の最小値
        /// </summary>
        public readonly Vector3 WorldMin = new Vector3(-50, 0, -50);

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
        /// マテリアルオブジェクト
        /// </summary>
        private Dictionary<string, Material> MaterialList = new Dictionary<string, Material>();
        /// <summary>
        /// 環境プローブオブジェクト
        /// </summary>
        private Dictionary<string, EnvironmentProbe> EnvProbeList = new Dictionary<string, EnvironmentProbe>();
        /// <summary>
        /// 選択中のアセット
        /// </summary>
        public Asset SelectAsset
        {
            get;
            set;
        }

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
                return _activeScene;
            }
        }
        public static Scene Create(string name)
        {
            SceneList.Add(name, new Scene());
            _activeScene = SceneList[name];
            return _activeScene;
        }
        #endregion
        #region [public scene method]
        public IEnumerable<string> GetAssetListStr(EAssetType assetType)
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
                case EAssetType.Materials:
                    foreach (var loop in MaterialList)
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
            }
        }

        public IEnumerable<Asset> GetAssetList(EAssetType assetType)
        {
            switch (assetType)
            {
                case EAssetType.Geometry:
                    foreach (var loop in GeometryList)
                    {
                        yield return loop.Value;
                    }
                    break;
                case EAssetType.Light:
                    foreach (var loop in LightList)
                    {
                        yield return loop.Value;
                    }
                    break;
                case EAssetType.Camera:
                    foreach (var loop in CameraList)
                    {
                        yield return loop.Value;
                    }
                    break;
                case EAssetType.Textures:
                    foreach (var loop in TextureList)
                    {
                        yield return loop.Value;
                    }
                    break;
                case EAssetType.Materials:
                    foreach (var loop in MaterialList)
                    {
                        yield return loop.Value;
                    }
                    break;
                case EAssetType.EnvProbe:
                    foreach (var loop in EnvProbeList)
                    {
                        yield return loop.Value;
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
                case EAssetType.Materials:
                    if (MaterialList.ContainsKey(key))
                    {
                        return MaterialList[key];
                    }
                    break;
                case EAssetType.EnvProbe:
                    if (EnvProbeList.ContainsKey(key))
                    {
                        return EnvProbeList[key];
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
            else if (value is Material)
            {
                AddAsset<Material>(key, value, EAssetType.Materials, MaterialList);
            }
            else if (value is EnvironmentProbe)
            {
                AddAsset<EnvironmentProbe>(key, value, EAssetType.EnvProbe, EnvProbeList);
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
                        GeometryList.Remove(key);
                        AssetFactory.Instance.RemoveItem(key);
                    }
                    break;
                case EAssetType.Light:
                    if (LightList.ContainsKey(key))
                    {
                        LightList.Remove(key);
                        AssetFactory.Instance.RemoveItem(key);
                    }
                    break;
                case EAssetType.Camera:
                    if (CameraList.ContainsKey(key))
                    {
                        CameraList.Remove(key);
                        AssetFactory.Instance.RemoveItem(key);
                    }
                    break;
                case EAssetType.Textures:
                    if (TextureList.ContainsKey(key))
                    {
                        TextureList.Remove(key);
                        TextureFactory.Instance.RemoveItem(key);
                    }
                    break;
                case EAssetType.Materials:
                    if (MaterialList.ContainsKey(key))
                    {
                        MaterialList.Remove(key);
                    }
                    break;
                case EAssetType.EnvProbe:
                    if (EnvProbeList.ContainsKey(key))
                    {
                        EnvProbeList.Remove(key);
                    }
                    break;
            }
        }
        #endregion
        #region [initialize]
        /// <summary>
        /// シーンの初期化
        /// </summary>
        public void Initialize()
        {
            MainCamera = AssetFactory.Instance.CreateMainCamera();
            SunLight = AssetFactory.Instance.CreateSunLight();
            
            Geometry map = AssetFactory.Instance.CreateEnvironmentMap();
            AddSceneObject(map.Key, map);
        }
        #endregion
        #region [dispose]
        public void Dispose()
        {
            //foreach (var loop in ShaderProgramList.Values)
            //{
            //    loop.Dispose();
            //}
            //foreach (var loop in MaterialList.Values)
            //{
            //    loop.Dispose();
            //}
            //foreach (var loop in EnvProbeList.Values)
            //{
            //    loop.Dispose();
            //}
        }
        public static void AllDispose()
        {
            foreach (var scene in SceneList.Values)
            {
                scene.Dispose();
            }
        }
        #endregion

        #region [process]
        /// <summary>
        /// ポリゴンごとに行うので、CPUベースで
        /// </summary>
        /// <param name="mouse"></param>
        public void Picking(Vector2 mouse)
        {
            float minLength = float.MaxValue;
            int[] a = new int[3];
            List<Vector3> minTriangle = null;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            int[] viewport = new int[4];
            viewport[0] = 0;
            viewport[1] = 0;
            viewport[2] = Viewport.Instance.Width;
            viewport[3] = Viewport.Instance.Height;
            CCalc.GetClipPos(MainCamera.Matrix, MainCamera.ProjMatrix, viewport, mouse, out near, out far);
            foreach (Geometry geometry in ActiveScene.GeometryList.Values)
            {
                //頂点配列の時
                if (geometry.Index.Count != 0)
                {

                }
                else
                {
                    for (int i = 0; i < geometry.Position.Count / 3; i++)
                    {
                        Vector3 vertex1 = CCalc.Multiply(geometry.ModelMatrix, geometry.Position[3 * i]);
                        Vector3 vertex2 = CCalc.Multiply(geometry.ModelMatrix, geometry.Position[3 * i + 1]);
                        Vector3 vertex3 = CCalc.Multiply(geometry.ModelMatrix, geometry.Position[3 * i + 2]);
                        Vector3 result = Vector3.Zero;
                        if (CCalc.CrossPlanetoLinePos(vertex1, vertex2, vertex3, near, far, ref minLength, out result))
                        {


                            if (minTriangle == null)
                            {
                                minTriangle = new List<Vector3>();
                                minTriangle.Add(vertex1);
                                minTriangle.Add(vertex2);
                                minTriangle.Add(vertex3);
                            }
                            else
                            {
                                minTriangle[0] = vertex1;
                                minTriangle[1] = vertex2;
                                minTriangle[2] = vertex3;
                            }
                        }
                    }
                }
            }
            if (minTriangle != null)
            {
                Vector3 normal = CCalc.Normal(minTriangle[0], minTriangle[1], minTriangle[2]);
                minTriangle[0] += normal * 0.01f;
                minTriangle[1] += normal * 0.01f;
                minTriangle[2] += normal * 0.01f;
                var picking = ActiveScene.FindObject("Picking", EAssetType.Geometry) as Primitive;
                if (picking == null)
                {
                    Primitive triangle = new Primitive("Picking", minTriangle, CCalc.RandomColor(), OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);
                    triangle.MaterialItem = new Material("Picking:Material");
                    triangle.MaterialItem.SetShader(ShaderFactory.Instance.DefaultAnalyzeShader);
                    AssetFactory.Instance.CreateGeometry(triangle);

                }
                else
                {
                    picking.AddVertex(minTriangle, CCalc.RandomColor());
                }
            }
        }

       
        #endregion


        
    }
}
