using KI.Asset.Loader.Importer;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// アセットファクトリ
    /// </summary>
    public class AssetFactory : KIFactoryBase<Polygon>
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static AssetFactory Instance { get; } = new AssetFactory();

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
        /// 軸の作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>軸</returns>
        public ICreateModel CreateAxis(string name, Vector3 min, Vector3 max)
        {
            return new Axis(name, min, max);
        }

        /// <summary>
        /// グリッド平面の作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="area">大きさ</param>
        /// <param name="space">間隔</param>
        /// <param name="color">色</param>
        /// <returns>グリッド平面</returns>
        public ICreateModel CreateGridPlane(string name, float area, float space, Vector3 color)
        {
            return new GridPlane(name, area, space, color);
        }

        /// <summary>
        /// 球の作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="radial">半径</param>
        /// <param name="hpartition">縦分割数</param>
        /// <param name="wpartition">横分割数</param>
        /// <param name="orient">内外</param>
        /// <returns>球</returns>
        public ICreateModel CreateSphere(string name, float radial, int hpartition, int wpartition, bool orient)
        {
            return new Sphere(name, radial, hpartition, wpartition, orient);
        }

        /// <summary>
        /// 平面の作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>平面</returns>
        public ICreateModel CreateRectangle(string name)
        {
            return new Rectangle(name);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="radial">半径</param>
        /// <param name="smoothNum">スムージング回数</param>
        public ICreateModel CreateIcosahedron(string name, float radial, int smoothNum)
        {
            return new Icosahedron(name, radial, smoothNum);
        }

        /// <summary>
        /// 3dモデルのロード
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>3dモデル</returns>
        public ICreateModel CreateLoad3DModel(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            string fileName = System.IO.Path.GetFileName(filePath);
            switch (extension)
            {
                // obj is not single object.
                //case ".obj":
                //    return new OBJConverter(filePath);
                case ".stl":
                    return new STLImporter(filePath);
                case ".half":
                    return new HalfEdgeImporter(filePath);
                case ".ply":
                    return new PLYImporter(filePath);
            }

            return null;
        }
    }
}
