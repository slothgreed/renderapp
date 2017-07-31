using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Asset;
using KI.Foundation.Core;

namespace KI.Renderer
{
    public class RenderObjectFactory : KIFactoryBase<RenderObject>
    {
        private static RenderObjectFactory _instance = new RenderObjectFactory();
        public static RenderObjectFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public RenderObject CreateRenderObject(string name)
        {
            return new RenderObject(name);
        }

        public RenderObject CreateRenderObject(string name, IGeometry primitive)
        {
            RenderObject renderObject = CreateRenderObject(name);
            renderObject.SetGeometryInfo(primitive.GeometryInfos.First());
            return renderObject;
        }

        public IEnumerable<RenderObject> CreateRenderObjects(string name, IGeometry primitive)
        {
            var list = new List<RenderObject>();
            foreach(var info in primitive.GeometryInfos)
            {
                RenderObject renderObject = CreateRenderObject(name);
                renderObject.SetGeometryInfo(info);
            }
            return list;
        }
    }
}
