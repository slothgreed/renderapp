using KI.Foundation.Core;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// アトリビュート
    /// </summary>
    public abstract class AttributeBase : KIObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">レンダリングタイプ</param>
        /// <param name="shader">シェーダ</param>
        public AttributeBase(string name, PrimitiveType type, Shader shader)
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
        public AttributeBase(string name, VertexBuffer vertexBuffer, PrimitiveType type, Shader shader)
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
        public PrimitiveType Type { get; private set; }

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; protected set; }

        /// <summary>
        /// 可視不可視
        /// </summary>
        public bool Visible { get; set; } = true;

        public virtual void Binding()
        {
        }

        public virtual void UnBinding()
        {
        }
    }
}
