using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Foundation.Tree;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// シーンクラス
    /// </summary>
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

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">シーン名</param>
        public Scene(string name)
            : base(name)
        {
            RootNode = new KINode("ROOT");
        }

        /// <summary>
        /// ルートノード
        /// </summary>
        public KINode RootNode { get; set; }

        /// <summary>
        /// 選択中のアセット
        /// </summary>
        public KIObject SelectAsset { get; set; }

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
        /// シーンのオブジェクトの取得
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>オブジェクト</returns>
        public KIObject FindObject(string key)
        {
            KINode obj;
            obj = RootNode.FindChild(key);
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
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
        }

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
