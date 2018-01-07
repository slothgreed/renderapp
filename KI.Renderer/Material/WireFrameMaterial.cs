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
        /// 一時保存用のテンポラリカラーバッファ
        /// </summary>
        private ArrayBuffer localColorBuffer { get; set; }

        /// <summary>
        /// カラーバッファ
        /// </summary>
        private ArrayBuffer vertexColorBuffer { get; set; }

        /// <summary>
        /// 一時保存用のテンポラリカラーバッファ
        /// </summary>
        private ArrayBuffer localIndexBuffer { get; set; }

        /// <summary>
        /// インデックスバッファ
        /// </summary>
        private ArrayBuffer indexBuffer { get; set; }

        private int localNum;
        private int num = 0;

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
            ColorList = color;
            IndexBufferList = index;
            num = index.Length;
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
        /// 頂点インデックス情報
        /// </summary>
        private int[] IndexBufferList
        {
            set
            {
                if (value.Length == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }
                if (indexBuffer == null)
                {
                    indexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
                }

                indexBuffer.SetData(value, EArrayType.IntArray);
            }
        }

        /// <summary>
        /// 紐づけ
        /// </summary>
        /// <returns>成功か</returns>
        public override bool Binding()
        {
            //インデックスバッファと頂点カラーを一時的に変更
            localNum = VertexBuffer.Num;
            localIndexBuffer = VertexBuffer.IndexBuffer;
            VertexBuffer.ChangeIndexBuffer(indexBuffer, num);

            //頂点カラーを一時的に変更
            localColorBuffer = VertexBuffer.ColorBuffer;
            VertexBuffer.ChangeVertexColor(vertexColorBuffer);
            return true;
        }

        /// <summary>
        /// 紐づけ解除
        /// </summary>
        /// <returns>成功か</returns>
        public override bool UnBinding()
        {
            VertexBuffer.ChangeIndexBuffer(localIndexBuffer, localNum);
            localIndexBuffer = null;
            localNum = 0;

            VertexBuffer.ChangeVertexColor(localColorBuffer);
            localColorBuffer = null;
            return true;
        }
    }
}
