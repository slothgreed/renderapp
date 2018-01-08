using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Material
{
    /// <summary>
    /// 頂点カラーのマテリアル
    /// </summary>
    public class VertexColorMaterial : MaterialBase
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
        /// <param name="shader">シェーダ</param>
        /// <param name="Color">色情報</param>
        public VertexColorMaterial(string name, VertexBuffer vertexBuffer, PrimitiveType type, Shader shader, Vector3[] colors)
            : base(name, vertexBuffer, type, shader)
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
