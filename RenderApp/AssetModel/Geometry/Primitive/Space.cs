using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.AssetModel
{
    public class Space : Geometry
    {
        
        public Space(string name,Vector3 min, Vector3 max)
            :base(name)
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

            List<Vector2> TexCoord = new List<Vector2>()
            {
                Vector2.Zero,
                Vector2.UnitX,
                Vector2.UnitY,
                Vector2.UnitX + Vector2.UnitY
            };
            Primitive front = new Primitive("Front",new List<Vector3> { v1, v2, v3, v0 }, TexCoord, PrimitiveType.Quads);
            Primitive left = new Primitive("Left",new List<Vector3> { v3, v7, v4, v0 }, TexCoord, PrimitiveType.Quads);
            Primitive back = new Primitive("Back",new List<Vector3> { v7, v6, v5, v4 }, TexCoord, PrimitiveType.Quads);
            Primitive right = new Primitive("Right",new List<Vector3> { v5, v6, v2, v1 }, TexCoord, PrimitiveType.Quads);
            Primitive top = new Primitive("Top", new List<Vector3> { v6, v7, v3, v2 }, TexCoord, PrimitiveType.Quads);
            Primitive bottom = new Primitive("Bottom",new List<Vector3> { v0, v4, v5, v1 }, TexCoord, PrimitiveType.Quads);
        }
     
    }
}
