using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Attribute
{
    /// <summary>
    /// 標準アトリビュート
    /// </summary>
    public class PolygonAttribute : AttributeBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="shader">シェーダ</param>
        public PolygonAttribute(string name, VertexBuffer vertexBuffer, PrimitiveType type, Shader shader)
            : base(name, vertexBuffer, type, shader)
        {
        }
    }
}
