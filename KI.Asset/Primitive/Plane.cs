﻿using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset
{
    /// <summary>
    /// 平面
    /// </summary>
    public class Plane : KIObject, IPolygon
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
            CreatePolygon();
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
            CreatePolygon();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Polygon[] Polygons { get; private set; }

        #region [形状の作成]
        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreatePolygon()
        {
            surface = KICalc.GetPlaneFormula(quad0, quad1, quad2);

            Mesh mesh = new Mesh(
                new Vertex(quad0, surface.Xyz, Vector2.Zero),
                new Vertex(quad1, surface.Xyz, Vector2.UnitX),
                new Vertex(quad2, surface.Xyz, Vector2.One),
                new Vertex(quad3, surface.Xyz, Vector2.UnitY));

            var polygon = new Polygon(this.Name, new List<Mesh>() { mesh }, PrimitiveType.Quads);

            Polygons = new Polygon[] { polygon };
        }
        #endregion
    }
}
