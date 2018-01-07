using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    /// <summary>
    /// マテリアル
    /// </summary>
    public abstract class MaterialBase : KIObject
    {
        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">レンダリングタイプ</param>
        /// <param name="shader">シェーダ</param>
        public MaterialBase(string name, PrimitiveType type, Shader shader)
            : base(name)
        {
            Shader = shader;
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="shader">シェーダ</param>
        public MaterialBase(string name, VertexBuffer vertexBuffer, PrimitiveType type, Shader shader)
            : base(name)
        {
            VertexBuffer = vertexBuffer;
            Shader = shader;
            Type = type;
        }

        /// <summary>
        /// シェーダ
        /// </summary>
        public Shader Shader { get; set; }

        /// <summary>
        /// レンダリングするときの種類
        /// </summary>
        public PrimitiveType Type { get; set; }

        /// <summary>
        /// 可視不可視
        /// </summary>
        public bool Visible { get; set; } = true;
    }
}
