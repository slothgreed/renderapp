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

        /// <summary>
        /// 頂点0
        /// </summary>
        private Vector3 quad0;

        /// <summary>
        /// 頂点1
        /// </summary>
        private Vector3 quad1;

        /// <summary>
        /// 頂点2
        /// </summary>
        private Vector3 quad2;

        /// <summary>
        /// 頂点3
        /// </summary>
        private Vector3 quad3;
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
            quad0 = q0;
            quad1 = q1;
            quad2 = q2;
            quad3 = q3;
            CreateGeometry();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public Plane(string name)
            : base(name)
        {
            quad0 = new Vector3(-1, -1, 0);
            quad1 = new Vector3(1, -1, 0);
            quad2 = new Vector3(1, 1, 0);
            quad3 = new Vector3(-1, 1, 0);
            CreateGeometry();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Geometry[] Geometrys { get; private set; }

        #region [形状の作成]
        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateGeometry()
        {
            var position = new List<Vector3>();
            var normal = new List<Vector3>();
            var texcoord = new List<Vector2>();
            position.Add(quad0);
            position.Add(quad1);
            position.Add(quad2);
            position.Add(quad3);

            surface = KICalc.GetPlaneFormula(quad0, quad1, quad2);

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
