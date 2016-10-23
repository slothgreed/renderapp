using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
using RenderApp.AssetModel;
using RenderApp.Utility;
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

        public static Project ActiveProject = new Project();

        private Project()
        {
            RootNode = new RANode("ROOT");
            GeometryRoot = new RANode("Geometry");
            TextureRoot = new RANode("Texture");
            MaterialRoot = new RANode("Material");
            RootNode.AddChild(GeometryRoot);
            RootNode.AddChild(TextureRoot);
            RootNode.AddChild(MaterialRoot);
        
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
        }

        internal void Dispose()
        {
            GeometryRoot.Dispose();
            TextureRoot.Dispose();
            MaterialRoot.Dispose();
        }
    }
}
