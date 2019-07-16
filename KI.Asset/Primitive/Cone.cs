using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;
using System;
using System.Collections.Generic;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// 円錐
    /// </summary>
    public class Cone : PrimitiveBase
    {
        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; private set; }

        /// <summary>
        /// 高さ
        /// </summary>
        public float Height { get; private set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="height">高さ</param>
        /// <param name="partition">分割数</param>
        public Cone(float radius, float height, int partition)
        {
            Radius = radius;
            Height = height;
            CreateModel(partition);
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        /// <param name="partition">分割数</param>
        private void CreateModel(int partition)
        {
            List<Vertex> vertex = new List<Vertex>();
            List<int> index = new List<int>();
            float theta = (float)Math.PI * 2 / partition;
            float right = (float)Math.PI / 2;

            //　円錐の先
            var position = Calculator.GetSphericalPolarCoordinates(Radius, right, right);
            var normal = position.Normalized();
            vertex.Add(new Vertex(0, position, normal, Vector3.Zero));

            for (int i = 0; i < partition; i++)
            {
                position = Calculator.GetSphericalPolarCoordinates(Radius, theta * i, 0);
                normal = vertex[vertex.Count - 1].Position.Normalized();
                vertex.Add(new Vertex(0, position, normal, Vector3.Zero));
            }

            // 底面の中心

            position = Vector3.Zero;
            normal = -Vector3.UnitY;
            vertex.Add(new Vertex(0, position, normal, Vector3.Zero));

            for (int i = 1; i < vertex.Count - 1; i++)
            {
                index.Add(0);
                index.Add(i);
                index.Add(i + 1);
            }

            // 最後のTriangle
            index.Add(0);
            index.Add(vertex.Count - 2);
            index.Add(1);

            for (int i = 1; i < vertex.Count - 1; i++)
            {
                // 底面部分
                index.Add(vertex.Count - 1);
                index.Add(i + 1);
                index.Add(i);
            }

            // 底面部分
            index.Add(vertex.Count - 1);
            index.Add(1);
            index.Add(vertex.Count - 2);

            Vertexs = vertex.ToArray();
            Index = index.ToArray();
        }
    }
}
