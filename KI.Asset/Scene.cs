using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using KI.Foundation.Core;
using KI.Foundation.Tree;

namespace KI.Asset
{
    public class Scene : KIObject
    {

        /// <summary>
        /// 空間の最大値
        /// </summary>
        public readonly Vector3 WorldMax = new Vector3(100, 125, 100);
        /// <summary>
        /// 空間の最小値
        /// </summary>
        public readonly Vector3 WorldMin = new Vector3(-100, -75, -100);

        public Scene(string name)
            : base(name)
        {
            RootNode = new KINode("ROOT");
        }

        #region [scene dictionary]
        /// <summary>
        /// ルートノード
        /// </summary>
        public KINode RootNode;

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
            KINode obj;
            obj = RootNode.FindChild(key);
            if(obj == null)
            {
                return null;
            }
            else
            {
                return obj.KIObject;
            }
        }

        public void AddObject(KIObject value, KINode parent = null)
        {
            if (parent == null)
            {
                parent = RootNode;
            }
            parent.AddChild(value);
        }
        public void AddObject(List<KIObject> value, KINode parent = null)
        {
            if (parent == null)
            {
                parent = RootNode;
            }
            foreach (var obj in value)
            {
                parent.AddChild(obj);
            }
        }

        public void DeleteObject(string key)
        {
            RootNode.RemoveRecursiveChild(key);
        }
        #endregion
        #region [dispose]
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
                if (!exist)
                {
                    return newKey;
                }
            }
        }
        
        public override void Dispose()
        {

        }
        #endregion



    }
}
