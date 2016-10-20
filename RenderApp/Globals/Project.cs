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
        public Node RootNode;
        private Node GeometryRoot;
        /// <summary>
        /// Textureオブジェクト
        /// </summary>
        private Node TextureRoot;
        /// <summary>
        /// マテリアルオブジェクト
        /// </summary>
        private Node MaterialRoot;

        public static Project ActiveProject = new Project();

        private Project()
        {
            RootNode = new Node("ROOT");
            GeometryRoot = new Node("Geometry");
            TextureRoot = new Node("Texture");
            MaterialRoot = new Node("Material");
            RootNode.AddChild(GeometryRoot);
            RootNode.AddChild(TextureRoot);
            RootNode.AddChild(MaterialRoot);
        
        }

        internal void AddChild(MyObject value)
        {
            if (value is Geometry)
            {
                GeometryRoot.AddChild(value);
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
