using System;
using OpenTK;

namespace KI.Gfx.Geometry
{
    /// <summary>
    /// 頂点
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <param name="position">位置</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        /// <param name="texcoord">テクスチャ座標</param>
        public Vertex(int index, Vector3 position, Vector3 normal, Vector3 color, Vector2 texcoord)
        {
            Index = index;
            Position = position;
            Normal = normal;
            Color = color;
            TexCoord = texcoord;
        }

        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <param name="position">位置</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        public Vertex(int index, Vector3 position)
            : this(index, position, Vector3.Zero, Vector3.One, Vector2.Zero)
        {
        }

        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <param name="position">位置</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        public Vertex(int index, Vector3 position, Vector3 normal, Vector3 color)
            : this(index, position, normal, color, Vector2.Zero)
        {
        }

        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <param name="position">位置</param>
        /// <param name="color">色</param>
        /// <param name="texcoord">テクスチャ座標</param>
        public Vertex(int index, Vector3 position, Vector3 color, Vector2 texcoord)
            : this(index, position, Vector3.Zero, color, texcoord)
        {
        }

        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <param name="position">位置</param>
        /// <param name="color">色</param>
        public Vertex(int index, Vector3 position, Vector3 color)
            : this(index, position, Vector3.Zero, color, Vector2.Zero)
        {
        }

        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <param name="position">位置</param>
        /// <param name="texcoord">テクスチャ座標</param>
        public Vertex(int index, Vector3 position, Vector2 texcoord)
            : this(index, position, Vector3.Zero, Vector3.One, texcoord)
        {
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <param name="vertex">頂点</param>
        public Vertex(int index, Vertex vertex)
            : this(index, vertex.Position, vertex.Normal, vertex.Color, vertex.TexCoord)
        {
        }

        /// <summary>
        /// 識別子
        /// </summary>
        public int Index { get; set; }
        
        /// <summary>
        /// 位置
        /// </summary>
        public virtual Vector3 Position { get; set; }

        /// <summary>
        /// 法線
        /// </summary>
        public virtual Vector3 Normal { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public virtual Vector3 Color { get; set; }

        /// <summary>
        /// テクスチャ座標
        /// </summary>
        public Vector2 TexCoord { get; set; }

        /// <summary>
        /// 編集したときに呼ぶ
        /// </summary>
        public virtual void Modified()
        {
        }
    }
}
