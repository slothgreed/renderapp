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
using RenderApp.Utility;
using KI.Foundation.Core;
using KI.Gfx.KIAsset;
namespace RenderApp
{
    public class Scene : KIObject
    {

        /// <summary>
        /// 空間の最大値
        /// </summary>
        public readonly Vector3 WorldMax = new Vector3(50, 75, 50);
        /// <summary>
        /// 空間の最小値
        /// </summary>
        public readonly Vector3 WorldMin = new Vector3(-50, 0, -50);

        public Scene(string name)
            : base(name)
        {

        }
        #region [scene dictionary]
        /// <summary>
        /// ルートノード
        /// </summary>
        public RANode RootNode;
        /// <summary>
        /// 選択中のアセット
        /// </summary>
        private KIObject _selectAsset;
        public KIObject SelectAsset
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
        #region [public scene method]
        /// <summary>
        /// シーンのオブジェクトの取得
        /// </summary>
        /// <param name="key"></param>
        /// <param name="assetType"></param>
        /// <returns></returns>
        public KIObject FindObject(string key)
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

        public void AddObject(KIObject value, RANode parent = null)
        {
            if (parent == null)
            {
                RootNode.AddChild(value);
            }
            else
            {
                parent.AddChild(value);
            }
        }

        private string GetNewKey<T>(string key, Dictionary<string, T> AssetList) where T : KIFile
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

            //List<RenderObject> sponzas = AssetFactory.Instance.CreateLoad3DModel(@"C:/Users/ido/Documents/GitHub/renderapp/RenderApp/Resource/Model/crytek-sponza/sponza.obj");
            //foreach (var sponza in sponzas)
            //{
            //    AddRootSceneObject(sponza);
            //}
            //List<RenderObject> ducks = AssetFactory.Instance.CreateLoad3DModel("C:/Users/ido/Documents/GitHub/renderapp/RenderApp/Resource/Model/duck/duck.obj");
            //foreach(var duck in ducks)
            //{
            //    duck.RotateX(-90);
            //    duck.RotateY(0);
            //    AddRootSceneObject(duck);
            //}

        }
        #endregion
        #region [dispose]
        public override void Dispose()
        {
            RootNode.Dispose();
        }
        #endregion
        #region [process]

        #endregion

    }
}
