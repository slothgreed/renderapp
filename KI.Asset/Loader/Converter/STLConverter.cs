using System;
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
        /// stlファイルのローダ
        /// </summary>
        private STLLoader stlData;

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public STLConverter(string filePath)
        {
            stlData = new STLLoader(filePath);
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Geometry[] Geometrys
        {
            get;
            private set;
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateGeometry()
        {
            Geometry info = new Geometry(stlData.Position, stlData.Normal, Vector3.One, null, null, GeometryType.Triangle);
            Geometrys = new Geometry[] { info };
        }
    }
}
