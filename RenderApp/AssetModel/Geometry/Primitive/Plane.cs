using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.AssetModel
{
    public class Plane
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
        public Plane(string name,Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
        {
            MakeInfinityPlane(q0, q1, q2, q3);
            geometry = new Primitive(name,Position,Normal,TexCoord,PrimitiveType.Quads);
        }
        public Plane(string name)
        {
            MakeInfinityPlane(new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(1, 1, 0), new Vector3(-1, 1, 0));
            geometry = new Primitive(name, Position, Normal, TexCoord, PrimitiveType.Quads);
        }
        public Geometry geometry;
        List<Vector3> Position = new List<Vector3>();
        List<Vector3> Color = new List<Vector3>();
        List<Vector3> Normal = new List<Vector3>();
        List<Vector2> TexCoord = new List<Vector2>();
        #region [形状の作成]
        /// <summary>
        /// 形状の設定
        /// </summary>
        private void SetObjectData()
        {
            //MakeXPlane();
            Color.Add(new Vector3(1, 0, 0));
            Color.Add(Color[0]);
            Color.Add(Color[0]);
            Color.Add(Color[0]);
        }
        ///// <summary>
        ///// 法線がX軸の平面
        ///// </summary>
        ///// <param name="X">X座標の位置</param>
        //public void MakeXPlane(float X = 0)
        //{
        //    ClearData();
        //    Position.Add(new Vector3(X, RenderSystem.m_WorldMin.Y, RenderSystem.m_WorldMin.Z));
        //    Position.Add(new Vector3(X, RenderSystem.m_WorldMax.Y, RenderSystem.m_WorldMin.Z));
        //    Position.Add(new Vector3(X, RenderSystem.m_WorldMax.Y, RenderSystem.m_WorldMax.Z));
        //    Position.Add(new Vector3(X, RenderSystem.m_WorldMin.Y, RenderSystem.m_WorldMax.Z));
        //    Normal.Add(new Vector3(1, 0, 0));
        //    Normal.Add(Normal[0]);
        //    Normal.Add(Normal[0]);
        //    Normal.Add(Normal[0]);
        //    CCalc.GetPlaneFormula(surface, Position[0], Position[1], Position[2]);
        //    BindBuffer();
        //}
        ///// <summary>
        ///// 法線がY軸の平面
        ///// </summary>
        //public void MakeYPlane(float Y = 0)
        //{
        //    ClearData();
        //    Position.Add(new Vector3(RenderSystem.m_WorldMin.X, Y, RenderSystem.m_WorldMin.Z));
        //    Position.Add(new Vector3(RenderSystem.m_WorldMax.X, Y, RenderSystem.m_WorldMin.Z));
        //    Position.Add(new Vector3(RenderSystem.m_WorldMax.X, Y, RenderSystem.m_WorldMax.Z));
        //    Position.Add(new Vector3(RenderSystem.m_WorldMin.X, Y, RenderSystem.m_WorldMax.Z));
        //    Normal.Add(new Vector3(0, 1, 0));
        //    Normal.Add(Normal[0]);
        //    Normal.Add(Normal[0]);
        //    Normal.Add(Normal[0]);
        //    CCalc.GetPlaneFormula(surface, Position[0], Position[1], Position[2]);
        //    BindBuffer();
        //}
        ///// <summary>
        ///// 法線がZ軸の平面
        ///// </summary>
        //public void MakeZPlane(float Z = 0)
        //{
        //    Position.Add(new Vector3(RenderSystem.m_WorldMin.X, RenderSystem.m_WorldMin.Y, Z));
        //    Position.Add(new Vector3(RenderSystem.m_WorldMax.X, RenderSystem.m_WorldMin.Y, Z));
        //    Position.Add(new Vector3(RenderSystem.m_WorldMax.X, RenderSystem.m_WorldMax.Y, Z));
        //    Position.Add(new Vector3(RenderSystem.m_WorldMin.X, RenderSystem.m_WorldMax.Y, Z));
        //    Normal.Add(new Vector3(0, 0, 1));
        //    Normal.Add(Normal[0]);
        //    Normal.Add(Normal[0]);
        //    Normal.Add(Normal[0]);
        //    CCalc.GetPlaneFormula(surface, Position[0], Position[1], Position[2]);
        //}
        /// <summary>
        /// 任意面
        /// </summary>
        public void MakeInfinityPlane(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
        {

            Position.Add(v0);
            Position.Add(v1);
            Position.Add(v2);
            Position.Add(v3);
            surface = RACalc.GetPlaneFormula(Position[0], Position[1], Position[2]);
            Normal.Add(new Vector3(surface));

            TexCoord.Add(Vector2.Zero);
            TexCoord.Add(Vector2.UnitX);
            TexCoord.Add(Vector2.UnitX + Vector2.UnitY);
            TexCoord.Add(Vector2.UnitY);
            
        }
        

        /// <summary>
        /// 任意面
        /// 頂点位置は適当とりあえずpositionが中心
        /// </summary>
        public void MakeInfinityPlane(Vector3 position, Vector3 normal)
        {
            //ClearData();
            //surface = CCalc.GetPlaneFormula(position, normal);
            //List<Vector3> plane = CCalc.GetPlanePoint(surface, position);
            //Position.Add(plane[0]);
            //Position.Add(plane[1]);
            //Position.Add(plane[2]);
            //Position.Add(plane[3]);

            //Normal.Add(normal);
            //Normal.Add(normal);
            //Normal.Add(normal);
            //Normal.Add(normal);

            //Color.Add(new Vector3(1, 0, 0));
            //Color.Add(Color[0]);
            //Color.Add(Color[0]);
            //Color.Add(Color[0]);
            //BindBuffer();
        }
        public void MakeQuad(Vector3 q0,Vector3 q1,Vector3 q2,Vector3 q3)
        {
            Vector3 normal;
            Position.Add(q0);
            Position.Add(q1);
            Position.Add(q2);
            Position.Add(q3);

            normal = Vector3.Cross(q1 - q0, q2 - q0).Normalized();
            surface = RACalc.GetPlaneFormula(q0, normal);

            Normal.Add(normal);
            Normal.Add(normal);
            Normal.Add(normal);
            Normal.Add(normal);
            Color.Add(new Vector3(1, 0, 0));
            Color.Add(Color[0]);
            Color.Add(Color[0]);
            Color.Add(Color[0]);
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
