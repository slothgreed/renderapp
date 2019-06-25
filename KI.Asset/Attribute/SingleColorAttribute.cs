using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// 単一色のアトリビュート
    /// </summary>
    public class SingleColorAttribute : AttributeBase
    {
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
        public SingleColorAttribute(string name, VertexBuffer vertexBuffer, Vector4 color)
            : base(name, vertexBuffer, KIPrimitiveType.Lines, new Material())
        {
            Color = color;

            Material.Shader = ShaderCreater.Instance.CreateShader(SHADER_TYPE.SingleColor);
        }

        public override void Binding()
        {
            foreach (var info in Material.Shader.GetShaderVariable())
            {
                if (info.Name == "u_Color")
                {
                    info.Variable = Color;
                }
            }
        }
    }
}