using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Foundation.Tree;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.KITexture;

namespace RenderApp.Globals
{
    /// <summary>
    /// プロジェクト
    /// </summary>
    public class Project
    {
        /// <summary>
        /// 形状ルート
        /// </summary>
        private KINode polygonRoot;

        /// <summary>
        /// Textureオブジェクト
        /// </summary>
        private KINode textureRoot;

        /// <summary>
        /// シェーダオブジェクト
        /// </summary>
        private KINode shaderProgramRoot;

        /// <summary>
        /// 現在のプロジェクト
        /// </summary>
        public static Project ActiveProject { get; set; } = new Project();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Project()
        {
            RootNode = new KINode("ROOT");
            polygonRoot = new KINode("Geometry");
            textureRoot = new KINode("Texture");
            shaderProgramRoot = new KINode("ShaderProgram");
            RootNode.AddChild(polygonRoot);
            RootNode.AddChild(textureRoot);
            RootNode.AddChild(shaderProgramRoot);
        }

        /// <summary>
        /// ルートノード
        /// </summary>
        public KINode RootNode { get; private set; }

        /// <summary>
        /// 子供の追加
        /// </summary>
        /// <param name="child">子供</param>
        internal void AddChild(KIObject child)
        {
            if (child is Polygon)
            {
                polygonRoot.AddChild(child);
                Workspace.Instance.MainScene.AddObject(child);
            }

            if (child is Texture)
            {
                textureRoot.AddChild(child);
            }

            if (child is ShaderProgram)
            {
                shaderProgramRoot.AddChild(child);
            }
        }

        /// <summary>
        /// アセットの取得
        /// </summary>
        /// <param name="assetType">アセット種類</param>
        /// <returns>アセットリスト</returns>
        internal IEnumerable<KIObject> GetObject(RAAsset assetType)
        {
            switch (assetType)
            {
                case RAAsset.Model:
                    return polygonRoot.AllChildren().Select(p => p.KIObject);
                case RAAsset.Texture:
                    return textureRoot.AllChildren().Select(p => p.KIObject);
                case RAAsset.ShaderProgram:
                    return shaderProgramRoot.AllChildren().Select(p => p.KIObject);
            }

            return null;
        }
    }
}
