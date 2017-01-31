using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIGeometry;
namespace RenderApp.AssetModel.RA_Geometry
{
    public class Sphere : VertexInfo
    {
        public Geometry geometry;
        /// <param name="radial">半径</param>
        /// <param name="hpartition">高さ分割数</param>
        /// <param name="wpartition">横分割数</param>
        /// <param name="orient">面の方向true=外向きfalse=内向き</param>
        public Sphere(string name,float radial, int hpartition, int wpartition,bool orient,Vector3 color)
        {
            SetObjectData(radial, hpartition, wpartition,orient,color);
        }
        /// <summary>
        /// 球面座標の取得
        /// </summary>
        private Vector3 GetSphericalPolarCoordinates(float radial,float theta,float phi)
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
        private void SetObjectData(float radial, int hpartition, int wpartition, bool orient, Vector3 color)
        {
            float theta = (float)Math.PI / hpartition;
            float phi = (float)Math.PI / wpartition;

            phi *= 2;

            //for (int i = 5; i < hpartition - 4; i++)
            //{
            //    for (int j = 0; j < wpartition; j++)
            //    {
            //        if (0 <= j && j <= 6)
            //            continue;
            //        if (8 <= j)
            //            continue;

            //        Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * j));
            //        Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
            //        Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

            //        Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
            //        Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * (j + 1)));
            //        Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

            //        TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 6]));
            //        TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 5]));
            //        TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 4]));
            //        TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 3]));
            //        TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 2]));
            //        TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 1]));



            //        Normal.Add(Position[Position.Count - 6].Normalized());
            //        Normal.Add(Position[Position.Count - 5].Normalized());
            //        Normal.Add(Position[Position.Count - 4].Normalized());

            //        Normal.Add(Position[Position.Count - 3].Normalized());
            //        Normal.Add(Position[Position.Count - 2].Normalized());
            //        Normal.Add(Position[Position.Count - 1].Normalized());

            //    }
            //}

            //一番上
            for (int i = 0; i < wpartition; i++)
            {
                Position.Add(GetSphericalPolarCoordinates(radial, 0, 0));
                Position.Add(GetSphericalPolarCoordinates(radial, theta, phi * i));
                Position.Add(GetSphericalPolarCoordinates(radial, theta, phi * (i + 1)));

                TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 3]));
                TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 2]));
                TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 1]));

                Normal.Add(Position[Position.Count - 3].Normalized());
                Normal.Add(Position[Position.Count - 2].Normalized());
                Normal.Add(Position[Position.Count - 1].Normalized());
            }


            for (int i = 2; i < hpartition; i++)
            {
                for (int j = 0; j < wpartition; j++)
                {
                    Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * j));
                    Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
                    Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

                    Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
                    Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * (j + 1)));
                    Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

                    TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 6]));
                    TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 5]));
                    TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 4]));
                    TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 3]));
                    TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 2]));
                    TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 1]));



                    Normal.Add(Position[Position.Count - 6].Normalized());
                    Normal.Add(Position[Position.Count - 5].Normalized());
                    Normal.Add(Position[Position.Count - 4].Normalized());

                    Normal.Add(Position[Position.Count - 3].Normalized());
                    Normal.Add(Position[Position.Count - 2].Normalized());
                    Normal.Add(Position[Position.Count - 1].Normalized());

                }
            }
            for (int i = 0; i < wpartition; i++)
            {
                Position.Add(GetSphericalPolarCoordinates(radial, theta * (hpartition - 1), phi * (i + 1)));
                Position.Add(GetSphericalPolarCoordinates(radial, theta * (hpartition - 1), phi * i));
                Position.Add(GetSphericalPolarCoordinates(radial, theta * hpartition, 0));

                TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 3]));
                TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 2]));
                TexCoord.Add(GetSphericalTexCoord(Position[Position.Count - 1]));

                Normal.Add(Position[Position.Count - 3].Normalized());
                Normal.Add(Position[Position.Count - 2].Normalized());
                Normal.Add(Position[Position.Count - 1].Normalized());

            }

            Vector2 max = Vector2.Zero;
            Vector2 min = Vector2.One;
            for(int i = 0; i <TexCoord.Count; i++)
            {
                if (max.X < TexCoord[i].X) { max.X = TexCoord[i].X; }
                if (max.Y < TexCoord[i].Y) { max.Y = TexCoord[i].Y; }

                if (min.X < TexCoord[i].X) { min.X = TexCoord[i].X; }
                if (min.Y < TexCoord[i].Y) { min.Y = TexCoord[i].Y; }
            }
            //TODO: きちんと実装
            //RetouchTexcoord(TexCoord);

            if (!orient)
            {
                Vector3 tmp3;
                Vector2 tmp2;
                for (int i = 0; i < Position.Count; i += 3)
                {
                    tmp3 = Position[i];
                    Position[i] = Position[i + 2];
                    Position[i + 2] = tmp3;

                    Normal[i] = -Normal[i];
                    Normal[i + 1] = -Normal[i + 1];
                    Normal[i + 2] = -Normal[i + 2];

                    tmp2 = TexCoord[i];
                    TexCoord[i] = TexCoord[i + 2];
                    TexCoord[i + 2] = tmp2;

                }
            }

            RenderObject geometry = new RenderObject("Sphere");
            geometry.CreatePNT(Position, Normal, TexCoord, PrimitiveType.Triangles);
            geometry.ConvertVertexArray();
            
        }
        /// <summary>
        /// 仮実装
        /// </summary>
        /// <param name="TexCoord"></param>
        private void RetouchTexcoord(List<Vector2> TexCoord)
        {
            float u1, u2, u3;
            for(int i = 0; i < TexCoord.Count; i+=3)
            {
                u1 = TexCoord[i].X;
                u2 = TexCoord[i + 1].X;
                u3 = TexCoord[i + 2].X;
                if (u1 < 0) { u1 = -u1; }
                if (u2 < 0) { u2 = -u2; }
                if (u3 < 0) { u3 = -u3; }

                if (u2 < 0.1f && u3 < 0.1)
                {
                    if (0.9f < u1)
                    {
                        TexCoord[i + 1] = new Vector2(1.0f, TexCoord[i + 1].Y);
                        TexCoord[i + 2] = new Vector2(1.0f, TexCoord[i + 2].Y);
                        continue;
                    }
                }
                if (u1 < 0.1f && u3 < 0.1)
                {
                    if (0.9f < u2)
                    {
                        TexCoord[i] = new Vector2(1.0f, TexCoord[i].Y);
                        TexCoord[i + 2] = new Vector2(1.0f, TexCoord[i + 2].Y);
                        continue;
                    }
                }
                if (u1 < 0.1f && u2 < 0.1)
                {
                    if (0.9f < u3)
                    {
                        TexCoord[i + 1] = new Vector2(1.0f, TexCoord[i + 1].Y);
                        TexCoord[i] = new Vector2(1.0f, TexCoord[i].Y);
                        continue;
                    }
                }

                if (u2 > 0.9f || u3 > 0.9f)
                {
                    if(u1 < 0.1)
                    {
                        TexCoord[i] = new Vector2(1.0f, TexCoord[i].Y);
                        continue;

                    }
                }
                if (u1 > 0.9f || u3 > 0.9f)
                {
                    if (u2 < 0.1)
                    {
                        TexCoord[i + 1] = new Vector2(0.0f, TexCoord[i + 1].Y);
                        continue;
                    }
                }

                if (u1 > 0.9f || u2 > 0.9f)
                {
                    if (u3 < 0.1)
                    {
                        TexCoord[i + 2] = new Vector2(0.0f, TexCoord[i + 2].Y);
                        continue;
                    }
                }

            }
        }
    }
}
