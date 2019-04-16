using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// ベクトル場のアトリビュート
    /// </summary>
    public class VectorFieldAttribute : AttributeBase
    {
        /// <summary>
        /// 方向場のバッファ
        /// </summary>
        private ArrayBuffer directionBuffer;

        public VectorFieldAttribute(string name, VertexBuffer vertexBuffer, Material material, Vector3[] directions, PolygonType polygonType)
            : base(name, vertexBuffer, polygonType, material)
        {
            directionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer, EArrayType.Vec3Array);
            directionBuffer.SetData(directions, EArrayType.Vec3Array);
        }

        public override void Binding()
        {
            foreach (var info in Material.Shader.GetShaderVariable())
            {
                switch (info.Name)
                {
                    case "direction":
                        info.Variable = directionBuffer;
                        break;
                }
            }
        }

        public override void Dispose()
        {
            directionBuffer.Dispose();
        }
    }
}
