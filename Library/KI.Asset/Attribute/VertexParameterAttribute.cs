using System.Linq;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Mathmatics;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// 頂点パラメータのアトリビュート
    /// </summary>
    public class VertexParameterAttribute : AttributeBase
    {
        /// <summary>
        /// 頂点パラメータ
        /// </summary>
        private float[] VertexParameter;

        /// <summary>
        /// カラーバッファ
        /// </summary>
        private ArrayBuffer vertexColorBuffer { get; set; }

        /// <summary>
        /// 最小値
        /// </summary>
        public float Min { get; set; }

        /// <summary>
        /// 最大値
        /// </summary>
        public float Max { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">種類</param>
        /// <param name="material">マテリアル</param>
        /// <param name="Color">色情報</param>
        public VertexParameterAttribute(string name, VertexBuffer vertexBuffer, Material material, float[] parameter)
            : base(name, vertexBuffer, material)
        {
            Max = parameter.Max();
            Min = parameter.Min();
            VertexParameter = parameter;
            vertexColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            UpdateVertexColor(Min, Max);
            vertexBuffer.ColorBuffer = vertexColorBuffer;

            VertexBuffer.EnableIndexBuffer = true;
        }

        /// <summary>
        /// 頂点カラーの更新
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <param name="lowValue">最小値</param>
        /// <param name="heightValue">最大値</param>
        public void UpdateVertexColor(float lowValue, float heightValue)
        {
            var colors = VertexParameter.Select(p => PseudoColor.GetColor(p, lowValue, heightValue)).ToArray();
            vertexColorBuffer.SetData(colors, EArrayType.Vec3Array);
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
