using System;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx
{
    public static class TypeUtility
    {
        /// <summary>
        /// PolygonType から PrimitiveType への変換
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PrimitiveType ConvertPolygonType(PolygonType type)
        {
            switch (type)
            {
                case PolygonType.Points:
                    return PrimitiveType.Points;
                case PolygonType.Lines:
                    return PrimitiveType.Lines;
                case PolygonType.Triangles:
                    return PrimitiveType.Triangles;
                case PolygonType.Quads:
                    return PrimitiveType.Quads;
                case PolygonType.Patches:
                    return PrimitiveType.Patches;
                default:
                    throw new NotSupportedException();
            }

        }
    }
}
