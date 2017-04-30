using System;
using OpenTK;
using KI.Foundation.Core;

namespace KI.Gfx.KIAsset
{
    public class Sphere : KIObject, IPrimitive
    {
        public GeometryInfo _Geometry;
        public GeometryInfo Geometry
        {
            get
            {
                return _Geometry;
            }
        }
        /// <param name="radial">半径</param>
        /// <param name="hpartition">高さ分割数</param>
        /// <param name="wpartition">横分割数</param>
        /// <param name="orient">面の方向true=外向きfalse=内向き</param>
        public Sphere(string name, float radial, int hpartition, int wpartition, bool orient, Vector3 color)
            : base(name)
        {
            SetObjectData(radial, hpartition, wpartition, orient, color);
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
        private void SetObjectData(float radial, int hpartition, int wpartition, bool orient, Vector3 color)
        {
            float theta = (float)Math.PI / hpartition;
            float phi = (float)Math.PI / wpartition;

            phi *= 2;

            //一番上
            for (int i = 0; i < wpartition; i++)
            {
                _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, 0, 0));
                _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta, phi * i));
                _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta, phi * (i + 1)));

                _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 3]));
                _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 2]));
                _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 1]));

                _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 3].Normalized());
                _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 2].Normalized());
                _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 1].Normalized());
            }


            for (int i = 2; i < hpartition; i++)
            {
                for (int j = 0; j < wpartition; j++)
                {
                    _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * j));
                    _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
                    _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

                    _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * i, phi * (j + 1)));
                    _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * (j + 1)));
                    _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * (i - 1), phi * j));

                    _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 6]));
                    _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 5]));
                    _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 4]));
                    _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 3]));
                    _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 2]));
                    _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 1]));



                    _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 6].Normalized());
                    _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 5].Normalized());
                    _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 4].Normalized());

                    _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 3].Normalized());
                    _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 2].Normalized());
                    _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 1].Normalized());

                }
            }
            for (int i = 0; i < wpartition; i++)
            {
                _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * (hpartition - 1), phi * (i + 1)));
                _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * (hpartition - 1), phi * i));
                _Geometry.Position.Add(GetSphericalPolarCoordinates(radial, theta * hpartition, 0));

                _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 3]));
                _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 2]));
                _Geometry.TexCoord.Add(GetSphericalTexCoord(_Geometry.Position[_Geometry.Position.Count - 1]));

                _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 3].Normalized());
                _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 2].Normalized());
                _Geometry.Normal.Add(_Geometry.Position[_Geometry.Position.Count - 1].Normalized());

            }

            Vector2 max = Vector2.Zero;
            Vector2 min = Vector2.One;
            for (int i = 0; i < _Geometry.TexCoord.Count; i++)
            {
                if (max.X < _Geometry.TexCoord[i].X) { max.X = _Geometry.TexCoord[i].X; }
                if (max.Y < _Geometry.TexCoord[i].Y) { max.Y = _Geometry.TexCoord[i].Y; }

                if (min.X < _Geometry.TexCoord[i].X) { min.X = _Geometry.TexCoord[i].X; }
                if (min.Y < _Geometry.TexCoord[i].Y) { min.Y = _Geometry.TexCoord[i].Y; }
            }
            //TODO: きちんと実装
            //RetouchTexcoord(TexCoord);

            if (!orient)
            {
                Vector3 tmp3;
                Vector2 tmp2;
                for (int i = 0; i < _Geometry.Position.Count; i += 3)
                {
                    tmp3 = _Geometry.Position[i];
                    _Geometry.Position[i] = _Geometry.Position[i + 2];
                    _Geometry.Position[i + 2] = tmp3;

                    _Geometry.Normal[i] = -_Geometry.Normal[i];
                    _Geometry.Normal[i + 1] = -_Geometry.Normal[i + 1];
                    _Geometry.Normal[i + 2] = -_Geometry.Normal[i + 2];

                    tmp2 = _Geometry.TexCoord[i];
                    _Geometry.TexCoord[i] = _Geometry.TexCoord[i + 2];
                    _Geometry.TexCoord[i + 2] = tmp2;

                }
            }

            Geometry.ConvertVertexArray();

        }
    }
}
