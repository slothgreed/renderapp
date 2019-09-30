using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// 頂点カラーのアトリビュート
    /// </summary>
    public class VertexColorAttribute : AttributeBase
    {
        /// <summary>
        /// 頂点パラメータ
        /// </summary>
        private Vector3[] vertexColors;

        /// <summary>
        /// カラーバッファ
        /// </summary>
        private ArrayBuffer vertexColorBuffer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">種類</param>
        /// <param name="material">マテリアル</param>
        /// <param name="Color">色情報</param>
        public VertexColorAttribute(string name, VertexBuffer vertexBuffer, Material material, Vector3[] colors)
            : base(name, vertexBuffer, material)
        {
            vertexColors = colors;
            vertexColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            vertexColorBuffer.SetData(colors, EArrayType.Vec3Array);
            vertexBuffer.ColorBuffer = vertexColorBuffer;
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            vertexColorBuffer.Dispose();
            base.Dispose();
        }
    }
}
