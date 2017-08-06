using System.Collections.Generic;
using KI.Asset;
using KI.Gfx.GLUtil;
using OpenTK;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// グリッド付き平面
    /// </summary>
    class GridPlane : IGeometry
    {
        /// <summary>
        /// グリッドの範囲
        /// </summary>
        private float area;

        /// <summary>
        /// グリッドの幅
        /// </summary>
        private float delta;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="area">大きさ</param>
        /// <param name="space">間隔</param>
        public GridPlane(float area, float space)
        {
            this.area = area;
            this.delta = space;
            CreateGeometry();
        }

        /// <summary>
        /// 形状
        /// </summary>
        public Geometry[] Geometrys { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateGeometry()
        {
            List<Vector3> position = new List<Vector3>();
            List<Vector3> color = new List<Vector3>();

            Vector3 line_start1 = new Vector3();
            Vector3 line_fin1 = new Vector3();
            Vector3 line_start2 = new Vector3();
            Vector3 line_fin2 = new Vector3();
            float world = area;

            for (float i = -world; i < world; i += delta)
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

            var info = new Geometry("GridPlane", position, null, color, null, null, GeometryType.Line);
            Geometrys = new Geometry[] { info };
        }
    }
}
