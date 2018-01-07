using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 頂点パラメータ
        /// </summary>
        private float[] VertexParameter;

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
        public VertexColorMaterial(string name, VertexBuffer vertexBuffer, PrimitiveType type, Shader shader, float[] parameter)
            : base(name, vertexBuffer, type, shader)
        {
            Max = parameter.Max();
            Min = parameter.Min();
            VertexParameter = parameter;
            UpdateVertexColor(Min, Max);
        }

        public float Min { get; set; }

        public float Max { get; private set; }

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
        /// <returns>成功か</returns>
        public override bool Binding()
        {
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
            VertexBuffer.ChangeVertexColor(localColorBuffer);
            localColorBuffer = null;
            return true;
        }

        /// <summary>
        /// 頂点カラーの更新
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <param name="lowValue">最小値</param>
        /// <param name="heightValue">最大値</param>
        public void UpdateVertexColor(float lowValue, float heightValue)
        {
            ColorList = VertexParameter.Select(p => KICalc.GetPseudoColor(p, lowValue, heightValue)).ToArray();
        }
    }
}
