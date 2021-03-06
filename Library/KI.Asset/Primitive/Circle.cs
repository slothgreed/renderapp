﻿using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
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
        public Vector3 Direction { get; private set; }

        /// <summary>
        /// 分割方向
        /// </summary>
        public int Partition { get; private set; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="center">位置</param>
        /// <param name="direction">円の向き</param>
        /// <param name="partition">分割数</param>
        public Circle(float radius, Vector3 center, Vector3 direction, int partition, KIPrimitiveType type = KIPrimitiveType.Triangles)
        {
            Radius = radius;
            Center = center;
            Direction = direction;
            Partition = partition;
            Type = type;
            Create();
        }

        /// <summary>
        /// XY基準で円の作成
        /// </summary>
        private void Create()
        {
            List<Vertex> vertex = new List<Vertex>();
            List<int> index = new List<int>();

            float theta = (float)Math.PI * 2 / Partition;
            var ex = Vector3.Cross(Direction, Vector3.UnitZ);
            Quaternion quart = Quaternion.FromAxisAngle(ex, Vector3.CalculateAngle(Direction, Vector3.UnitZ));
            var quartMat = Matrix4.CreateFromQuaternion(quart);

            if (Type == KIPrimitiveType.Triangles)
            {
                vertex.Add(new Vertex(0, Center));
                for (int i = 0; i < Partition; i++)
                {
                    var pos = Calculator.GetSphericalPolarCoordinates(Radius, (float)Math.PI / 2, theta * i);
                    pos = Calculator.Multiply(quartMat, pos);
                    vertex.Add(new Vertex(i + 1, pos + Center));
                }

                for (int i = 1; i < vertex.Count - 1; i++)
                {
                    index.Add(0);
                    index.Add(i + 1);
                    index.Add(i);
                }

                index.Add(0);
                index.Add(1);
                index.Add(vertex.Count - 1);

                Vertexs = vertex.ToArray();
                Index = index.ToArray();
            }
            else if(Type == KIPrimitiveType.Lines)
            {
                for (int i = 0; i < Partition; i++)
                {
                    var pos = Calculator.GetSphericalPolarCoordinates(Radius, (float)Math.PI / 2, theta * i);
                    pos = Calculator.Multiply(quartMat, pos);
                    vertex.Add(new Vertex(i + 1, pos + Center));
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

                Vertexs = vertex.ToArray();
                Index = index.ToArray();
            }
        }
    }
}
