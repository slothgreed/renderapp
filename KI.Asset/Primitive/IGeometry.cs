namespace KI.Asset
{
    /// <summary>
    /// 形状のプリミティブ種類
    /// </summary>
    public enum PrimitiveType
    {
        Axis,
        Plane,
        Sphere
    }

    /// <summary>
    /// 形状クラスのインタフェース
    /// </summary>
    public interface IGeometry
    {
        /// <summary>
        /// 形状
        /// </summary>
        Geometry[] Geometrys
        {
            get;
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        void CreateGeometry();
    }
}
