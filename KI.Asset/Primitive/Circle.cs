using KI.Foundation.Core;
using KI.Gfx;
using KI.Mathmatics;
using OpenTK;
using System;
using System.Collections.Generic;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// 円
    /// </summary>
    public class Circle
    {
        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; private set; }

        /// <summary>
        /// 中心
        /// </summary>
        public Vector3 Center { get; private set; }

        /// <summary>
        /// 円の向き
        /// </summary>
        public Vector3 Normal { get; private set; }

        /// <summary>
        /// 分割方向
        /// </summary>
        public int Partition { get; private set; }
        
        /// <summary>
        /// 頂点情報
        /// </summary>
        public Vector3[] Position { get; private set; }

        /// <summary>
        /// 頂点インデクス
        /// </summary>
        public int[] Index { get; private set; }

        /// <summary>
        /// タイプ
        /// </summary>
        public PolygonType Type { get; private set; } = PolygonType.LineLoop;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="center">位置</param>
        /// <param name="normal">円の向き</param>
        /// <param name="partition">分割数</param>
        public Circle(float radius, Vector3 center, Vector3 normal, int partition)
        {
            Radius = radius;
            Center = center;
            Normal = normal;
            Partition = partition;
            Create();
        }

        private void Create()
        {
            List<Vector3> position = new List<Vector3>();
            List<int> index = new List<int>();

            float theta = (float)Math.PI * 2 / Partition;
            var ex = Vector3.Cross(Normal, Vector3.UnitZ);
            Quaternion quart = Quaternion.FromAxisAngle(ex, Vector3.CalculateAngle(Normal, Vector3.UnitZ));
            var quartMat = Matrix4.CreateFromQuaternion(quart);
            for (int i = 0; i < Partition; i++)
            {
                var pos = Calculator.GetSphericalPolarCoordinates(Radius, (float)Math.PI / 2, theta * i);
                pos = Calculator.Multiply(quartMat, pos);
                position.Add(pos + Center);
            }

            for (int i = 0; i < position.Count - 1; i++)
            {
                index.Add(i);
                index.Add(i + 1);
            }

            index.Add(position.Count - 1);
            index.Add(0);

            Position = position.ToArray();
            Index = index.ToArray();

        }
    }
}
