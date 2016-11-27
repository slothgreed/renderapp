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
        /// ルートノード
        /// </summary>
        public RANode RootNode;
        ///// <summary>
        ///// アセットリスト TODO実装
        ///// </summary>
        //private List<Dictionary<string, Asset>> AssetList = new List<Dictionary<string, Asset>>();
        ///// <summary>
        ///// モデルリスト
        ///// </summary>
        //private Dictionary<string, Geometry> GeometryList = new Dictionary<string, Geometry>();
        ///// <summary>
        ///// 光源オブジェクト
        ///// </summary>
        //private Dictionary<string, Light> LightList = new Dictionary<string, Light>();
        ///// <summary>
        ///// カメラオブジェクト
        ///// </summary>
        //private Dictionary<string, Camera> CameraList = new Dictionary<string, Camera>();
        ///// <summary>
        ///// 環境プローブオブジェクト
        ///// </summary>
        //private Dictionary<string, EnvironmentProbe> EnvProbeList = new Dictionary<string, EnvironmentProbe>();
        /// <summary>
        /// 選択中のアセット
        /// </summary>
        private RAObject _selectAsset;
        public RAObject SelectAsset
        {
            get
            {
                return _selectAsset;
            }
            set
            {
                _selectAsset = value;
            }
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
        //public IEnumerable<string> GetAssetListStr(EAssetType assetType)
        //{
        //    switch (assetType)
        //    {
        //        case EAssetType.Geometry:
        //            foreach (var loop in GeometryList)
        //            {
        //                yield return loop.Key;
        //            }
        //            break;
        //        case EAssetType.Light:
        //            foreach (var loop in LightList)
        //            {
        //                yield return loop.Key;
        //            }
        //            break;
        //        case EAssetType.Camera:
        //            foreach (var loop in CameraList)
        //            {
        //                yield return loop.Key;
        //            }
        //            break;
        //        case EAssetType.EnvProbe:
        //            foreach (var loop in EnvProbeList)
        //            {
        //                yield return loop.Key;
        //            }
        //            break;
        //    }
        //}

        //public IEnumerable<Asset> GetAssetList(EAssetType assetType)
        //{
        //    switch (assetType)
        //    {
        //        case EAssetType.Geometry:
        //            foreach (var loop in GeometryList)
        //            {
        //                yield return loop.Value;
        //            }
        //            break;
        //        case EAssetType.Light:
        //            foreach (var loop in LightList)
        //            {
        //                yield return loop.Value;
        //            }
        //            break;
        //        case EAssetType.Camera:
        //            foreach (var loop in CameraList)
        //            {
        //                yield return loop.Value;
        //            }
        //            break;
        //        case EAssetType.EnvProbe:
        //            foreach (var loop in EnvProbeList)
        //            {
        //                yield return loop.Value;
        //            }
        //            break;
        //    }
        //}

        /// <summary>
        /// シーンのオブジェクトの取得
        /// </summary>
        /// <param name="key"></param>
        /// <param name="assetType"></param>
        /// <returns></returns>
        public RAObject FindObject(string key)
        {
            RANode obj;
            obj = RootNode.FindChild(key);
            if(obj == null)
            {
                return null;
            }
            else
            {
                return obj.RAObject;
            }
        }
      
        public void AddRootSceneObject(RAObject value)
        {
            AddSceneObject(RootNode,value);
        }

        public void AddSceneObject(RANode parent, RAObject value)
        {
            parent.AddChild(value);
        }

        private string GetNewKey<T>(string key, Dictionary<string, T> AssetList) where T : RAFile
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
        internal void DeleteNode(string key)
        {
            RootNode.RemoveRecursiveChild(key);
        }
        #endregion
        #region [initialize]
        /// <summary>
        /// シーンの初期化
        /// </summary>
        public void Initialize()
        {
            RootNode = new RANode("ROOT");
            MainCamera = AssetFactory.Instance.CreateMainCamera();
            Scene.ActiveScene.AddRootSceneObject(MainCamera);

            SunLight = AssetFactory.Instance.CreateSunLight();
            Scene.ActiveScene.AddRootSceneObject(SunLight);

            Geometry map = AssetFactory.Instance.CreateEnvironmentMap();
            AddRootSceneObject(map);
            List<Geometry> sponzas = AssetFactory.Instance.CreateLoad3DModel(@"C:/Users/ido/Documents/GitHub/renderapp/RenderApp/Resource/Model/crytek-sponza/sponza.obj");
            foreach (var sponza in sponzas)
            {
                AddRootSceneObject(sponza);
            }
            List<Geometry> ducks = AssetFactory.Instance.CreateLoad3DModel("C:/Users/ido/Documents/GitHub/renderapp/RenderApp/Resource/Model/duck/duck.obj");
            foreach(var duck in ducks)
            {
                duck.RotateX(-90);
                duck.RotateY(0);
                AddRootSceneObject(duck);
            }
        }
        #endregion
        #region [dispose]
        public void Dispose()
        {
            RootNode.Dispose();
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
        /// Picking
        /// </summary>
        /// <param name="mouse">マウス座標</param>
        /// <param name="geometry">形状データ</param>
        /// <param name="minLength">この数値以下のポリゴンを取得</param>
        /// <param name="selectIndex">選択したデータの頂点番号</param>
        /// <returns></returns>
        public bool Picking(Vector3 near, Vector3 far, Geometry geometry, ref float minLength, ref int selectIndex)
        {
            bool select = false;
            //頂点配列の時
            if (geometry.Index.Count != 0)
            {

            }
            else
            {
                for (int i = 0; i < geometry.Position.Count / 3; i++)
                {
                    Vector3 vertex1 = RACalc.Multiply(geometry.ModelMatrix, geometry.Position[3 * i]);
                    Vector3 vertex2 = RACalc.Multiply(geometry.ModelMatrix, geometry.Position[3 * i + 1]);
                    Vector3 vertex3 = RACalc.Multiply(geometry.ModelMatrix, geometry.Position[3 * i + 2]);
                    Vector3 result = Vector3.Zero;
                    if (RACalc.CrossPlanetoLinePos(vertex1, vertex2, vertex3, near, far, ref minLength, out result))
                    {
                        selectIndex = 3 * i;
                        select = true;
                    }
                }
            }
            return select;
        }
        /// <summary>
        /// ポリゴンごとに行うので、CPUベースで頂点番号を取得
        /// </summary>
        /// <param name="mouse"></param>
        public bool Picking(Vector2 mouse,ref Geometry selectGeometry,ref int selectIndex)
        {
            selectIndex = -1;
            float minLength = float.MaxValue;
            int[] a = new int[3];
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            int[] viewport = new int[4];
            viewport[0] = 0;
            viewport[1] = 0;
            viewport[2] = Viewport.Instance.Width;
            viewport[3] = Viewport.Instance.Height;
            RACalc.GetClipPos(MainCamera.Matrix, MainCamera.ProjMatrix, viewport, mouse, out near, out far);
            Geometry geometry;
            foreach (RANode geometryNode in RootNode.AllChildren())
            {
                geometry = null;
                if (geometryNode.RAObject is Geometry)
                {
                    geometry = geometryNode.RAObject as Geometry;
                }
                else
                {
                    continue;
                }
                if(Picking(near, far, geometry,ref minLength,ref selectIndex))
                {
                    selectGeometry = geometry;
                }
            }
            if(selectIndex == -1)
            {
                return false;   
            }
            return true;

        }

       
        #endregion


        
    }
}
