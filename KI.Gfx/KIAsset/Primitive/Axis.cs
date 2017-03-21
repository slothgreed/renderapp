using System.Collections.Generic;
using KI.Gfx.KIAsset;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Foundation.Core;
namespace RenderApp.AssetModel.RA_Geometry
{
    class Axis : KIObject, IPrimitive
    {
        public Vector3 Min
        {
            get;
            set;
        }
        public Vector3 Max
        {
            get;
            set;
        }
        public Axis(string name,Vector3 min, Vector3 max)
            : base(name)
        {
            Min = min;
            Max = max;
            _Geometry = new GeometryInfo();
            CreateObject();
        }

        private void CreateObject()
        {
            _Geometry.Position.Add(new Vector3(Max.X, 0.0f, 0.0f));
            _Geometry.Position.Add(new Vector3(Min.X, 0.0f, 0.0f));
            _Geometry.Position.Add(new Vector3(0.0f, Max.Y, 0.0f));
            _Geometry.Position.Add(new Vector3(0.0f, Min.Y, 0.0f));
            _Geometry.Position.Add(new Vector3(0.0f, 0.0f, Max.Z));
            _Geometry.Position.Add(new Vector3(0.0f, 0.0f, Min.Z));

            _Geometry.Color.Add(new Vector3(1, 0, 0));
            _Geometry.Color.Add(new Vector3(1, 0, 0));
            _Geometry.Color.Add(new Vector3(0, 1, 0));
            _Geometry.Color.Add(new Vector3(0, 1, 0));
            _Geometry.Color.Add(new Vector3(0, 0, 1));
            _Geometry.Color.Add(new Vector3(0, 0, 1));
        }

        public GeometryInfo _Geometry;
        public GeometryInfo Geometry
        {
            get
            {
                return _Geometry;
            }
        }

    }
}
