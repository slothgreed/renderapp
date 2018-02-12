using KI.Gfx.Geometry;

namespace KI.Asset
{
    /// <summary>
    /// 形状クラスのインタフェース
    /// </summary>
    public interface IPolygon
    {
        /// <summary>
        /// 形状
        /// </summary>
        Polygon[] Polygons
        {
            get;
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        void CreatePolygon();
    }
}
