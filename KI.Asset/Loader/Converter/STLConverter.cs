using KI.Asset.Loader;
using KI.Gfx.GLUtil;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class STLConverter : IGeometry
    {
        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public STLConverter(string filePath)
        {
            var stlData = new STLLoader(filePath);
            Geometry info = new Geometry(stlData.Position, stlData.Normal, Vector3.One, null, null, GeometryType.Triangle);
            GeometryInfos = new Geometry[] { info };
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Geometry[] GeometryInfos
        {
            get;
            private set;
        }
    }
}
