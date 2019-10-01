using System;
using System.Linq;
using KI.Asset.Attribute;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Renderer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CADApp.Model.Node
{
    public class AssemblyNode : SceneNode
    {
        /// <summary>
        /// 頂点
        /// </summary>
        AttributeBase pointAttribute;

        /// <summary>
        /// 稜線
        /// </summary>
        AttributeBase lineAttribute;

        /// <summary>
        /// コントロールポイント
        /// </summary>
        AttributeBase controlPointAttribute;

        /// <summary>
        /// Triangle
        /// </summary>
        AttributeBase triangleAttribute;

        Material material;

        private Assembly assembly;
        /// <summary>
        /// スケッチの頂点情報
        /// </summary>
        public Assembly Assembly
        {
            get { return assembly; }
            set
            {
                assembly = value;
                OnAssemblyUpdated(null, null);
                assembly.AssemblyUpdated += OnAssemblyUpdated;
            }
        }

        public bool VisibleVertex { get; set; } = true;
        public bool VisibleLine { get; set; } = true;
        public bool VisibleTriangle { get; set; } = true;
        public bool VisibleControlPoint { get; set; } = true;

        public AssemblyNode(string name, Assembly assmebly, Shader _shader)
           : base(name)
        {
            material = new Material(_shader);
            Assembly = assmebly;
        }

        public AssemblyNode(string name, Shader _shader)
            : base(name)
        {
            material = new Material(_shader);
        }

        /// <summary>
        /// 点・線・ポリゴンのレンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(Scene scene)
        {
            if (Assembly.CurrentEdit == true)
            {
                throw new Exception();
            }

            if (VisibleVertex)
            {
                Draw(scene, KIPrimitiveType.Points, pointAttribute);
            }

            if (VisibleControlPoint)
            {
                Draw(scene, KIPrimitiveType.Points, controlPointAttribute);
            }

            if (VisibleLine)
            {
                Draw(scene, KIPrimitiveType.Lines, lineAttribute);
            }

            if (VisibleTriangle)
            {
                Draw(scene, KIPrimitiveType.Triangles, triangleAttribute);
            }
        }

        private void Draw(Scene scene, KIPrimitiveType type, AttributeBase attribute)
        {
            ShaderHelper.InitializeState(scene, this, attribute.VertexBuffer, attribute.Material);
            attribute.Binding();
            attribute.Material.Shader.BindBuffer();
            attribute.VertexBuffer.Render();

            attribute.Material.Shader.UnBindBuffer();
        }

        public override void Dispose()
        {
            pointAttribute.VertexBuffer.Dispose();
            controlPointAttribute.VertexBuffer.Dispose();
            BufferFactory.Instance.RemoveByValue(lineAttribute.VertexBuffer.IndexBuffer);
            BufferFactory.Instance.RemoveByValue(triangleAttribute.VertexBuffer.IndexBuffer);

            Assembly.AssemblyUpdated -= OnAssemblyUpdated;

            pointAttribute = null;
            controlPointAttribute = null;
            lineAttribute = null;
            triangleAttribute = null;
            base.Dispose();
        }

        private void GenerateBuffer()
        {
            var controlPointBuffer = new VertexBuffer();
            var vertexBuffer = new VertexBuffer();
            var lineBuffer = vertexBuffer.ShallowCopy();
            var triangleBuffer = vertexBuffer.ShallowCopy();

            lineBuffer.IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            triangleBuffer.IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);

            lineAttribute = new SingleColorAttribute("Line", lineBuffer, Vector4.Zero);
            pointAttribute = new SingleColorAttribute("Point", vertexBuffer, Vector4.UnitY + Vector4.UnitZ + Vector4.UnitW);
            triangleAttribute = new PolygonAttribute("Triangle", triangleBuffer, material);
            controlPointAttribute = new SingleColorAttribute("Point", controlPointBuffer, Vector4.UnitY + Vector4.UnitW);
        }

        private void OnAssemblyUpdated(object sender, EventArgs e)
        {
            if (pointAttribute == null)
            {
                GenerateBuffer();
            }

            if (Assembly.Vertex != null &&
                Assembly.Vertex.Count > 0)
            {
                pointAttribute.VertexBuffer.SetBuffer(KIPrimitiveType.Points, Assembly.Vertex.ToArray(), Enumerable.Range(0, Assembly.Vertex.Count).ToArray());
            }

            if (Assembly.ControlPoint != null &&
                Assembly.ControlPoint.Count > 0)
            {
                controlPointAttribute.VertexBuffer.SetBuffer(KIPrimitiveType.Points, Assembly.ControlPoint.ToArray(), Enumerable.Range(0, Assembly.ControlPoint.Count).ToArray());
            }

            if (Assembly.LineIndex != null &&
                Assembly.LineIndex.Count > 1)
            {
                lineAttribute.VertexBuffer.SetIndexArray(KIPrimitiveType.Lines, Assembly.LineIndex.ToArray());
            }
            if (Assembly.TriangleIndex != null &&
                Assembly.TriangleIndex.Count > 2)
            {
                triangleAttribute.VertexBuffer.SetIndexArray(KIPrimitiveType.Triangles, Assembly.TriangleIndex.ToArray());
            }
        }
    }
}
