using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.GLUtil;
using RenderApp.AssetModel.LightModel;
using OpenTK;
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
            string SphereMapAlbedo = Project.TextureDirectory + @"\SphreMap.jpg";
            string SphereMapVertexShader = Project.ShaderDirectory + @"\sphereMap.vert";
            string SphereMapFragmentShader = Project.ShaderDirectory + @"\sphereMap.frag";
            Sphere sphere = new Sphere("SphereMap",Scene.ActiveScene.WorldMax.X * 2, 20, 20, false, Vector3.UnitX);
            sphere.MaterialItem = new Material("SphereMaterial");
            Texture texture = new Texture(Asset.GetNameFromPath(SphereMapAlbedo), SphereMapAlbedo);
            sphere.MaterialItem.AddTexture(TextureKind.Albedo, texture);
            sphere.MaterialItem.SetShader(ShaderFactory.Instance.DefaultDefferredShader);
            sphere.MaterialItem.AddTexture(TextureKind.Albedo, texture);
            Scene.ActiveScene.AddSceneObject(sphere.MaterialItem.Key, sphere.MaterialItem);
            Scene.ActiveScene.AddSceneObject(texture.Key, texture);

            return sphere;
        }

        internal Camera CreateMainCamera()
        {
            return new Camera("MainCamera");
        }

        internal Light CreateSunLight()
        {
            return new PointLight("SunLight",new Vector3(10), Vector3.Zero);
        }
    }
}
