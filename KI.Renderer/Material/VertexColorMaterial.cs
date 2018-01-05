using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Foundation.Utility;
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
        /// 一時保存用のテンポラリカラーバッファ
        /// </summary>
        private ArrayBuffer localColorBuffer { get; set; }

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
        public VertexColorMaterial(string name, Shader shader, Vector3[] color)
            : base(name, shader)
        {
            ColorList = color;
        }

        /// <summary>
        /// カラー情報
        /// </summary>
        private Vector3[] ColorList
        {
            set
            {
                if (value.Length == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }
                if (vertexColorBuffer == null)
                {
                    vertexColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                }

                vertexColorBuffer.SetData(value, EArrayType.Vec3Array);
            }
        }

        /// <summary>
        /// 紐づけ
        /// </summary>
        /// <param name="vertexBuffer">レンダリングする頂点バッファ</param>
        /// <returns>成功か</returns>
        public override bool Binding(VertexBuffer vertexBuffer)
        {
            //頂点カラーを一時的に変更
            localColorBuffer = vertexBuffer.ColorBuffer;
            vertexBuffer.ChangeVertexColor(vertexColorBuffer);
            return true;
        }

        /// <summary>
        /// 紐づけ解除
        /// </summary>
        /// <param name="vertexBuffer">レンダリングする頂点バッファ</param>
        /// <returns>成功か</returns>
        public override bool UnBinding(VertexBuffer vertexBuffer)
        {
            vertexBuffer.ChangeVertexColor(localColorBuffer);
            localColorBuffer = null;
            return true;
        }
    }
}
