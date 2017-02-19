using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.GLUtil;
using OpenTK;
using RenderApp.Utility;
using OpenTK.Graphics.OpenGL;
using RenderApp.Globals;
using RenderApp.AssetModel.RA_Geometry;
using KI.Foundation.Core;
using KI.Gfx.KIAsset;
using KI.Foundation.Utility;

namespace RenderApp.AssetModel
{
    class AssetFactory
    {
        private static AssetFactory _instance = new AssetFactory();
        public static AssetFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public Geometry CreateEnvironmentMap()
        {
            return null;
            //string SphereMapAlbedo = ProjectInfo.TextureDirectory + @"\SphreMap.jpg";
            //string SphereMapVertexShader = ProjectInfo.ShaderDirectory + @"\sphereMap.vert";
            //string SphereMapFragmentShader = ProjectInfo.ShaderDirectory + @"\sphereMap.frag";
            //Sphere sphere = new Sphere("SphereMap",SceneManager.Instance.ActiveScene.WorldMax.X * 2, 20, 20, false, Vector3.UnitX);
            //sphere.geometry.MaterialItem = new Material("SphereMaterial");
            //Texture texture = TextureFactory.Instance.CreateTexture(SphereMapAlbedo);
            //sphere.geometry.MaterialItem.AddTexture(TextureKind.Albedo, texture);
            //sphere.geometry.MaterialItem.AddTexture(TextureKind.Albedo, texture);
            //Project.ActiveProject.AddChild(sphere.geometry.MaterialItem);
            //Project.ActiveProject.AddChild(texture);
            //return sphere.geometry;
        }
        public Material CreateMaterial(string name)
        {
            Material material = new Material(name);
            Project.ActiveProject.AddChild(material);
            return material;
        }
        internal Camera CreateCamera(string name)
        {
            return new Camera(name);
        }

        internal Light CreateLight(string name)
        {
            return new PointLight(name, new Vector3(10), Vector3.Zero);
        }
        internal Geometry CreateGeometry(Geometry geometry)
        {
            if(geometry.MaterialItem != null)
            {
                SceneManager.Instance.ActiveScene.AddObject(geometry);
            }
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
            for (int i = 0; i < geometry.GeometryInfo.Position.Count / 3; i++)
            {
                position.Add(geometry.GeometryInfo.Position[3 * i]);
                position.Add(geometry.GeometryInfo.Position[3 * i + 1]);

                position.Add(geometry.GeometryInfo.Position[3 * i + 1]);
                position.Add(geometry.GeometryInfo.Position[3 * i + 2]);

                position.Add(geometry.GeometryInfo.Position[3 * i + 2]);
                position.Add(geometry.GeometryInfo.Position[3 * i]);

            }
            RenderObject wireframe = new RenderObject("WireFrame :" + geometry.Name);
            wireframe.CreatePC(position, KICalc.RandomColor(), PrimitiveType.Lines);
            CreateGeometry(wireframe);

            return true;
        }
        internal bool CreatePolygon(KIObject asset)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            List<Vector3> position = new List<Vector3>(geometry.GeometryInfo.Position);
            List<Vector3> normal = new List<Vector3>(geometry.GeometryInfo.Normal);
            RenderObject polygon = new RenderObject("Polygon :" + geometry.Name);
            polygon.CreatePNC(position, normal, new Vector3(0.7f, 0.7f, 0.7f), PrimitiveType.Triangles);
            CreateGeometry(polygon);

            return true;
        }
        internal bool CreateVoxel(KIObject asset, int partition = 64)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            KI.Gfx.Analyzer.Voxel voxel = new KI.Gfx.Analyzer.Voxel(geometry.GeometryInfo.Position, geometry.GeometryInfo.Index, geometry.ModelMatrix, partition);
            RenderObject wireframe = new RenderObject("Voxel :" + geometry.Name);
            wireframe.CreatePNC(voxel.vPosition, voxel.vNormal, KICalc.RandomColor(), PrimitiveType.Quads);
            CreateGeometry(wireframe);

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
            Geometry renderObject = plane.CreateRenderObject().First();
            renderObject.MaterialItem = CreateMaterial(name);
            Project.ActiveProject.AddChild(renderObject.MaterialItem);
            renderObject.MaterialItem.CurrentShader = shader;
            return renderObject;
        }

        internal Geometry CreatePostProcessPlane(string name)
        {
            Plane plane;
            plane = new Plane(name);
            Geometry renderObject = plane.CreateRenderObject().First();
            renderObject.MaterialItem = CreateMaterial(name);
            Project.ActiveProject.AddChild(renderObject.MaterialItem);
            return renderObject;
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
            }
            return null;
        }
    }
}
