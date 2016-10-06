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
        public Dictionary<string, Asset> assetList = new Dictionary<string, Asset>();

        private void AddItem(Asset asset)
        {
            assetList.Add(asset.Key, asset);
        }
        public Asset FindItem(string key)
        {
            if (assetList.ContainsKey(key))
            {
                return assetList[key];
            }
            else
            {
                return null;
            }
        }
        public void RemoveItem(string key)
        {
            if (assetList.ContainsKey(key))
            {
                assetList[key].Dispose();
                assetList.Remove(key);
            }
        }
        public void Dispose()
        {
            foreach (var loop in assetList.Values)
            {
                loop.Dispose();
            }
            assetList.Clear();
        }

        public Geometry CreateEnvironmentMap()
        {
            string SphereMapAlbedo = Project.TextureDirectory + @"\SphreMap.jpg";
            string SphereMapVertexShader = Project.ShaderDirectory + @"\sphereMap.vert";
            string SphereMapFragmentShader = Project.ShaderDirectory + @"\sphereMap.frag";
            Sphere sphere = new Sphere("SphereMap",Scene.ActiveScene.WorldMax.X * 2, 20, 20, false, Vector3.UnitX);
            sphere.MaterialItem = new Material("SphereMaterial");
            Texture texture = TextureFactory.Instance.CreateTexture(Asset.GetNameFromPath(SphereMapAlbedo), SphereMapAlbedo);
            sphere.MaterialItem.AddTexture(TextureKind.Albedo, texture);
            sphere.MaterialItem.AddTexture(TextureKind.Albedo, texture);
            Scene.ActiveScene.AddSceneObject(sphere.MaterialItem.Key, sphere.MaterialItem);
            Scene.ActiveScene.AddSceneObject(texture.Key, texture);
            AddItem(sphere);
            return sphere;
        }

       



        internal Camera CreateMainCamera()
        {
            Camera camera = new Camera("MainCamera");
            Scene.ActiveScene.AddSceneObject(camera.Key, camera);
            AddItem(camera);
            return camera;
        }

        internal Light CreateSunLight()
        {
            Light light =  new PointLight("SunLight",new Vector3(10), Vector3.Zero);
            Scene.ActiveScene.AddSceneObject(light.Key, light);
            AddItem(light);
            return light;
        }
        internal Geometry CreateGeometry(Geometry geometry)
        {
            if(geometry.MaterialItem != null)
            {
                Scene.ActiveScene.AddSceneObject(geometry.Key, geometry.MaterialItem);
            }
            Scene.ActiveScene.AddSceneObject(geometry.Key,geometry);
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
    }
}
