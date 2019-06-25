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
        public static PrimitiveType ConvertPolygonType(KIPrimitiveType type)
        {
            switch (type)
            {
                case KIPrimitiveType.Points:
                    return PrimitiveType.Points;
                case KIPrimitiveType.Lines:
                    return PrimitiveType.Lines;
                case KIPrimitiveType.Triangles:
                    return PrimitiveType.Triangles;
                case KIPrimitiveType.Quads:
                    return PrimitiveType.Quads;
                case KIPrimitiveType.Patches:
                    return PrimitiveType.Patches;
                default:
                    throw new NotSupportedException();
            }

        }
    }
}
