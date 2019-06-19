using System;
using System.Collections.Generic;
using System.Linq;
using KI.Asset;
using KI.Foundation.Core;
using OpenTK;

namespace KI.Renderer
{
    /// <summary>
    /// シーンクラス
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// 空間の最大値
        /// </summary>
        public readonly Vector3 WorldMax = new Vector3(1, 1, 1);

        /// <summary>
        /// 空間の最小値
        /// </summary>
        public readonly Vector3 WorldMin = new Vector3(-1, -1, -1);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">シーン名</param>
        public Scene(string name, SceneNode rootNode)
        {
            RootNode = rootNode;
        }

        /// <summary>
        /// ルートノード
        /// </summary>
        public SceneNode RootNode { get; set; }

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
        public LightNode MainLight { get; set; }


        #region [public scene method]

        /// <summary>
        /// オブジェクトの追加
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void AddObject(string filePath)
        {
            var model = AssetFactory.Instance.CreateLoad3DModel(filePath);
            var polygonNode = SceneNodeFactory.Instance.CreatePolygonNode(filePath, model, null);
            AddObject(polygonNode);
        }

        public SceneNode FindNode(SceneNode obj)
        {
            return RootNode.FindRecursiveChild(obj.Name) as SceneNode;
        }

        /// <summary>
        /// シーンのオブジェクトの取得
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>オブジェクト</returns>
        public SceneNode FindNode(string key)
        {
            SceneNode obj = RootNode.FindRecursiveChild(key) as SceneNode;
            if (obj == null)
            {
                return null;
            }
            else
            {
                return obj;
            }
        }

        /// <summary>
        /// オブジェクトの追加
        /// </summary>
        /// <param name="value">追加するオブジェクト</param>
        /// <param name="parent">親</param>
        public void AddObject(KIObject value, SceneNode parent = null)
        {
            AddObject(new List<KIObject>() { value }, parent);
        }

        /// <summary>
        /// オブジェクトの追加
        /// </summary>
        /// <param name="value">追加するオブジェクトリスト</param>
        /// <param name="parent">親</param>
        public void AddObject(List<KIObject> value, SceneNode parent = null)
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
        /// オブジェクトの追加
        /// </summary>
        /// <param name="value">追加するオブジェクトリスト</param>
        /// <param name="parent">親</param>
        public void AddObject(SceneNode value, SceneNode parent = null)
        {
            if (parent == null)
            {
                parent = RootNode;
            }

            parent.AddChild(value);
        }

        /// <summary>
        /// オブジェクトの削除
        /// </summary>
        /// <param name="key">キー</param>
        public void DeleteObject(string key)
        {
            RootNode.RemoveRecursiveChild(key);
        }

        /// <summary>
        /// シーンにカメラをフィットさせる
        /// </summary>
        public void FitToScene(Camera camera)
        {
            FitToBox(camera, WorldMin, WorldMax);
        }

        /// <summary>
        /// 指定矩形にカメラをフィットさせる
        /// </summary>
        /// <param name="camera">カメラ</param>
        /// <param name="min">矩形最小値</param>
        /// <param name="max">矩形最大値</param>
        public void FitToBox(Camera camera, Vector3 min, Vector3 max)
        {
            Vector3 center = (max + min) / 2;
            Vector3 position = camera.Position;

            float bdbDist = (max - center).Length;

            float distance = bdbDist / (float)Math.Sin(camera.FOV / 2.0f);

            camera.LookAt = center;
            camera.LookAtDistance = distance * 1.2f / camera.FOVAspect;
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
