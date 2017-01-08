using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using RenderApp.GLUtil;
namespace RenderApp.AssetModel.RA_Geometry
{
    public class Cube
    {
        public Vector3 Min
        {
            get;
            set;
        }
        public Vector3 Max
        {
            get;
            set;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Cube(string name,Vector3 min,Vector3 max)
        {
            Min = min;
            Max = max;
            //手前
        }
        public List<Geometry> ConvertGeometry()
        {
            List<Geometry> geometry = new List<Geometry>();

            Vector3 v0 = new Vector3(Min.X, Min.Y, Min.Z);
            Vector3 v1 = new Vector3(Max.X, Min.Y, Min.Z);
            Vector3 v2 = new Vector3(Max.X, Max.Y, Min.Z);
            Vector3 v3 = new Vector3(Min.X, Max.Y, Min.Z);
            Vector3 v4 = new Vector3(Min.X, Min.Y, Max.Z);
            Vector3 v5 = new Vector3(Max.X, Min.Y, Max.Z);
            Vector3 v6 = new Vector3(Max.X, Max.Y, Max.Z);
            Vector3 v7 = new Vector3(Min.X, Max.Y, Max.Z);

            List<Vector2> TexCoord = new List<Vector2>()
            {
                Vector2.Zero,
                Vector2.UnitX,
                Vector2.UnitY,
                Vector2.UnitX + Vector2.UnitY
            };
            var front = new RenderObject("Front");
            var left = new RenderObject("Left");
            var back = new RenderObject("Back");
            var right = new RenderObject("Right");
            var top = new RenderObject("Top");
            var bottom = new RenderObject("Bottom");

            front.CreateP(new List<Vector3> { v0, v3, v2, v1 }, PrimitiveType.Quads);
            front.CreateP(new List<Vector3> { v0, v4, v7, v3 }, PrimitiveType.Quads);
            front.CreateP(new List<Vector3> { v4, v5, v6, v7 }, PrimitiveType.Quads);
            front.CreateP(new List<Vector3> { v1, v2, v6, v5 }, PrimitiveType.Quads);
            front.CreateP(new List<Vector3> { v2, v3, v7, v6 }, PrimitiveType.Quads);
            front.CreateP(new List<Vector3> { v1, v5, v4, v0 }, PrimitiveType.Quads);

            geometry.Add(front);
            geometry.Add(left);
            geometry.Add(back);
            geometry.Add(right);
            geometry.Add(top);
            geometry.Add(bottom);

            return geometry;
        }

    }
}
