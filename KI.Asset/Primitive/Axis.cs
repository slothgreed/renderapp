using System.Collections.Generic;
using KI.Foundation.Core;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 軸
    /// </summary>
    public class Axis : KIObject, IGeometry
    {
        /// <summary>
        /// 最小値
        /// </summary>
        private Vector3 min;

        /// <summary>
        /// 最大値
        /// </summary>
        private Vector3 max;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public Axis(string name, Vector3 min, Vector3 max)
            : base(name)
        {
            this.min = min;
            this.max = max;
            CreateObject();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Geometry[] Geometrys { get; private set; }

        /// <summary>
        /// 軸作成
        /// </summary>
        private void CreateObject()
        {
            var position = new List<Vector3>();
            var color = new List<Vector3>();
            position.Add(new Vector3(max.X, 0.0f, 0.0f));
            position.Add(new Vector3(min.X, 0.0f, 0.0f));
            position.Add(new Vector3(0.0f, max.Y, 0.0f));
            position.Add(new Vector3(0.0f, min.Y, 0.0f));
            position.Add(new Vector3(0.0f, 0.0f, max.Z));
            position.Add(new Vector3(0.0f, 0.0f, min.Z));

            color.Add(new Vector3(1, 0, 0));
            color.Add(new Vector3(1, 0, 0));
            color.Add(new Vector3(0, 1, 0));
            color.Add(new Vector3(0, 1, 0));
            color.Add(new Vector3(0, 0, 1));
            color.Add(new Vector3(0, 0, 1));

            Geometry info = new Geometry(position, null, color, null, null, Gfx.GLUtil.GeometryType.Line);
            Geometrys = new Geometry[] { info };
        }
    }
}
