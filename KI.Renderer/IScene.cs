using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Core;
using KI.Foundation.Tree;
using OpenTK;

namespace KI.Renderer
{
    /// <summary>
    /// シーンクラス
    /// </summary>
    public abstract class IScene
    {
        /// <summary>
        /// 空間の最大値
        /// </summary>
        public readonly Vector3 WorldMax = new Vector3(100, 125, 100);

        /// <summary>
        /// 空間の最小値
        /// </summary>
        public readonly Vector3 WorldMin = new Vector3(-100, -75, -100);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">シーン名</param>
        public IScene(string name)
        {
            RootNode = new KINode("ROOT");
            MainCamera = AssetFactory.Instance.CreateCamera("MainCamera");
            SunLight = RenderObjectFactory.Instance.CreateDirectionLight("SunLight", WorldMax, Vector3.Zero);
            AddObject(MainCamera);
            AddObject(SunLight);
        }

        /// <summary>
        /// ルートノード
        /// </summary>
        public KINode RootNode { get; set; }

        /// <summary>
        /// 選択中のアセット
        /// </summary>
        public SceneNode SelectNode { get; set; }

        /// <summary>
        /// メインカメラ
        /// </summary>
        public Camera MainCamera { get; set; }

        /// <summary>
        /// 主な光源
        /// </summary>
        public Light SunLight { get; set; }

        #region [public scene method]

        /// <summary>
        /// 初期化
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// オブジェクトの追加
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void AddObject(string filePath)
        {
            var model = AssetFactory.Instance.CreateLoad3DModel(filePath);
            var renderObject = RenderObjectFactory.Instance.CreateRenderObjects(filePath, model);
            foreach (var obj in renderObject)
            {
                AddObject(obj);
            }
        }

        /// <summary>
        /// シーンのオブジェクトの取得
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>オブジェクト</returns>
        public KIObject FindObject(string key)
        {
            KINode obj = RootNode.FindRecursiveChild(key);
            if (obj == null)
            {
                return null;
            }
            else
            {
                return obj.KIObject;
            }
        }

        /// <summary>
        /// オブジェクトの追加
        /// </summary>
        /// <param name="value">追加するオブジェクト</param>
        /// <param name="parent">親</param>
        public void AddObject(KIObject value, KINode parent = null)
        {
            AddObject(new List<KIObject>() { value }, parent);
        }

        /// <summary>
        /// オブジェクトの追加
        /// </summary>
        /// <param name="value">追加するオブジェクトリスト</param>
        /// <param name="parent">親</param>
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

        /// <summary>
        /// オブジェクトの削除
        /// </summary>
        /// <param name="key">キー</param>
        public void DeleteObject(string key)
        {
            RootNode.RemoveRecursiveChild(key);
        }

        #endregion

        /// <summary>
        /// 新しいキーの作成
        /// </summary>
        /// <typeparam name="T">KIObject</typeparam>
        /// <param name="key">初期キー</param>
        /// <param name="assetList">既存のアセット</param>
        /// <returns>キー</returns>
        private string GetNewKey<T>(string key, Dictionary<string, T> assetList) where T : KIObject
        {
            string newKey = key;
            int serialNumber = 0;
            bool exist = true;
            while (true)
            {
                serialNumber++;
                newKey = key + serialNumber;
                exist = assetList.ContainsKey(newKey);
                if (!exist)
                {
                    return newKey;
                }
            }
        }
    }
}
