using System.Collections.Generic;
using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using KI.Renderer;
using OpenTK;

namespace RenderApp.AssetModel.RA_Geometry
{
    public class Cube
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Cube(string name, Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public Vector3 Min
        {
            get;
            private set;
        }

        public Vector3 Max
        {
            get;
            private set;
        }

        public List<RenderObject> ConvertGeometrys(bool reverse = false)
        {
            List<RenderObject> geometry = new List<RenderObject>();

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
                Vector2.UnitY,
                Vector2.UnitX + Vector2.UnitY,
                Vector2.UnitX
            };
            var front = RenderObjectFactory.Instance.CreateRenderObject("Front");
            var left = RenderObjectFactory.Instance.CreateRenderObject("Left");
            var back = RenderObjectFactory.Instance.CreateRenderObject("Back");
            var right = RenderObjectFactory.Instance.CreateRenderObject("Right");
            var top = RenderObjectFactory.Instance.CreateRenderObject("Top");
            var bot = RenderObjectFactory.Instance.CreateRenderObject("Bottom");

            if (reverse == false)
            {
                front.SetGeometryInfo(new Geometry(new List<Vector3> { v0, v3, v2, v1 }, null, null, TexCoord, null, GeometryType.Quad));
                left.SetGeometryInfo(new Geometry(new List<Vector3> { v0, v4, v7, v3 }, null, null, TexCoord, null, GeometryType.Quad));
                back.SetGeometryInfo(new Geometry(new List<Vector3> { v4, v5, v6, v7 }, null, null, TexCoord, null, GeometryType.Quad));
                right.SetGeometryInfo(new Geometry(new List<Vector3> { v1, v2, v6, v5 }, null, null, TexCoord, null, GeometryType.Quad));
                top.SetGeometryInfo(new Geometry(new List<Vector3> { v2, v3, v7, v6 }, null, null, TexCoord, null, GeometryType.Quad));
                bot.SetGeometryInfo(new Geometry(new List<Vector3> { v1, v5, v4, v0 }, null, null, TexCoord, null, GeometryType.Quad));
            }
            else
            {
                front.SetGeometryInfo(new Geometry(new List<Vector3> { v3, v0, v1, v2 }, null, null, TexCoord, null, GeometryType.Quad));
                left.SetGeometryInfo(new Geometry(new List<Vector3> { v7, v4, v0, v3 }, null, null, TexCoord, null, GeometryType.Quad));
                back.SetGeometryInfo(new Geometry(new List<Vector3> { v6, v5, v4, v7 }, null, null, TexCoord, null, GeometryType.Quad));
                right.SetGeometryInfo(new Geometry(new List<Vector3> { v2, v1, v5, v6 }, null, null, TexCoord, null, GeometryType.Quad));
                top.SetGeometryInfo(new Geometry(new List<Vector3> { v2, v6, v7, v3 }, null, null, TexCoord, null, GeometryType.Quad));
                bot.SetGeometryInfo(new Geometry(new List<Vector3> { v1, v0, v4, v5 }, null, null, TexCoord, null, GeometryType.Quad));
            }

            geometry.Add(right);    ///< posx
            geometry.Add(top);      ///< posy
            geometry.Add(back);     ///< posz
            geometry.Add(left);    ///< negx
            geometry.Add(bot);      ///< negy
            geometry.Add(front);    ///< negz

            string vert = ShaderCreater.Instance.GetVertexShader(front);
            string frag = ShaderCreater.Instance.GetFragShader(front);
            Shader shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);

            foreach (var geom in geometry)
            {
                geom.Shader = shader;
            }

            return geometry;
        }
    }
}
