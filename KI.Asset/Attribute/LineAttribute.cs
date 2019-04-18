using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// ワイヤフレームのアトリビュート
    /// </summary>
    public class LineAttribute : AttributeBase
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
        public Vector4 Color { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">種類</param>
        /// <param name="material">マテリアル</param>
        /// <param name="Color">色情報</param>
        public LineAttribute(string name, VertexBuffer vertexBuffer, Vector4 color, int[] index)
            : base(name, vertexBuffer, PolygonType.Lines, new Material())
        {
            Color = color;

            vertexBuffer.IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            vertexBuffer.SetIndexArray(index);

            Material.Shader = ShaderCreater.Instance.CreateShader(SHADER_TYPE.Line);
        }

        public override void Binding()
        {
            foreach (var info in Material.Shader.GetShaderVariable())
            {
                if (info.Name == "u_LineColor")
                {
                    info.Variable = Color;
                }
            }
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