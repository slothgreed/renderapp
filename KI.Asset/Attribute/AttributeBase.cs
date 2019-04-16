using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;

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
        public AttributeBase(string name, PolygonType type, Material material)
            : base(name)
        {
            Material = material;
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="shader">シェーダ</param>
        public AttributeBase(string name, VertexBuffer vertexBuffer, PolygonType type, Material material)
            : base(name)
        {
            VertexBuffer = vertexBuffer;
            Material = material;
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="shader">シェーダ</param>
        public AttributeBase(string name, PolygonType type)
            : base(name)
        {
            Type = type;
        }

        /// <summary>
        /// マテリアル
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// レンダリングするときの種類
        /// </summary>
        public PolygonType Type { get; private set; }

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; }

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

        public void UpdateVertexBuffer(VertexBuffer vertexBuffer)
        {
            VertexBuffer = vertexBuffer;
        }
    }
}
