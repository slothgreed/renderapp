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
            GeometryInfo info = new GeometryInfo(stlData.Position, stlData.Normal, Vector3.One, null, null, GeometryType.Triangle);
            GeometryInfos = new GeometryInfo[] { info };
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public GeometryInfo[] GeometryInfos
        {
            get;
            private set;
        }
    }
}
