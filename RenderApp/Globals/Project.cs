using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
using RenderApp.AssetModel;
using RenderApp.Utility;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.Globals
{
    public class Project
    {
        public RANode RootNode;
        private RANode GeometryRoot;
        /// <summary>
        /// Textureオブジェクト
        /// </summary>
        private RANode TextureRoot;
        /// <summary>
        /// マテリアルオブジェクト
        /// </summary>
        private RANode MaterialRoot;
        /// <summary>
        /// シェーダオブジェクト
        /// </summary>
        private RANode ShaderProgramRoot;
        public static Project ActiveProject = new Project();

        private Project()
        {
            RootNode = new RANode("ROOT");
            GeometryRoot = new RANode("Geometry");
            TextureRoot = new RANode("Texture");
            MaterialRoot = new RANode("Material");
            ShaderProgramRoot = new RANode("ShaderProgram");
            RootNode.AddChild(GeometryRoot);
            RootNode.AddChild(TextureRoot);
            RootNode.AddChild(MaterialRoot);
            RootNode.AddChild(ShaderProgramRoot);
        
        }

        internal void AddChild(RAObject value)
        {
            if (value is Geometry)
            {
                GeometryRoot.AddChild(value);
                Scene.ActiveScene.AddRootSceneObject(value);
            }
            if (value is Texture)
            {
                TextureRoot.AddChild(value);
            }
            if (value is Material)
            {
                MaterialRoot.AddChild(value);
            }
            if(value is ShaderProgram)
            {
                ShaderProgramRoot.AddChild(value);
            }
        }
        internal IEnumerable<RAObject> GetObject(RAAsset assetType)
        {
            switch (assetType)
            {
                case RAAsset.Model:
                    return GeometryRoot.AllChildrenObject();
                case RAAsset.Texture:
                    return TextureRoot.AllChildrenObject();
                case RAAsset.Material:
                    return MaterialRoot.AllChildrenObject();
                case RAAsset.ShaderProgram:
                    return ShaderProgramRoot.AllChildrenObject();
            }
            return null;
        }
        internal void Dispose()
        {
            GeometryRoot.Dispose();
            TextureRoot.Dispose();
            MaterialRoot.Dispose();
        }
    }
}
