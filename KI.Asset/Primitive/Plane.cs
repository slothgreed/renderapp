using OpenTK;
using KI.Foundation.Utility;
using KI.Foundation.Core;
using System.Collections.Generic;

namespace KI.Asset
{
    public class Plane : KIObject, IGeometry
    {
        #region [メンバ変数]
        /// <summary>
        /// 平面の公式
        /// </summary>
        private Vector4 surface = new Vector4();

        public GeometryInfo[] GeometryInfos
        {
            get;
            private set;
        }

        #endregion
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Plane(string name, Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
            : base(name)
        {
            CreateObject(q0, q1, q2, q3);
        }

        public Plane(string name)
            : base(name)
        {
            Vector3 q0 = new Vector3(-1, -1, 0);
            Vector3 q1 = new Vector3(1, -1, 0);
            Vector3 q2 = new Vector3(1, 1, 0);
            Vector3 q3 = new Vector3(-1, 1, 0);
            CreateObject(q0, q1, q2, q3);

        }
        #region [形状の作成]
        /// <summary>
        /// 任意面
        /// </summary>
        public void CreateObject(Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
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

            var info = new GeometryInfo(position, normal, Vector3.UnitX, texcoord, null, Gfx.GLUtil.GeometryType.Quad);

            GeometryInfos = new GeometryInfo[] { info };

        }
        #endregion

        #region [線分との交点検出]
        /// <summary>
        /// 平面と線分の交点
        /// </summary>
        /// <param name="near">始点</param>
        /// <param name="far">終点</param>
        public Vector3 CrossPoint(Vector3 near, Vector3 far)
        {
            Vector3 line = far - near;
            Vector3 result = new Vector3();

            result = KICalc.crossPlanetoLine(near, far, surface);

            return result;
        }
        #endregion
    }
}
