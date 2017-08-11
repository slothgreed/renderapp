using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Core;
using KI.Foundation.Tree;
using KI.Gfx;
using KI.Gfx.KITexture;

namespace RenderApp.Globals
{
    public class Project
    {
        /// <summary>
        /// 形状ルート
        /// </summary>
        private KINode geometryRoot;

        /// <summary>
        /// Textureオブジェクト
        /// </summary>
        private KINode textureRoot;

        /// <summary>
        /// シェーダオブジェクト
        /// </summary>
        private KINode shaderProgramRoot;

        public static Project ActiveProject = new Project();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Project()
        {
            RootNode = new KINode("ROOT");
            geometryRoot = new KINode("Geometry");
            textureRoot = new KINode("Texture");
            shaderProgramRoot = new KINode("ShaderProgram");
            RootNode.AddChild(geometryRoot);
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
            if (child is Geometry)
            {
                geometryRoot.AddChild(child);
                Workspace.MainScene.AddObject(child);
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
                    return geometryRoot.AllChildrenObject();
                case RAAsset.Texture:
                    return textureRoot.AllChildrenObject();
                case RAAsset.ShaderProgram:
                    return shaderProgramRoot.AllChildrenObject();
            }

            return null;
        }
    }
}
