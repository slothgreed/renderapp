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

        public VectorFieldAttribute(string name, VertexBuffer vertexBuffer, Shader shader, Vector3[] directions, PolygonType polygonType)
            : base(name, vertexBuffer, polygonType, shader)
        {
            directionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            directionBuffer.SetData(directions, EArrayType.Vec3Array);
        }

        public override void Binding()
        {
            foreach(var info in Shader.GetShaderVariable())
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
