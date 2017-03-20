using System.Collections.Generic;
using RenderApp.AssetModel;
using KI.Foundation.Core;
using KI.Foundation.Tree;
using KI.Gfx;
using KI.Gfx.KIAsset;

namespace RenderApp.Globals
{
    public class Project
    {
        public KINode RootNode;
        private KINode GeometryRoot;
        /// <summary>
        /// Textureオブジェクト
        /// </summary>
        private KINode TextureRoot;
        /// <summary>
        /// シェーダオブジェクト
        /// </summary>
        private KINode ShaderProgramRoot;
        public static Project ActiveProject = new Project();

        private Project()
        {
            RootNode = new KINode("ROOT");
            GeometryRoot = new KINode("Geometry");
            TextureRoot = new KINode("Texture");
            ShaderProgramRoot = new KINode("ShaderProgram");
            RootNode.AddChild(GeometryRoot);
            RootNode.AddChild(TextureRoot);
            RootNode.AddChild(ShaderProgramRoot);
        
        }

        internal void AddChild(KIObject value)
        {
            if (value is Geometry)
            {
                GeometryRoot.AddChild(value);
                SceneManager.Instance.ActiveScene.AddObject(value);
            }
            if (value is Texture)
            {
                TextureRoot.AddChild(value);
            }
            if(value is ShaderProgram)
            {
                ShaderProgramRoot.AddChild(value);
            }
        }
        internal IEnumerable<KIObject> GetObject(RAAsset assetType)
        {
            switch (assetType)
            {
                case RAAsset.Model:
                    return GeometryRoot.AllChildrenObject();
                case RAAsset.Texture:
                    return TextureRoot.AllChildrenObject();
                case RAAsset.ShaderProgram:
                    return ShaderProgramRoot.AllChildrenObject();
            }
            return null;
        }
        internal void Dispose()
        {
        }
    }
}
