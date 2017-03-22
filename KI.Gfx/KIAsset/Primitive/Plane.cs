using OpenTK;
using KI.Foundation.Utility;
using KI.Foundation.Core;

namespace KI.Gfx.KIAsset
{
    public class Plane : KIObject, IPrimitive
    {
        #region [メンバ変数]
        /// <summary>
        /// 平面の公式
        /// </summary>
        private Vector4 surface = new Vector4();

        private GeometryInfo _Geometry = new GeometryInfo();
        public GeometryInfo Geometry
        {
            get
            {
                return _Geometry;
            }
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
            _Geometry.Position.Add(q0);
            _Geometry.Position.Add(q1);
            _Geometry.Position.Add(q2);
            _Geometry.Position.Add(q3);

            surface = KICalc.GetPlaneFormula(q0, q1, q2);

            _Geometry.Normal.Add(new Vector3(surface));
            _Geometry.Normal.Add(_Geometry.Normal[0]);
            _Geometry.Normal.Add(_Geometry.Normal[0]);
            _Geometry.Normal.Add(_Geometry.Normal[0]);

            _Geometry.TexCoord.Add(Vector2.Zero);
            _Geometry.TexCoord.Add(Vector2.UnitX);
            _Geometry.TexCoord.Add(Vector2.UnitX + Vector2.UnitY);
            _Geometry.TexCoord.Add(Vector2.UnitY);

        }
        #endregion

        #region [線分との交点検出]
        /// <summary>
        /// 平面と線分の交点
        /// </summary>
        /// <param name="near">始点</param>
        /// <param name="far">終点</param>
        public Vector3 CrossPoint(Vector3 near,Vector3 far)
        {
            Vector3 line = far - near;
            Vector3 result = new Vector3();
            
            result = KICalc.crossPlanetoLine(near, far, surface);

            return result;
        }
        #endregion
    }
}
