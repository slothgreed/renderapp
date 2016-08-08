using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using RenderApp.GLUtil;
namespace RenderApp.AssetModel
{
    public class Cube : Geometry
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Cube(string name,Vector3 min,Vector3 max)
            : base(name)
        {
            Min = min;
            Max = max;
            Vector3 v0 = new Vector3(min.X, min.Y, min.Z);
            Vector3 v1 = new Vector3(max.X, min.Y, min.Z);
            Vector3 v2 = new Vector3(max.X, max.Y, min.Z);
            Vector3 v3 = new Vector3(min.X, max.Y, min.Z);

            Vector3 v4 = new Vector3(min.X, min.Y, max.Z);
            Vector3 v5 = new Vector3(max.X, min.Y, max.Z);
            Vector3 v6 = new Vector3(max.X, max.Y, max.Z);
            Vector3 v7 = new Vector3(min.X, max.Y, max.Z);

            //手前
            var front = new Primitive("Front",new List<Vector3> { v0, v3, v2, v1 }, PrimitiveType.Quads);
            var left = new Primitive("Left",new List<Vector3> { v0, v4, v7, v3 }, PrimitiveType.Quads);
            var back = new Primitive("Back",new List<Vector3> { v4, v5, v6, v7 }, PrimitiveType.Quads);
            var right = new Primitive("Right",new List<Vector3> { v1, v2, v6, v5 }, PrimitiveType.Quads);
            var top = new Primitive("Top",new List<Vector3> { v2, v3, v7, v6 }, PrimitiveType.Quads);
            var bottom = new Primitive("Bottom", new List<Vector3> { v1, v5, v4, v0 }, PrimitiveType.Quads);
        }
     
    }
}
