using System;
using System.Collections.Generic;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset.Loader.Converter
{
    public static class ConverterUtility
    {
        /// <summary>
        /// オブジェクトを(-1,-1,-1)(1,1,1)の空間に正規化する
        /// </summary>
        /// <param name="vertexs">頂点</param>
        public static void NormalizeObject(List<Vertex> vertexs)
        {
            var sum = Vector3.Zero;
            var min = vertexs[0].Position;
            var max = vertexs[0].Position;
            foreach (var vertex in vertexs)
            {
                min = Vector3.ComponentMin(min, vertex.Position);
                max = Vector3.ComponentMax(max, vertex.Position);
            }

            max -= min;
            float maxValue = Math.Max(max.X, max.Y);
            maxValue = Math.Max(maxValue, max.Z);

            var half = new Vector3(0.5f);
            foreach (var vertex in vertexs)
            {
                vertex.Position = (vertex.Position - min);
                vertex.Position = Vector3.Divide(vertex.Position, maxValue);
                vertex.Position -= half;
            }

        }
    }
}
