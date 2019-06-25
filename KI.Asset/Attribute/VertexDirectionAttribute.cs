using System;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// 頂点方向のアトリビュート
    /// </summary>
    public class VertexDirectionAttribute : AttributeBase
    {
        /// <summary>
        /// 配列バッファ
        /// </summary>
        private ArrayBuffer vertexPositionBuffer;

        /// <summary>
        /// ワイヤフレームの色
        /// </summary>
        private Vector4 lineColor;

        /// <summary>
        /// ベクトルのアングル
        /// </summary>
        private float vectorAngle = 30;

        /// <summary>
        /// ベクトルの長さ
        /// </summary>
        private float vectorLength = 0.01f;

        /// <summary>
        /// 傘部分の長さ
        /// </summary>
        private float hatLength = 0.005f;

        /// <summary>
        /// 方向
        /// </summary>
        public Vector3[] Direction { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="polygon">線分</param>
        /// <param name="material">マテリアル</param>
        public VertexDirectionAttribute(string name, Material material, Vector3[] position, Vector3[] direction, Vector4 color, Vector3[] normal = null, bool displayVector = false)
            : base(name, KIPrimitiveType.Lines, material)
        {
            lineColor = color;
            Direction = direction;

            Vector3[] vectors;

            if (displayVector)
            {
                vectors = CreateVectorLine(position, direction, normal);
            }
            else
            {
                vectors = CreateLine(position, direction);
            }

            SetupBuffer(vectors);
        }

        private void SetupBuffer(Vector3[] lines)
        {
            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
            }

            VertexBuffer = new VertexBuffer();
            vertexPositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            vertexPositionBuffer.SetData(lines, EArrayType.Vec3Array);
            VertexBuffer.PositionBuffer = vertexPositionBuffer;
            VertexBuffer.Num = lines.Length;
        }

        /// <summary>
        /// 線分配列の算出
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="direction">線分</param>
        /// <returns>ベクトル</returns>
        private Vector3[] CreateLine(Vector3[] position, Vector3[] direction)
        {
            Vector3[] vectors = new Vector3[direction.Length * 2];

            var radian = MathHelper.RadiansToDegrees(vectorAngle);
            for (int i = 0; i < position.Length; i++)
            {
                var begin = position[i];
                var end = position[i] + direction[i];
                var vector = (end - begin).Normalized();

                end = vector * vectorLength + begin;
                vectors[2 * i + 0] = begin;
                vectors[2 * i + 1] = end;
            }

            return vectors;
        }

        /// <summary>
        /// ベクトル配列の算出
        /// </summary>
        /// <param name="lines">線分</param>
        /// <param name="normals">線分の法線</param>
        /// <returns>ベクトル</returns>
        private Vector3[] CreateVectorLine(Vector3[] position, Vector3[] direction, Vector3[] normals)
        {
            if (normals == null)
            {
                throw new ArgumentNullException();
            }

            Vector3[] vectors = new Vector3[direction.Length * 6];

            var radian = MathHelper.RadiansToDegrees(vectorAngle);
            for (int i = 0; i < position.Length; i++)
            {
                var begin = position[i];
                var end = position[i] + direction[i];
                var vector = (end - begin).Normalized();

                var matrix1 = Matrix4.CreateFromAxisAngle(normals[i], radian);
                var matrix2 = Matrix4.CreateFromAxisAngle(normals[i], -radian);

                var posit1 = Vector4.Transform(new Vector4(vector), matrix1).Xyz;
                var posit2 = Vector4.Transform(new Vector4(vector), matrix2).Xyz;

                end = vector * vectorLength + begin;
                vectors[6 * i + 0] = begin;
                vectors[6 * i + 1] = end;

                vectors[6 * i + 2] = end;
                vectors[6 * i + 3] = posit1 * hatLength + end;

                vectors[6 * i + 4] = end;
                vectors[6 * i + 5] = posit2 * hatLength + end;
            }

            return vectors;
        }

        public override void Binding()
        {
            foreach (var info in Material.Shader.GetShaderVariable())
            {
                switch (info.Name)
                {
                    case "u_Color":
                        info.Variable = lineColor;
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
