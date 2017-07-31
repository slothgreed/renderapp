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
        STLLoader stlData;

        public GeometryInfo[] GeometryInfos
        {
            get;
            private set;
        }

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="position"></param>
        public STLConverter(string name,string filePath)
        {
            stlData = new STLLoader(name, filePath);
            GeometryInfo info = new GeometryInfo(stlData.Position, stlData.Normal, Vector3.One, null, null, GeometryType.Triangle);
            GeometryInfos = new GeometryInfo[] { info };

        }
    }
}
