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
    public class Circle : PrimitiveBase
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
        /// コンストラクタ
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="center">位置</param>
        /// <param name="normal">円の向き</param>
        /// <param name="partition">分割数</param>
        public Circle(float radius, Vector3 center, Vector3 normal, int partition, KIPrimitiveType type = KIPrimitiveType.Triangles)
        {
            Radius = radius;
            Center = center;
            Normal = normal;
            Partition = partition;
            Type = type;
            Create();
        }

        /// <summary>
        /// XY基準で円の作成
        /// </summary>
        private void Create()
        {
            List<Vector3> position = new List<Vector3>();
            List<int> index = new List<int>();

            float theta = (float)Math.PI * 2 / Partition;
            var ex = Vector3.Cross(Normal, Vector3.UnitZ);
            Quaternion quart = Quaternion.FromAxisAngle(ex, Vector3.CalculateAngle(Normal, Vector3.UnitZ));
            var quartMat = Matrix4.CreateFromQuaternion(quart);

            if (Type == KIPrimitiveType.Triangles)
            {
                position.Add(Center);
                for (int i = 0; i < Partition; i++)
                {
                    var pos = Calculator.GetSphericalPolarCoordinates(Radius, (float)Math.PI / 2, theta * i);
                    pos = Calculator.Multiply(quartMat, pos);
                    position.Add(pos + Center);
                }

                for (int i = 1; i < position.Count - 1; i++)
                {
                    index.Add(0);
                    index.Add(i + 1);
                    index.Add(i);
                }

                index.Add(0);
                index.Add(1);
                index.Add(position.Count - 1);

                Position = position.ToArray();
                Index = index.ToArray();
            }
            else if(Type == KIPrimitiveType.Lines)
            {
                for (int i = 0; i < Partition; i++)
                {
                    var pos = Calculator.GetSphericalPolarCoordinates(Radius, (float)Math.PI / 2, theta * i);
                    pos = Calculator.Multiply(quartMat, pos);
                    position.Add(pos + Center);
                    index.Add(i);
                    if (i == Partition - 1)
                    {
                        index.Add(0);
                    }
                    else
                    {
                        index.Add(i + 1);
                    }
                }

                Position = position.ToArray();
                Index = index.ToArray();
            }
        }
    }
}
