using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// ワイヤフレームのアトリビュート
    /// </summary>
    public class WireFrameAttribute : AttributeBase
    {
        /// <summary>
        /// カラーバッファ
        /// </summary>
        private ArrayBuffer vertexColorBuffer { get; set; }

        /// <summary>
        /// 頂点番号バッファ
        /// </summary>
        private ArrayBuffer indexBuffer { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        private Vector3[] colors;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">種類</param>
        /// <param name="shader">シェーダ</param>
        /// <param name="Color">色情報</param>
        public WireFrameAttribute(string name, VertexBuffer vertexBuffer, Shader shader, Vector3[] color, int[] index)
            : base(name, vertexBuffer, PolygonType.Lines, shader)
        {
            colors = color;
            vertexBuffer.ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            vertexBuffer.SetColor(color);

            vertexBuffer.IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            vertexBuffer.SetIndexArray(index);
        }

        /// <summary>
        /// 色の更新
        /// </summary>
        /// <param name="color">色</param>
        public void UpdateColor(Vector3 color)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }

            VertexBuffer.SetColor(colors);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            vertexColorBuffer.Dispose();
            indexBuffer.Dispose();
            base.Dispose();
        }
    }
}
