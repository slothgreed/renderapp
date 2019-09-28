using KI.Asset.Primitive;
using KI.Gfx.KITexture;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.HUD
{
    /// <summary>
    /// 背景
    /// </summary>
    public class BackGround
    {
        /// <summary>
        /// 背景の平面
        /// </summary>
        public Rectangle Plane { get; private set; }

        /// <summary>
        /// 球
        /// </summary>
        public Sphere Sphere { get; private set; }

        /// <summary>
        /// テクスチャ
        /// </summary>
        public Texture Texture { get; private set; }

        /// <summary>
        /// 背景
        /// </summary>
        /// <param name="color">背景色</param>
        public BackGround(Vector3 color)
        {
            Plane = new Rectangle(
                new Vector3(-1, -1, 0),
                new Vector3(1, -1, 0),
                new Vector3(1, 1, 0),
                new Vector3(-1, 1, 0));

            Sphere = new Sphere(0.1f, 10, 10, true);
            Plane.Vertexs[0].Color = Vector3.Zero;
            Plane.Vertexs[1].Color = Vector3.Zero;
            Plane.Vertexs[2].Color = Vector3.UnitZ;
            Plane.Vertexs[3].Color = Vector3.UnitZ;
        }

        /// <summary>
        /// 背景
        /// </summary>
        /// <param name="texture">テクスチャ</param>
        public BackGround(Texture texture)
        {
            Plane = new Rectangle(Vector3.Zero, Vector3.UnitX, Vector3.UnitX + Vector3.UnitY, Vector3.UnitY);
            Texture = texture;
        }

    }
}
