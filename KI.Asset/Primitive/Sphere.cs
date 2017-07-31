using System;
using OpenTK;
using KI.Foundation.Core;
using System.Collections.Generic;

namespace KI.Asset
{
    public class Sphere : KIObject, IGeometry
    {
        public GeometryInfo[] GeometryInfos
        {
            get;
            private set;
        }
        /// <param name="radial">半径</param>
        /// <param name="hpartition">高さ分割数</param>
        /// <param name="wpartition">横分割数</param>
        /// <param name="orient">面の方向true=外向きfalse=内向き</param>
        public Sphere(string name, float radial, int hpartition, int wpartition, bool orient)
            : base(name)
        {
            SetObjectData(radial, hpartition, wpartition, orient);
        }
        /// <summary>
        /// 球面座標の取得
        /// </summary>
        private Vector3 GetSphericalPolarCoordinates(float radial, float theta, float phi)
        {
            Vector3 pos = new Vector3();
            pos.X = (float)(radial * Math.Sin(theta) * Math.Cos(phi));
            pos.Y = (float)(radial * Math.Sin(theta) * Math.Sin(phi));
            pos.Z = (float)(radial * Math.Cos(theta));
            return pos;
        }

        private Vector2 GetSphericalTexCoord(Vector3 position)
        {
            Vector2 texcoord = Vector2.Zero;
            float PI = (float)Math.PI;
            position.Normalize();

            float atan2 = (float)Math.Atan2(position.Z, position.X);
            float asin = (float)Math.Asin(position.Y);

            texcoord.X = 0.5f;
            texcoord.X += atan2 / (2 * PI);
            texcoord.Y = 0.5f;
            texcoord.Y -= asin / PI;

            return texcoord;
        }
        private void SetObjectData(float radial, int hpartition, int wpartition, bool orient)
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
                position.Add(GetSphericalPolarCoordinates(radial, 0, 0));
                position.Add(GetSphericalPolarCoordinates(radial, theta, phi * i));
                position.Add(GetSphericalPolarCoordinates(radial, theta, phi * (i + 1)));

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
                    position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * j));
                    position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
                    position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

                    position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
                    position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * (j + 1)));
                    position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

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
                position.Add(GetSphericalPolarCoordinates(radial, theta * (hpartition - 1), phi * (i + 1)));
                position.Add(GetSphericalPolarCoordinates(radial, theta * (hpartition - 1), phi * i));
                position.Add(GetSphericalPolarCoordinates(radial, theta * hpartition, 0));

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
            //TODO: きちんと実装
            //RetouchTexcoord(TexCoord);

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

            var info = new GeometryInfo(position, normal, null, texcoord, null, Gfx.GLUtil.GeometryType.Triangle);

            info.ConvertVertexArray();

            GeometryInfos = new GeometryInfo[] { info };

        }
    }
}
