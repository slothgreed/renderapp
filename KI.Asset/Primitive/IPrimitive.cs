namespace KI.Asset
{
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
        GeometryInfo[] GeometryInfos
        {
            get;
        }
    }
}
