using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 平面
    /// </summary>
    public class Plane : KIObject, IGeometry
    {
        #region [メンバ変数]
        /// <summary>
        /// 平面の公式
        /// </summary>
        private Vector4 surface = new Vector4();

        #endregion
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="q0">頂点0</param>
        /// <param name="q1">頂点1</param>
        /// <param name="q2">頂点2</param>
        /// <param name="q3">頂点3</param>
        public Plane(string name, Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
            : base(name)
        {
            CreateObject(q0, q1, q2, q3);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public Plane(string name)
            : base(name)
        {
            Vector3 q0 = new Vector3(-1, -1, 0);
            Vector3 q1 = new Vector3(1, -1, 0);
            Vector3 q2 = new Vector3(1, 1, 0);
            Vector3 q3 = new Vector3(-1, 1, 0);
            CreateObject(q0, q1, q2, q3);
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Geometry[] Geometrys { get; private set; }

        #region [形状の作成]
        /// <summary>
        /// 任意面
        /// </summary>
        /// <param name="q0">頂点0</param>
        /// <param name="q1">頂点1</param>
        /// <param name="q2">頂点2</param>
        /// <param name="q3">頂点3</param>
        private void CreateObject(Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
        {
            var position = new List<Vector3>();
            var normal = new List<Vector3>();
            var texcoord = new List<Vector2>();
            position.Add(q0);
            position.Add(q1);
            position.Add(q2);
            position.Add(q3);

            surface = KICalc.GetPlaneFormula(q0, q1, q2);

            normal.Add(new Vector3(surface));
            normal.Add(normal[0]);
            normal.Add(normal[0]);
            normal.Add(normal[0]);

            texcoord.Add(Vector2.Zero);
            texcoord.Add(Vector2.UnitX);
            texcoord.Add(Vector2.UnitX + Vector2.UnitY);
            texcoord.Add(Vector2.UnitY);

            var info = new Geometry(position, normal, Vector3.UnitX, texcoord, null, Gfx.GLUtil.GeometryType.Quad);

            Geometrys = new Geometry[] { info };
        }
        #endregion
    }
}
