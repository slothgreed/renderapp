using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Attribute
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
            : base(name, vertexBuffer, PrimitiveType.Lines, shader)
        {
            colors = color;
            vertexColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            vertexBuffer.ColorBuffer = vertexColorBuffer;
            vertexColorBuffer.SetData(color, EArrayType.Vec3Array);

            indexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            indexBuffer.SetData(index, EArrayType.IntArray);
            vertexBuffer.IndexBuffer = indexBuffer;
            vertexBuffer.Num = index.Length;
            vertexBuffer.EnableIndexBuffer = true;
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

            vertexColorBuffer.SetData(colors, EArrayType.Vec3Array);
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
