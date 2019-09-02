using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// Primitive の Utility
    /// </summary>
    public static class PrimitiveUtility
    {

        /// <summary>
        /// Primitiveの移動
        /// </summary>
        /// <param name="primitive">Primitive</param>
        /// <param name="move">移動</param>
        public static void Move(PrimitiveBase primitive, Vector3 move)
        {
            foreach (var vertex in primitive.Vertexs)
            {
                vertex.Position += move;
            }
        }

        /// <summary>
        /// Primitiveの拡大縮小
        /// </summary>
        /// <param name="primitive">Primitive</param>
        /// <param name="scale">拡大縮小</param>
        public static void Scale(PrimitiveBase primitive, Vector3 scale)
        {
            foreach (var vertex in primitive.Vertexs)
            {
                vertex.Position *= scale;
            }
        }

        /// <summary>
        /// Primitive の回転
        /// </summary>
        /// <param name="primitive">Primitive</param>
        /// <param name="axis">軸</param>
        /// <param name="degree">角度</param>
        public static void Rotate(PrimitiveBase primitive, Vector3 axis, float degree)
        {
            var quart = Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(degree));
            var quartMat = Matrix4.CreateFromQuaternion(quart);
            foreach (var vertex in primitive.Vertexs)
            {
                vertex.Position = Matrix4Utility.Multiply(quartMat, vertex.Position);
            }
        }

        /// <summary>
        /// Primitiveの変換
        /// </summary>
        /// <param name="primitive">Primitive</param>
        /// <param name="matrix">変換行列</param>
        public static void Transform(PrimitiveBase primitive, Matrix4 matrix)
        {
            foreach (var vertex in primitive.Vertexs)
            {
                vertex.Position = Matrix4Utility.Multiply(matrix, vertex.Position);
            }
        }
    }
}
