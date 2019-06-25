using KI.Mathmatics;
using OpenTK;
using System;
using System.Collections.Generic;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// 円柱
    /// </summary>
    public class Cylinder : PrimitiveBase
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
        public Cylinder(float radius, float height, int partition)
        {
            Radius = radius;
            Height = height;
            CreateModel(partition);
        }

        /// <summary>
        /// 形状の作成 XZ基準になっている。
        /// </summary>
        /// <param name="partition">分割数</param>
        private void CreateModel(int partition)
        {
            List<Vector3> position = new List<Vector3>();
            List<Vector3> normal = new List<Vector3>();
            List<int> index = new List<int>();
            float theta = (float)Math.PI * 2 / partition;

            // 円柱の上側中心
            position.Add(new Vector3(0, Height, 0));
            normal.Add(position[0].Normalized());

            var center = new Vector3(0, Height / 2, 0);
            for (int i = 0; i < partition; i++)
            {
                var pos = Calculator.GetSphericalPolarCoordinates(Radius, theta * (i + 1), 0);
                pos.Y = Height;
                position.Add(pos);

                normal.Add((pos - center).Normalized());
            }

            //円柱の上側下側の境界
            int border = position.Count;

            // 円柱の下側中心
            position.Add(new Vector3(0));
            normal.Add(-Vector3.UnitY);
            for (int i = 0; i < partition; i++)
            {
                var pos = Calculator.GetSphericalPolarCoordinates(Radius, theta * (i + 1), 0);
                position.Add(pos);
                normal.Add((pos - center).Normalized());
            }
            
            for (int i = 1; i < border - 1; i++)
            {
                index.Add(0);
                index.Add(i);
                index.Add(i + 1);
            }

            // 最後のTriangle
            index.Add(0);
            index.Add(border - 1);
            index.Add(1);

            for (int i = border + 1; i < position.Count - 1; i++)
            {
                // 底面部分
                index.Add(border);
                index.Add(i + 1);
                index.Add(i);
            }

            // 底面部分
            index.Add(border);
            index.Add(border + 1);
            index.Add(position.Count - 1);

            for (int i = 0; i < partition - 1; i++)
            {
                // 最初は中心点があるため+1から
                var top0 = i + 1;
                var top1 = i + 2;
                var top2 = border + 1 + i;
                var top3 = border + 2 + i;

                index.Add(top0);
                index.Add(top2);
                index.Add(top1);

                index.Add(top1);
                index.Add(top2);
                index.Add(top3);
            }

            index.Add(1);
            index.Add(border - 1);
            index.Add(border + 1);

            index.Add(border - 1);
            index.Add(position.Count - 1);
            index.Add(border + 1);

            Position = position.ToArray();
            Normal = normal.ToArray();
            Index = index.ToArray();
        }
    }
}
