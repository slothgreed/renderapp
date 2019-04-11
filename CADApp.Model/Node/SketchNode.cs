using System;
using System.Linq;
using CADApp.Model.Assembly;
using KI.Asset.Attribute;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Renderer;
using OpenTK.Graphics.OpenGL;

namespace CADApp.Model.Node
{
    public class SketchNode : SceneNode
    {
        /// <summary>
        /// 頂点バッファ
        /// </summary>
        VertexBuffer vertexBuffer;

        /// <summary>
        /// 稜線のインデックスバッファ
        /// </summary>
        VertexBuffer lineBufer;

        /// <summary>
        /// Triangle のインデックスバッファ
        /// </summary>
        VertexBuffer triangleBuffer;

        Shader shader;

        /// <summary>
        /// スケッチの頂点情報
        /// </summary>
        public Sketch Sketch { get; private set; }

             
        public SketchNode(string name, Sketch sketch, Shader _shader)
           : base(name)
        {
            shader = _shader;
            Sketch = sketch;
            sketch.SketchUpdated += OnSketchUpdated;
            GenerateBuffer();
        }

        private void GenerateBuffer()
        {
            vertexBuffer = new VertexBuffer();
            lineBufer = vertexBuffer.ShallowCopy();
            lineBufer.IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            triangleBuffer = vertexBuffer.ShallowCopy();
            triangleBuffer.IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
        }


        /// <summary>
        /// 点・線・ポリゴンのレンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(Scene scene)
        {
            if (Sketch.CurrentEdit == true)
            {
                throw new Exception();
            }

            Draw(scene, PolygonType.Points, vertexBuffer);
            Draw(scene, PolygonType.Lines, lineBufer);
            Draw(scene, PolygonType.Triangles, triangleBuffer);
        }

        private void Draw(Scene scene, PolygonType type, VertexBuffer vertexBuffer)
        {
            ShaderHelper.InitializeState(shader, scene, this, vertexBuffer, null);
            shader.BindBuffer();
            if (vertexBuffer.EnableIndexBuffer)
            {
                DeviceContext.Instance.DrawElements(type, vertexBuffer.Num, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                DeviceContext.Instance.DrawArrays(type, 0, vertexBuffer.Num);
            }

            shader.UnBindBuffer();
        }

        public override void Dispose()
        {
            vertexBuffer.Dispose();
            BufferFactory.Instance.RemoveByValue(lineBufer.IndexBuffer);
            Sketch.SketchUpdated -= OnSketchUpdated;
            base.Dispose();
        }

        private void OnSketchUpdated(object sender, EventArgs e)
        {
            if (Sketch.Vertex != null)
            {
                vertexBuffer.SetBuffer(Sketch.Vertex.ToArray(), Enumerable.Range(0, Sketch.Vertex.Count).ToArray());
            }

            if (Sketch.LineIndex != null)
            {
                lineBufer.SetIndexArray(Sketch.LineIndex.ToArray());
            }
            if (Sketch.TriangleIndex != null)
            {
                triangleBuffer.SetIndexArray(Sketch.TriangleIndex.ToArray());
            }
        }
    }
}
