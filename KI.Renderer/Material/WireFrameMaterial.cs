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

namespace KI.Renderer.Material
{
    /// <summary>
    /// ワイヤフレームのマテリアル
    /// </summary>
    public class WireFrameMaterial : MaterialBase
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
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">種類</param>
        /// <param name="shader">シェーダ</param>
        /// <param name="Color">色情報</param>
        public WireFrameMaterial(string name, VertexBuffer vertexBuffer, Shader shader, Vector3[] color, int[] index)
            : base(name, vertexBuffer, PrimitiveType.Lines, shader)
        {
            vertexColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            vertexColorBuffer.SetData(color, EArrayType.Vec3Array);
            vertexBuffer.ColorBuffer = vertexColorBuffer;

            indexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            indexBuffer.SetData(index, EArrayType.IntArray);
            vertexBuffer.IndexBuffer = indexBuffer;
            vertexBuffer.Num = index.Length;
            vertexBuffer.EnableIndexBuffer = true;
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
