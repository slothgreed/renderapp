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
            Sphere sphere = new Sphere(Scene.ActiveScene.WorldMax.X * 2, 20, 20, false, Vector3.UnitX);
            sphere.MaterialItem = new Material(null);
            sphere.MaterialItem.SetShader(new Shader(new ShaderProgram(Global.SphereMapVertexShader), new ShaderProgram(Global.SphereMapFragmentShader)));
            sphere.MaterialItem.AddTexture(TextureKind.Albedo, new Texture(Global.SphereMapAlbedo));

            return sphere;
        }

        internal Camera CreateMainCamera()
        {
            return new Camera();
        }

        internal Light CreateSunLight()
        {
            return new PointLight(new Vector3(10), Vector3.Zero);
        }
    }
}
