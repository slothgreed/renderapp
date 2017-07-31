using System.Collections.Generic;
using OpenTK;
using KI.Foundation.Core;
using KI.Gfx.KIShader;
using RenderApp.AssetModel.RA_Geometry;
using KI.Analyzer;
using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Renderer;

namespace RenderApp.AssetModel
{
    //class AssetFactory : KIFactoryBase<RenderObject>
    //{
    //    private static AssetFactory _instance = new AssetFactory();
    //    public static AssetFactory Instance
    //    {
    //        get
    //        {
    //            return _instance;
    //        }
    //    }

    //    internal List<RenderObject> CreateWorld(string name, Vector3 min, Vector3 max)
    //    {
    //        Cube cube = new Cube("World", min, max);
    //        return cube.ConvertGeometrys(true);
    //    }
    //    internal List<RenderObject> CreateAxis(string name, Vector3 min, Vector3 max)
    //    {
    //        Axis axis = new Axis(name, min, max);
    //        RenderObject renderObject = new RenderObject(name);
    //        GeometryInfo info = new GeometryInfo(axis.Geometry.Position, null, axis.Geometry.Color, axis.Geometry.TexCoord, null, GeometryType.Line);
    //        renderObject.SetGeometryInfo(info);
    //        return new List<RenderObject> { renderObject };
    //    }
    //    internal List<RenderObject> CreateLoad3DModel(string filePath)
    //    {
    //        string extension = System.IO.Path.GetExtension(filePath);
    //        string fileName = System.IO.Path.GetFileName(filePath);
    //        switch (extension)
    //        {
    //            case ".obj":
    //                var obj = new OBJConverter(fileName, filePath);
    //                return obj.CreateRenderObject();
    //            case ".stl":
    //                var stl = new STLConverter(fileName, filePath);
    //                return stl.CreateRenderObject();
    //            case ".half":
    //                var half = new HalfEdge();
    //                HalfEdgeIO.ReadFile(filePath, half);
    //                var renderObject = new RenderObject(fileName);
    //                renderObject.SetGeometryInfo(half.CreateGeometryInfo());
    //                renderObject.HalfEdge = half;
    //                return new List<RenderObject> { renderObject };
    //            case ".ply":
    //                var ply = new PLYConverter(fileName, filePath);
    //                return ply.CreateRenderObject();
    //        }
    //        return null;
    //    }
    //}
}
