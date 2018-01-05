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
        public MaterialBase(string name, Shader shader)
            : base(name)
        {
            Shader = shader;
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
        /// 紐づけ
        /// </summary>
        /// <param name="vertexBuffer">レンダリングする頂点バッファ</param>
        /// <returns>成功か</returns>
        public abstract bool Binding(VertexBuffer vertexBuffer);

        /// <summary>
        /// 紐づけ解除
        /// </summary>
        /// <param name="vertexBuffer">レンダリングする頂点バッファ</param>
        /// <returns>成功か</returns>
        public abstract bool UnBinding(VertexBuffer vertexBuffer);

        /// <summary>
        /// 可視不可視
        /// </summary>
        public bool Visible { get; set; } = true;
    }
}
