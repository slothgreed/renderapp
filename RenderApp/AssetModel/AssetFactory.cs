using System.Collections.Generic;
using System.Linq;
using OpenTK;
using KI.Foundation.Core;
using KI.Gfx.KIAsset;
using KI.Gfx.Analyzer;
using KI.Foundation.Utility;
using KI.Gfx.KIShader;
using RenderApp.AssetModel.RA_Geometry;
namespace RenderApp.AssetModel
{
    class AssetFactory : KIFactoryBase<RenderObject>
    {
        private static AssetFactory _instance = new AssetFactory();
        public static AssetFactory Instance
        {
            get
            {
                return _instance;
            }
        }


        public RenderObject CreateRenderObject(string name)
        {
            RenderObject renderObject = new RenderObject(name);
            AddItem(renderObject);
            return renderObject;
        }

        public EnvironmentProbe CreateEnvironmentMap(string name)
        {
            return new EnvironmentProbe(name);
        }
        internal Camera CreateCamera(string name)
        {
            return new Camera(name);
        }

        internal Light CreateLight(string name)
        {
            return new PointLight(name, new Vector3(-11, 300, -18), Vector3.Zero);
        }

        internal Geometry CreatePlane(string name, Shader shader)
        {
            Plane plane;
            plane = new Plane(name);
            RenderObject renderObject = new RenderObject(name);
            GeometryInfo info = new GeometryInfo(plane.Geometry.Position, null, null, plane.Geometry.TexCoord, null,GeometryType.Quad);
            renderObject.SetGeometryInfo(info);
            renderObject.Shader = shader;
            return renderObject;
        }

        internal Geometry CreatePostProcessPlane(string name)
        {
            Plane plane = new Plane(name);
            RenderObject renderObject = new RenderObject(name);
            GeometryInfo info = new GeometryInfo(plane.Geometry.Position, null, null, plane.Geometry.TexCoord, null, GeometryType.Quad);
            renderObject.SetGeometryInfo(info);
            return renderObject;
        }

        internal List<RenderObject> CreateWorld(string name, Vector3 min, Vector3 max)
        {
            Cube cube = new Cube("World", min, max);
            return cube.ConvertGeometrys(true);
        }

        internal List<RenderObject> CreateLoad3DModel(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            string fileName = System.IO.Path.GetFileName(filePath);
            switch (extension)
            {
                case ".obj":
                    var obj = new OBJConverter(fileName, filePath);
                    return obj.CreateRenderObject();
                case ".stl":
                    var stl = new STLConverter(fileName, filePath);
                    return stl.CreateRenderObject();
                case ".half":
                    var half = new HalfEdge();
                    half.ReadFile(filePath);
                    var renderObject = new RenderObject(fileName);
                    renderObject.SetGeometryInfo(half.CreateGeometryInfo());
                    return new List<RenderObject> { renderObject };
            }
            return null;
        }

    }
}
