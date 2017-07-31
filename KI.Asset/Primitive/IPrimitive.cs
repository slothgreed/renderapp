namespace KI.Asset
{
    public interface IGeometry
    {
        GeometryInfo[] GeometryInfos
        {
            get;
        }
    }

    public enum PrimitiveType
    {
        Axis,
        Plane,
        Sphere
    }
}
