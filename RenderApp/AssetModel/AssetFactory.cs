using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.GLUtil;
using RenderApp.AssetModel.LightModel;
using OpenTK;
using RenderApp.Utility;
using OpenTK.Graphics.OpenGL;
using RenderApp.Globals;
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
            string SphereMapAlbedo = ProjectInfo.TextureDirectory + @"\SphreMap.jpg";
            string SphereMapVertexShader = ProjectInfo.ShaderDirectory + @"\sphereMap.vert";
            string SphereMapFragmentShader = ProjectInfo.ShaderDirectory + @"\sphereMap.frag";
            Sphere sphere = new Sphere("SphereMap",Scene.ActiveScene.WorldMax.X * 2, 20, 20, false, Vector3.UnitX);
            sphere.MaterialItem = new Material("SphereMaterial");
            Texture texture = TextureFactory.Instance.CreateTexture(Asset.GetNameFromPath(SphereMapAlbedo), SphereMapAlbedo);
            sphere.MaterialItem.AddTexture(TextureKind.Albedo, texture);
            sphere.MaterialItem.AddTexture(TextureKind.Albedo, texture);
            Project.ActiveProject.AddChild(sphere.MaterialItem);
            Project.ActiveProject.AddChild(texture);
            return sphere;
        }

       



        internal Camera CreateMainCamera()
        {
            Camera camera = new Camera("MainCamera");
            Scene.ActiveScene.AddSceneObject(camera);
            return camera;
        }

        internal Light CreateSunLight()
        {
            Light light =  new PointLight("SunLight",new Vector3(10), Vector3.Zero);
            Scene.ActiveScene.AddSceneObject(light);
            return light;
        }
        internal Geometry CreateGeometry(Geometry geometry)
        {
            if(geometry.MaterialItem != null)
            {
                Scene.ActiveScene.AddSceneObject(geometry.MaterialItem);
            }
            Scene.ActiveScene.AddSceneObject(geometry);
            return geometry;
        }

        #region analyze method
        internal bool CanCreateGeometry(Asset asset)
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

        internal bool CreateWireFrame(Asset asset)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            List<Vector3> position = new List<Vector3>();
            for (int i = 0; i < geometry.Position.Count / 3; i++)
            {
                position.Add(geometry.Position[3 * i]);
                position.Add(geometry.Position[3 * i + 1]);

                position.Add(geometry.Position[3 * i + 1]);
                position.Add(geometry.Position[3 * i + 2]);

                position.Add(geometry.Position[3 * i + 2]);
                position.Add(geometry.Position[3 * i]);

            }
            Geometry wireframe = new Primitive("WireFrame :" + geometry.Key, position, CCalc.RandomColor(), PrimitiveType.Lines);
            wireframe.MaterialItem = new Material("Picking");
            wireframe.MaterialItem.SetShader(ShaderFactory.Instance.DefaultAnalyzeShader);
            CreateGeometry(wireframe);

            return true;
        }
        internal bool CreatePolygon(Asset asset)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            List<Vector3> position = new List<Vector3>(geometry.Position);
            List<Vector3> normal = new List<Vector3>(geometry.Normal);
            Geometry polygon = new Primitive("Polygon :" + geometry.Key, position, normal, new Vector3(0.7f, 0.7f, 0.7f), PrimitiveType.Triangles);
            polygon.MaterialItem = new Material("Polygon");
            polygon.MaterialItem.SetShader(ShaderFactory.Instance.DefaultAnalyzeShader);
            CreateGeometry(polygon);

            return true;
        }
        internal bool CreateVoxel(Asset asset, int partition = 64)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            Analyzer.Voxel voxel = new Analyzer.Voxel(geometry.Position, geometry.Index, geometry.ModelMatrix, partition);
            Geometry wireframe = new Primitive("Voxel :" + geometry.Key, voxel.vPosition, voxel.vNormal, CCalc.RandomColor(), PrimitiveType.Quads);
            wireframe.MaterialItem = new Material("Voxel");
            wireframe.MaterialItem.SetShader(ShaderFactory.Instance.DefaultAnalyzeShader);
            CreateGeometry(wireframe);

            return true;
        }

        internal bool CreateOctree(Asset asset)
        {
            if (!CanCreateGeometry(asset))
            {
                return false;
            }
            Geometry geometry = asset as Geometry;
            return true;
        }
        #endregion

        internal Plane CreatePlane(string name, Shader shader)
        {
            Plane plane;
            plane = new Plane(name);
            plane.MaterialItem = new Material(name);
            Project.ActiveProject.AddChild(plane.MaterialItem);
            plane.MaterialItem.SetShader(shader);
            return plane;
        }
    }
}
