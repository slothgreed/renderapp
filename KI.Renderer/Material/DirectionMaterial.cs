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
    /// ベクトル表示用のマテリアル
    /// </summary>
    public class DirectionMaterial : MaterialBase
    {
        /// <summary>
        /// 配列バッファ
        /// </summary>
        private ArrayBuffer vertexPositionBuffer;

        /// <summary>
        /// ワイヤフレームの色
        /// </summary>
        private Vector4 wireFrameColor;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="shader">シェーダ</param>
        public DirectionMaterial(string name, Vector3[] postions, Vector4 color, Shader shader)
            : base(name, PrimitiveType.Lines, shader)
        {
            wireFrameColor = color;
            SetupBuffer(postions);
        }

        private void SetupBuffer(Vector3[] positions)
        {
            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
            }

            VertexBuffer = new VertexBuffer();
            vertexPositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            vertexPositionBuffer.SetData(positions,EArrayType.Vec3Array);
            VertexBuffer.PositionBuffer = vertexPositionBuffer;
            VertexBuffer.Num = positions.Length;
        }

        public override void Binding()
        {
            foreach (var info in Shader.GetShaderVariable())
            {
                switch (info.Name)
                {
                    case "wireColor":
                        info.Variable = wireFrameColor;
                        break;
                }
            }
        }

        public override void Dispose()
        {
            vertexPositionBuffer.Dispose();
        }
    }
}
