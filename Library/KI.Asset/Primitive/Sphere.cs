﻿using System;
using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;
using KI.Mathmatics;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// 球
    /// </summary>
    public class Sphere : PrimitiveBase
    {
        /// <summary>
        /// 半径
        /// </summary>
        private float radial;

        /// <summary>
        /// 高さ分割数
        /// </summary>
        private int hpartition;

        /// <summary>
        /// 横分割数
        /// </summary>
        private int wpartition;

        /// <summary>
        /// 面の方向true=外向きfalse=内向き
        /// </summary>
        private bool orient;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="radial">半径</param>
        /// <param name="hpartition">高さ分割数</param>
        /// <param name="wpartition">横分割数</param>
        /// <param name="orient">面の方向true=外向きfalse=内向き</param>
        public Sphere(float radial, int hpartition, int wpartition, bool orient)
        {
            this.radial = radial;
            this.hpartition = hpartition;
            this.wpartition = wpartition;
            this.orient = orient;
            CreateModel();
        }

        /// <summary>
        /// オブジェクトの設定
        /// </summary>
        public void CreateModel()
        {
            float theta = (float)Math.PI / hpartition;
            float phi = (float)Math.PI / wpartition;

            var position = new List<Vector3>();
            var normal = new List<Vector3>();
            var texcoord = new List<Vector2>();

            phi *= 2;

            //一番上
            for (int i = 0; i < wpartition; i++)
            {
                position.Add(Calculator.GetSphericalPolarCoordinates(radial, 0, 0));
                position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta, phi * i));
                position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta, phi * (i + 1)));

                texcoord.Add(GetSphericalTexCoord(position[position.Count - 3]));
                texcoord.Add(GetSphericalTexCoord(position[position.Count - 2]));
                texcoord.Add(GetSphericalTexCoord(position[position.Count - 1]));

                normal.Add(position[position.Count - 3].Normalized());
                normal.Add(position[position.Count - 2].Normalized());
                normal.Add(position[position.Count - 1].Normalized());
            }

            for (int i = 2; i < hpartition; i++)
            {
                for (int j = 0; j < wpartition; j++)
                {
                    position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * i, phi * j));
                    position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
                    position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

                    position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
                    position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * (j + 1)));
                    position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

                    texcoord.Add(GetSphericalTexCoord(position[position.Count - 6]));
                    texcoord.Add(GetSphericalTexCoord(position[position.Count - 5]));
                    texcoord.Add(GetSphericalTexCoord(position[position.Count - 4]));
                    texcoord.Add(GetSphericalTexCoord(position[position.Count - 3]));
                    texcoord.Add(GetSphericalTexCoord(position[position.Count - 2]));
                    texcoord.Add(GetSphericalTexCoord(position[position.Count - 1]));

                    normal.Add(position[position.Count - 6].Normalized());
                    normal.Add(position[position.Count - 5].Normalized());
                    normal.Add(position[position.Count - 4].Normalized());

                    normal.Add(position[position.Count - 3].Normalized());
                    normal.Add(position[position.Count - 2].Normalized());
                    normal.Add(position[position.Count - 1].Normalized());
                }
            }

            for (int i = 0; i < wpartition; i++)
            {
                position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * (hpartition - 1), phi * (i + 1)));
                position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * (hpartition - 1), phi * i));
                position.Add(Calculator.GetSphericalPolarCoordinates(radial, theta * hpartition, 0));

                texcoord.Add(GetSphericalTexCoord(position[position.Count - 3]));
                texcoord.Add(GetSphericalTexCoord(position[position.Count - 2]));
                texcoord.Add(GetSphericalTexCoord(position[position.Count - 1]));

                normal.Add(position[position.Count - 3].Normalized());
                normal.Add(position[position.Count - 2].Normalized());
                normal.Add(position[position.Count - 1].Normalized());
            }

            Vector2 max = Vector2.Zero;
            Vector2 min = Vector2.One;
            for (int i = 0; i < texcoord.Count; i++)
            {
                if (max.X < texcoord[i].X) { max.X = texcoord[i].X; }
                if (max.Y < texcoord[i].Y) { max.Y = texcoord[i].Y; }

                if (min.X < texcoord[i].X) { min.X = texcoord[i].X; }
                if (min.Y < texcoord[i].Y) { min.Y = texcoord[i].Y; }
            }

            if (!orient)
            {
                Vector3 tmp3;
                Vector2 tmp2;
                for (int i = 0; i < position.Count; i += 3)
                {
                    tmp3 = position[i];
                    position[i] = position[i + 2];
                    position[i + 2] = tmp3;

                    normal[i] = -normal[i];
                    normal[i + 1] = -normal[i + 1];
                    normal[i + 2] = -normal[i + 2];

                    tmp2 = texcoord[i];
                    texcoord[i] = texcoord[i + 2];
                    texcoord[i + 2] = tmp2;
                }
            }

            Vertexs = new Vertex[position.Count];
            Index = new int[position.Count];
            Type = KIPrimitiveType.Triangles;
            for (int i = 0; i < position.Count / 3; i++)
            {
                Vertexs[3 * i + 0] = new Vertex(3 * i, position[3 * i], normal[3 * i], texcoord[3 * i]);
                Vertexs[3 * i + 1] = new Vertex(3 * i + 1, position[3 * i + 1], normal[3 * i + 1], texcoord[3 * i + 1]);
                Vertexs[3 * i + 2] = new Vertex(3 * i + 2, position[3 * i + 2], normal[3 * i + 2], texcoord[3 * i + 2]);
                Index[3 * i + 0] = 3 * i + 0;
                Index[3 * i + 1] = 3 * i + 1;
                Index[3 * i + 2] = 3 * i + 2;
            }
        }

        /// <summary>
        /// テクスチャ座標の取得
        /// </summary>
        /// <param name="position">位置</param>
        /// <returns>テクスチャ座標</returns>
        private Vector2 GetSphericalTexCoord(Vector3 position)
        {
            Vector2 texcoord = Vector2.Zero;
            float pi = (float)Math.PI;
            position.Normalize();

            float atan2 = (float)Math.Atan2(position.Z, position.X);
            float asin = (float)Math.Asin(position.Y);

            texcoord.X = 0.5f;
            texcoord.X += atan2 / (2 * pi);
            texcoord.Y = 0.5f;
            texcoord.Y -= asin / pi;

            return texcoord;
        }
    }
}
