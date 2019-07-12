using KI.Asset.Loader.Importer;
using KI.Asset.Primitive;
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
