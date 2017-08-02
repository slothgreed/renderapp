using OpenTK;
using KI.Foundation.Core;
using System.Collections.Generic;
using KI.Analyzer;

namespace KI.Asset
{
    /// <summary>
    /// アセットファクトリ
    /// </summary>
    public class AssetFactory : KIFactoryBase<Geometry>
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static AssetFactory Instance { get; } = new AssetFactory();

        /// <summary>
        /// 環境プローブの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>環境プローブ</returns>
        public EnvironmentProbe CreateEnvironmentMap(string name)
        {
            return new EnvironmentProbe(name);
        }

        /// <summary>
        /// カメラの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>カメラ</returns>
        public Camera CreateCamera(string name)
        {
            return new Camera(name);
        }

        /// <summary>
        /// ライトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>ライト</returns>
        public Light CreateLight(string name)
        {
            return new PointLight(name, new Vector3(-11, 300, -18), Vector3.Zero);
        }

        public IGeometry CreateAxis(string name, Vector3 min, Vector3 max)
        {
            return new Axis(name, min, max);
        }

        public IGeometry CreateSphere(string name, float radial, int hpartition, int wpartition, bool orient)
        {
            return new Sphere(name, radial, hpartition, wpartition, orient);
        }

        public IGeometry CreatePlane(string name)
        {
            return new Plane(name);
        }

        //public List<RenderObject> CreateWorld(string name, Vector3 min, Vector3 max)
        //{
        //    Cube cube = new Cube("World", min, max);
        //    return cube.ConvertGeometrys(true);
        //}

        public IGeometry CreateLoad3DModel(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            string fileName = System.IO.Path.GetFileName(filePath);
            switch (extension)
            {
                case ".obj":
                    return new OBJConverter(filePath);
                case ".stl":
                    return new STLConverter(filePath);
                case ".half":
                    return new HalfEdgeConverter(filePath);
                case ".ply":
                    return new PLYConverter(filePath);
            }

            return null;
        }

        public IGeometry CreateHalfEdge(List<Vector3> position, List<int> index)
        {
            return new HalfEdgeConverter(new HalfEdge(position, index));
        }
    }
}
