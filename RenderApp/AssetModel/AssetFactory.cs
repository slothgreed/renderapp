using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Foundation.Core;
using KI.Gfx.KIAsset;
using KI.Gfx.Analyzer;
using KI.Foundation.Utility;
using KI.Gfx.KIShader;
using RenderApp.AssetModel.RA_Geometry;
namespace RenderApp.AssetModel
{
    class AssetFactory : KIFactoryBase< RenderObject>
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
        internal Geometry CreateGeometry(Geometry geometry)
        {
            SceneManager.Instance.ActiveScene.AddObject(geometry);
            return geometry;
        }

        #region analyze method
        internal bool CanCreateGeometry(KIObject asset)
        {
            if (!(asset is Geometry))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            if (geometry.RenderType != OpenTK.Graphics.OpenGL.PrimitiveType.Triangles)
            {
                return false;
            }
            return true;
        }

        internal bool CreateWireFrame(KIObject asset)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            List<Vector3> position = new List<Vector3>();
            if (geometry.geometryInfo.Index.Count != 0)
            {
                for (int i = 0; i < geometry.geometryInfo.Index.Count / 3; i++)
                {
                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i]]);
                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i + 1]]);

                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i + 1]]);
                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i + 2]]);

                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i + 2]]);
                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i]]);
                }
            }
            else
            {
                for (int i = 0; i < geometry.geometryInfo.Position.Count / 3; i++)
                {
                    position.Add(geometry.geometryInfo.Position[3 * i]);
                    position.Add(geometry.geometryInfo.Position[3 * i + 1]);

                    position.Add(geometry.geometryInfo.Position[3 * i + 1]);
                    position.Add(geometry.geometryInfo.Position[3 * i + 2]);

                    position.Add(geometry.geometryInfo.Position[3 * i + 2]);
                    position.Add(geometry.geometryInfo.Position[3 * i]);

                }
            }
            RenderObject wireframe = AssetFactory.Instance.CreateRenderObject("WireFrame :" + geometry.Name);
            wireframe.CreateGeometryInfo(new GeometryInfo(position, null, KICalc.RandomColor(), null, null,GeometryType.Line), PrimitiveType.Lines);
            wireframe.ModelMatrix = geometry.ModelMatrix;
            CreateGeometry(wireframe);

            //Geometry geometry = asset as Geometry;
            //HalfEdge half = new HalfEdge(geometry.geometryInfo);
            //half.WriteFile(@"C:/Users/ido/Documents/KIProject/renderapp/RenderApp/Resource/Model/cube.half");
            //half.ReadFile(@"C:/Users/ido/Documents/KIProject/renderapp/RenderApp/Resource/Model/cube.half");

            //GeometryInfo info = half.CreateGeometryInfo();
            //RenderObject halfEdge = AssetFactory.Instance.CreateRenderObject("HalfEdge :" + geometry.Name);
            //halfEdge.CreateGeometryInfo(info, PrimitiveType.Triangles);
            //halfEdge.ModelMatrix = geometry.ModelMatrix;
            //CreateGeometry(halfEdge);

            return true;
        }

        internal bool CreateHalfEdge(KIObject asset)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            return true;
        }

        internal bool CreatePolygon(KIObject asset)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            geometry.geometryInfo.ConvertVertexArray();
            geometry.SetupBuffer();
            //List<Vector3> position = new List<Vector3>(geometry.GeometryInfo.Position);
            //List<Vector3> normal = new List<Vector3>(geometry.GeometryInfo.Normal);
            //RenderObject polygon = AssetFactory.Instance.CreateRenderObject("Polygon :" + geometry.Name);
            //polygon.CreatePNC(position, normal, new Vector3(0.7f, 0.7f, 0.7f), PrimitiveType.Triangles);
            //CreateGeometry(polygon);

            return true;
        }
        internal bool CreateVoxel(KIObject asset, int partition = 64)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            KI.Gfx.Analyzer.Voxel voxel = new KI.Gfx.Analyzer.Voxel(geometry.geometryInfo.Position, geometry.geometryInfo.Index, geometry.ModelMatrix, partition);
            RenderObject voxelObject = AssetFactory.Instance.CreateRenderObject("Voxel :" + geometry.Name);
            GeometryInfo info = new GeometryInfo(voxel.vPosition, voxel.vNormal, KICalc.RandomColor(), null, null,GeometryType.Quad);
            voxelObject.CreateGeometryInfo(info, PrimitiveType.Quads);
            voxelObject.Transformation(geometry.ModelMatrix);
            CreateGeometry(voxelObject);

            return true;
        }

        internal bool CreateOctree(KIObject asset)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            return true;
        }
        #endregion

        internal Geometry CreatePlane(string name, Shader shader)
        {
            Plane plane;
            plane = new Plane(name);
            RenderObject renderObject = new RenderObject(name);
            GeometryInfo info = new GeometryInfo(plane.Geometry.Position, null, null, plane.Geometry.TexCoord, null,GeometryType.Quad);
            renderObject.CreateGeometryInfo(info, PrimitiveType.Quads);
            renderObject.Shader = shader;
            return renderObject;
        }

        internal Geometry CreatePostProcessPlane(string name)
        {
            Plane plane = new Plane(name);
            RenderObject renderObject = new RenderObject(name);
            GeometryInfo info = new GeometryInfo(plane.Geometry.Position, null, null, plane.Geometry.TexCoord, null, GeometryType.Quad);
            renderObject.CreateGeometryInfo(info, PrimitiveType.Quads);
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
                    GeometryInfo info = half.CreateGeometryInfo();
                    int r = 100;
                    List<Vector3> Color = new List<Vector3>();
                    foreach (var vertex in half.m_Vertex)
                    {
                        Color.Add(Vector3.UnitX);
                    }
                    for (int i = 0; i < half.m_Vertex.Count; i++)
                    {
                        if (i % r == 0)
                        {
                            var color = KICalc.RandomColor();
                            Color[half.m_Vertex[i].Index] = color;
                            foreach (var vertex in half.m_Vertex[i].AroundVertex)
                            {
                                Color[vertex.Index] = color;
                            }
                        }
                    }

                    info.Color = Color;
                    renderObject.CreateGeometryInfo(info, PrimitiveType.Triangles);
                    return new List<RenderObject> { renderObject };
            }
            return null;
        }

    }
}
