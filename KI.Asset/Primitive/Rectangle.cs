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
    public class Rectangle : PrimitiveBase, ICreateModel
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
            Position = new Vector3[4];
            Position[0] = q0;
            Position[1] = q1;
            Position[2] = q2;
            Position[3] = q3;
            Index = new int[4];
            Index[0] = 0;
            Index[1] = 1;
            Index[2] = 2;
            Index[3] = 3;

            Texcoord = new Vector2[4];
            Texcoord[0] = Vector2.Zero;
            Texcoord[1] = Vector2.UnitX;
            Texcoord[2] = Vector2.One;
            Texcoord[3] = Vector2.UnitY;


            CreateModel();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public Rectangle()
        {
            Position = new Vector3[4];
            Position[0] = new Vector3(-1, -1, 0);
            Position[1] = new Vector3(1, -1, 0);
            Position[2] = new Vector3(1, 1, 0);
            Position[3] = new Vector3(-1, 1, 0);
            Index = new int[4];
            Index[0] = 0;
            Index[1] = 1;
            Index[2] = 2;
            Index[3] = 3;


            Texcoord = new Vector2[4];
            Texcoord[0] = Vector2.Zero;
            Texcoord[1] = Vector2.UnitX;
            Texcoord[2] = Vector2.One;
            Texcoord[3] = Vector2.UnitY;

            CreateModel();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Polygon Model { get; private set; }

        #region [形状の作成]
        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateModel()
        {
            Formula = Plane.Formula(Position[0], Position[1], Position[2]);

            Mesh mesh = new Mesh(
                new Vertex(0, Position[0], Formula.Xyz, Vector3.UnitX, Vector2.Zero),
                new Vertex(1, Position[1], Formula.Xyz, Vector3.UnitY, Vector2.UnitX),
                new Vertex(2, Position[2], Formula.Xyz, Vector3.UnitZ, Vector2.One),
                new Vertex(3, Position[3], Formula.Xyz, Vector3.One, Vector2.UnitY));

            Model = new Polygon("Rectangle", new List<Mesh>() { mesh }, KIPrimitiveType.Quads);
        }
        #endregion
    }
}
