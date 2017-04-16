using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIShader;
using RenderApp.Utility;
using RenderApp.GfxUtility;
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
            var front = AssetFactory.Instance.CreateRenderObject("Front");
            var left = AssetFactory.Instance.CreateRenderObject("Left");
            var back = AssetFactory.Instance.CreateRenderObject("Back");
            var right = AssetFactory.Instance.CreateRenderObject("Right");
            var top = AssetFactory.Instance.CreateRenderObject("Top");
            var bot = AssetFactory.Instance.CreateRenderObject("Bottom");

            if(reverse == false)
            {
                front.CreatePT( new List<Vector3> { v0, v3, v2, v1 }, TexCoord, PrimitiveType.Quads);
                left.CreatePT(  new List<Vector3> { v0, v4, v7, v3 }, TexCoord, PrimitiveType.Quads);
                back.CreatePT(  new List<Vector3> { v4, v5, v6, v7 }, TexCoord, PrimitiveType.Quads);
                right.CreatePT( new List<Vector3> { v1, v2, v6, v5 }, TexCoord, PrimitiveType.Quads);
                top.CreatePT(   new List<Vector3> { v2, v3, v7, v6 }, TexCoord, PrimitiveType.Quads);
                bot.CreatePT(   new List<Vector3> { v1, v5, v4, v0 }, TexCoord, PrimitiveType.Quads);
            }
            else
            {
                front.CreatePT( new List<Vector3> { v3, v0, v1, v2 }, TexCoord, PrimitiveType.Quads);
                left.CreatePT(  new List<Vector3> { v7, v4, v0, v3 }, TexCoord, PrimitiveType.Quads);
                back.CreatePT(  new List<Vector3> { v6, v5, v4, v7 }, TexCoord, PrimitiveType.Quads);
                right.CreatePT( new List<Vector3> { v2, v1, v5, v6 }, TexCoord, PrimitiveType.Quads);
                top.CreatePT(   new List<Vector3> { v2, v6, v7, v3 }, TexCoord, PrimitiveType.Quads);
                bot.CreatePT(   new List<Vector3> { v1, v0, v4, v5 }, TexCoord, PrimitiveType.Quads);
            }

            geometry.Add(right);    ///< posx
            geometry.Add(top);      ///< posy
            geometry.Add(back);     ///< posz
            geometry.Add(left);    ///< negx
            geometry.Add(bot);      ///< negy
            geometry.Add(front);      ///< negz

            string vert = ShaderCreater.Instance.GetVertexShader(front);
            string frag = ShaderCreater.Instance.GetFragShader(front);
            Shader shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);

            foreach(var geom in geometry)
            {
                geom.Shader = shader;
            }

            return geometry;
        }

    }
}
