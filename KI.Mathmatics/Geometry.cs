using System;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// 幾何学演算
    /// </summary>
    public static class Geometry
    {
        /// <summary>
        /// 法線の算出
        /// </summary>
        /// <param name="vertex1">ベクトル1</param>
        /// <param name="vertex2">ベクトル2</param>
        /// <param name="vertex3">ベクトル3</param>
        /// <returns>法線</returns>
        public static Vector3 Normal(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            Vector3 point = vertex2 - vertex1;
            Vector3 point2 = vertex3 - vertex1;
            Vector3 normal = new Vector3();
            normal = Vector3.Cross(point, point2);

            return normal.Normalized();
        }

        /// <summary>
        /// 法線の算出
        /// </summary>
        /// <param name="vector1">ベクトル1</param>
        /// <param name="vector2">ベクトル2</param>
        /// <returns>法線</returns>
        public static Vector3 Normal(Vector3 vector1, Vector3 vector2)
        {
            Vector3 normal;
            normal = Vector3.Cross(vector1, vector2);
            return normal.Normalized();
        }

        /// <summary>
        /// 三角形の面積の算出
        /// </summary>
        /// <param name="tri1">頂点1</param>
        /// <param name="tri2">頂点2</param>
        /// <param name="tri3">頂点3</param>
        /// <returns>面積</returns>
        public static float Area(Vector3 tri1, Vector3 tri2, Vector3 tri3)
        {
            float edge1 = (tri1 - tri2).Length;
            float edge2 = (tri2 - tri3).Length;
            float edge3 = (tri3 - tri1).Length;
            float sum = (edge1 + edge2 + edge3) / 2;

            float value = sum * (sum - edge1) * (sum - edge2) * (sum - edge3);

            return (float)Math.Sqrt(value);
        }

        /// <summary>
        /// 仮想タンジェントベクトルの算出
        /// </summary>
        /// <param name="normal">法線</param>
        /// <param name="tangent1">タンジェント1</param>
        /// <param name="tangent2">タンジェント2</param>
        public static void VirtualTangent(Vector3 normal, out Vector3 tangent1, out Vector3 tangent2)
        {
            if (Math.Abs(normal.X) < Math.Abs(normal.Y))
            {
                tangent1.X = 0;
                tangent1.Y = -normal.Z;
                tangent1.Z = normal.Y;
            }
            else
            {
                tangent1.X = normal.Z;
                tangent1.Y = 0;
                tangent1.Z = -normal.X;
            }

            tangent1.Normalize();
            tangent2 = Vector3.Cross(normal, tangent1);
            tangent2.Normalize();
        }
    }
}
