using System.Collections.Generic;
using OpenTK;
using KI.Asset;
using KI.Gfx.GLUtil;

namespace RenderApp.AssetModel
{
    class GridPlane : IGeometry
    {
        /// <summary>
        /// グリッドの範囲
        /// </summary>
        private float area;

        /// <summary>
        /// グリッドの幅
        /// </summary>
        private float space;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GridPlane(float area, float space)
        {
            this.area = area;
            this.space = space;
            Create();
        }

        public void Create()
        {
            List<Vector3> Position = new List<Vector3>();
            List<Vector3> Color = new List<Vector3>();

            Vector3 line_start1 = new Vector3();
            Vector3 line_fin1 = new Vector3();
            Vector3 line_start2 = new Vector3();
            Vector3 line_fin2 = new Vector3();
            float world = area;

            for (float i = -world; i < world; i += space)
            {
                if (i != 0)
                {
                    line_start1 = new Vector3(-world, 0, i);
                    line_fin1 = new Vector3(world, 0, i);

                    line_start2 = new Vector3(i, 0, -world);
                    line_fin2 = new Vector3(i, 0, world);

                    Position.Add(line_start1);
                    Position.Add(line_fin1);
                    Position.Add(line_start2);
                    Position.Add(line_fin2);
                    Color.Add(Vector3.One);
                    Color.Add(Vector3.One);
                    Color.Add(Vector3.One);
                    Color.Add(Vector3.One);
                }
            }

            var info = new GeometryInfo(Position, null, Color, null, null, GeometryType.Line);
            GeometryInfos = new GeometryInfo[] { info };
        }

        public GeometryInfo[] GeometryInfos
        {
            get;
            private set;
        }
    }
}
