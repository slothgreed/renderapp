﻿using System;
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
        public Scene(string name)
        {
            RootNode = new EmptyNode("ROOT");
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
        public Light SunLight { get; set; }


        #region [public scene method]

        /// <summary>
        /// オブジェクトの追加
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void AddObject(string filePath)
        {
            var model = AssetFactory.Instance.CreateLoad3DModel(filePath);
            var renderObject = RenderObjectFactory.Instance.CreateRenderObject(filePath, model);
            AddObject(renderObject);
        }

        public KINode FindNode(SceneNode obj)
        {
            return RootNode.FindRecursiveChild(obj.Name);
        }

        /// <summary>
        /// シーンのオブジェクトの取得
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>オブジェクト</returns>
        public KINode FindNode(string key)
        {
            KINode obj = RootNode.FindRecursiveChild(key);
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
        /// オブジェクトの追加
        /// </summary>
        /// <param name="value">追加するオブジェクトリスト</param>
        /// <param name="parent">親</param>
        public void AddObject(KINode value, KINode parent = null)
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
            Vector3 center = (WorldMax + WorldMin) / 2;
            Vector3 position = camera.Position;

            float bdbDist = (WorldMax - center).Length;

            float distance = bdbDist / (float)Math.Sin(camera.FOV / 2.0f);

            camera.LookAt = center;
            camera.LookAtDistance = distance / camera.FOVAspect;
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
