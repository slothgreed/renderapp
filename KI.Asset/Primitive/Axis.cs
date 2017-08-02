using OpenTK;
using KI.Foundation.Core;
using System.Collections.Generic;

namespace KI.Asset
{
    public class Axis : KIObject, IGeometry
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

        public Axis(string name, Vector3 min, Vector3 max)
            : base(name)
        {
            Min = min;
            Max = max;
            CreateObject();
        }

        private void CreateObject()
        {
            var position = new List<Vector3>();
            var color = new List<Vector3>();
            position.Add(new Vector3(Max.X, 0.0f, 0.0f));
            position.Add(new Vector3(Min.X, 0.0f, 0.0f));
            position.Add(new Vector3(0.0f, Max.Y, 0.0f));
            position.Add(new Vector3(0.0f, Min.Y, 0.0f));
            position.Add(new Vector3(0.0f, 0.0f, Max.Z));
            position.Add(new Vector3(0.0f, 0.0f, Min.Z));

            color.Add(new Vector3(1, 0, 0));
            color.Add(new Vector3(1, 0, 0));
            color.Add(new Vector3(0, 1, 0));
            color.Add(new Vector3(0, 1, 0));
            color.Add(new Vector3(0, 0, 1));
            color.Add(new Vector3(0, 0, 1));

            GeometryInfo info = new GeometryInfo(position, null, color, null, null, Gfx.GLUtil.GeometryType.Line);
            GeometryInfos = new GeometryInfo[] { info };
        }

        public GeometryInfo[] GeometryInfos
        {
            get;
            private set;
        }
    }
}
