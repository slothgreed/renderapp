using System.Collections.Generic;
using KI.Asset;
using KI.Gfx.GLUtil;
using OpenTK;

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
            List<Vector3> position = new List<Vector3>();
            List<Vector3> color = new List<Vector3>();

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

                    position.Add(line_start1);
                    position.Add(line_fin1);
                    position.Add(line_start2);
                    position.Add(line_fin2);
                    color.Add(Vector3.One);
                    color.Add(Vector3.One);
                    color.Add(Vector3.One);
                    color.Add(Vector3.One);
                }
            }

            var info = new Geometry(position, null, color, null, null, GeometryType.Line);
            GeometryInfos = new Geometry[] { info };
        }

        public Geometry[] GeometryInfos
        {
            get;
            private set;
        }
    }
}
