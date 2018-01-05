using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Material
{
    /// <summary>
    /// 形状のマテリアル
    /// </summary>
    public class GeometryMaterial : MaterialBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">種類</param>
        /// <param name="shader">シェーダ</param>
        public GeometryMaterial(string name, PrimitiveType type, Shader shader)
            : base(name, type, shader)
        {
            VertexBuffer = new VertexBuffer();
        }

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; }

        public override bool Binding(VertexBuffer vertexBuffer)
        {
            return true;
        }

        public override bool UnBinding(VertexBuffer vertexBuffer)
        {
            return true;
        }
    }
}
