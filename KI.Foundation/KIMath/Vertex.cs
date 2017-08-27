using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace KI.Foundation.KIMath
{
    /// <summary>
    /// 頂点
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        /// <param name="texcoord">テクスチャ座標</param>
        /// <param name="index">要素番号</param>
        public Vertex(Vector3 position, Vector3 normal, Vector3 color, Vector2 texcoord, int index)
        {
            Position = position;
            Normal = normal;
            Color = color;
            TexCoord = texcoord;
            Index = index;
        }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 法線
        /// </summary>
        public virtual Vector3 Normal { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public Vector3 Color { get; set; }

        /// <summary>
        /// テクスチャ座標
        /// </summary>
        public Vector2 TexCoord { get; set; }

        /// <summary>
        /// 要素番号
        /// </summary>
        public int Index { get; set; }

    }
}
