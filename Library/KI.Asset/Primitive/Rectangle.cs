using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;
using KI.Asset.Primitive;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// 平面
    /// </summary>
    public class Rectangle : PrimitiveBase
    {
        #region [メンバ変数]
        /// <summary>
        /// 平面の公式
        /// </summary>
        public Vector4 Formula
        {
            get;
            private set;
        }

        #endregion
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="q0">頂点0</param>
        /// <param name="q1">頂点1</param>
        /// <param name="q2">頂点2</param>
        /// <param name="q3">頂点3</param>
        public Rectangle(Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
        {
            Vertexs = new Vertex[4];
            Vertexs[0] = new Vertex(0, q0);
            Vertexs[1] = new Vertex(1, q1);
            Vertexs[2] = new Vertex(2, q2);
            Vertexs[3] = new Vertex(3, q3);

            Index = new int[4];
            Index[0] = 0;
            Index[1] = 1;
            Index[2] = 2;
            Index[3] = 3;

            Vertexs[0].TexCoord = Vector2.Zero;
            Vertexs[1].TexCoord = Vector2.UnitX;
            Vertexs[2].TexCoord = Vector2.One;
            Vertexs[3].TexCoord = Vector2.UnitY;

            Formula = Plane.Formula(Vertexs[0].Position, Vertexs[1].Position, Vertexs[2].Position);
            Type = KIPrimitiveType.Quads;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public Rectangle()
        {
            Vertexs = new Vertex[4];
            Vertexs[0] = new Vertex(0,new Vector3(-1, -1, 0));
            Vertexs[1] = new Vertex(1,new Vector3(1, -1, 0));
            Vertexs[2] = new Vertex(2,new Vector3(1, 1, 0));
            Vertexs[3] = new Vertex(3, new Vector3(-1, 1, 0));
            Index = new int[4];
            Index[0] = 0;
            Index[1] = 1;
            Index[2] = 2;
            Index[3] = 3;


            Vertexs[0].TexCoord = Vector2.Zero;
            Vertexs[1].TexCoord = Vector2.UnitX;
            Vertexs[2].TexCoord = Vector2.One;
            Vertexs[3].TexCoord = Vector2.UnitY;

            Formula = Plane.Formula(Vertexs[0].Position, Vertexs[1].Position, Vertexs[2].Position);
            Type = KIPrimitiveType.Quads;
        }
    }
}
