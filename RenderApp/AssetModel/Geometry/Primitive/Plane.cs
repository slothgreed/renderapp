using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.AssetModel.RA_Geometry
{
    public class Plane : IRenderObjectConverter
    {
        #region [メンバ変数]
        /// <summary>
        /// 平面の公式
        /// </summary>
        private Vector4 surface = new Vector4();
        private List<Vector3> Position = new List<Vector3>();
        #endregion
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Plane(string name,Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
        {
            Position.Add(q0);
            Position.Add(q1);
            Position.Add(q2);
            Position.Add(q3);
        }
        public Plane(string name)
        {
            Position.Add(new Vector3(-1, -1, 0));
            Position.Add(new Vector3(1, -1, 0));
            Position.Add(new Vector3(1, 1, 0));
            Position.Add(new Vector3(-1, 1, 0));
        }
        #region [形状の作成]
        /// <summary>
        /// 任意面
        /// </summary>
        public List<RenderObject> CreateRenderObject()
        {
            List<Vector3> Normal = new List<Vector3>();
            List<Vector2> TexCoord = new List<Vector2>();
            surface = RACalc.GetPlaneFormula(Position[0], Position[1], Position[2]);

            Normal.Add(new Vector3(surface));
            Normal.Add(Normal[0]);
            Normal.Add(Normal[0]);
            Normal.Add(Normal[0]);

            TexCoord.Add(Vector2.Zero);
            TexCoord.Add(Vector2.UnitX);
            TexCoord.Add(Vector2.UnitX + Vector2.UnitY);
            TexCoord.Add(Vector2.UnitY);

            RenderObject render = new RenderObject("quad");
            render.CreatePNT(Position, Normal, TexCoord, PrimitiveType.Quads);
            _renderObject = new List<RenderObject>() { render };
            return _renderObject;
        }
        private List<RenderObject> _renderObject;
        public List<RenderObject> RenderObject
        {
            get
            {
                return _renderObject;
            }
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
            
            result = RACalc.crossPlanetoLine(near, far, surface);

            return result;
        }
        #endregion
    }
}
