using KI.Gfx.Geometry;

namespace KI.Asset
{
    /// <summary>
    /// 形状クラスのインタフェース
    /// </summary>
    public interface ICreateModel
    {
        /// <summary>
        /// 形状
        /// </summary>
        Polygon Model
        {
            get;
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        void CreateModel();
    }
}
