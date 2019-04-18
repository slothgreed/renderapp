using System.Collections.Generic;
using System.Linq;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Renderer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CADApp.Model.Node
{
    /// <summary>
    /// アプリケーションルートノード
    /// </summary>
    public class AppRootNode : SceneNode
    {
        /// <summary>
        /// 選択中の頂点バッファ
        /// </summary>
        VertexBuffer selectVertexBuffer;

        bool VisibleSelectVertex;
        bool VisibleSelectLine;
        bool VisibleSelectTriangle;

        /// <summary>
        /// 選択中の稜線のVBO
        /// </summary>
        VertexBuffer selectLineBuffer;

        /// <summary>
        /// 選択中のTriangle のVBO
        /// </summary>
        VertexBuffer selectTriangleBuffer;

        /// <summary>
        /// 選択中のコントロールポイントのVBO
        /// </summary>
        VertexBuffer selectControlPointBuffer;

        Material material;

        public AppRootNode(string name)
            : base(name)
        {
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            material = new Material(shader);
            GenerateBuffer();
        }

        private void GenerateBuffer()
        {
            selectVertexBuffer = new VertexBuffer();
            selectLineBuffer = new VertexBuffer();
            selectTriangleBuffer = new VertexBuffer();
            selectControlPointBuffer = new VertexBuffer();
        }

        public void UpdateSelectObject()
        {
            List<Vertex> vertexs = new List<Vertex>();
            List<Vertex> triangleVertex = new List<Vertex>();
            List<Vertex> lineVertex = new List<Vertex>();

            VisibleSelectVertex = false;
            VisibleSelectLine = false;
            VisibleSelectTriangle = false;

            foreach (var node in AllChildren().OfType<AssemblyNode>())
            {
                foreach (var index in node.Assembly.SelectVertexs)
                {
                    vertexs.Add(new Vertex(vertexs.Count, node.Assembly.Vertex[index].Position, Vector3.UnitZ));
                }

                foreach (var index in node.Assembly.SelectControlPoints)
                {
                    vertexs.Add(new Vertex(vertexs.Count, node.Assembly.ControlPoint[index].Position, Vector3.UnitZ));
                }

                foreach (var index in node.Assembly.SelectLines)
                {
                    int start;
                    int end;
                    node.Assembly.GetLine(index, out start, out end);
                    lineVertex.Add((new Vertex(lineVertex.Count, node.Assembly.Vertex[start].Position, Vector3.UnitZ)));
                    lineVertex.Add((new Vertex(lineVertex.Count, node.Assembly.Vertex[end].Position, Vector3.UnitZ)));
                }

                foreach (var index in node.Assembly.SelectTriangles)
                {
                    int triangle0;
                    int triangle1;
                    int triangle2;
                    node.Assembly.GetTriangle(index, out triangle0, out triangle1, out triangle2);
                    triangleVertex.Add(new Vertex(triangleVertex.Count, node.Assembly.Vertex[triangle0].Position, Vector3.UnitZ));
                    triangleVertex.Add(new Vertex(triangleVertex.Count, node.Assembly.Vertex[triangle1].Position, Vector3.UnitZ));
                    triangleVertex.Add(new Vertex(triangleVertex.Count, node.Assembly.Vertex[triangle2].Position, Vector3.UnitZ));
                }
            }

            if (vertexs.Count > 0)
            {
                selectVertexBuffer.SetBuffer(vertexs.ToArray(), Enumerable.Range(0, vertexs.Count).ToArray());
                VisibleSelectVertex = true;
            }

            if(lineVertex.Count >0)
            {
                selectLineBuffer.SetBuffer(lineVertex.ToArray(), Enumerable.Range(0, lineVertex.Count).ToArray());
                VisibleSelectLine = true;
            }

            if (triangleVertex.Count > 0)
            {
                selectTriangleBuffer.SetBuffer(triangleVertex.ToArray(), Enumerable.Range(0, triangleVertex.Count).ToArray());
                VisibleSelectTriangle = true;
            }
        }

        public override void RenderCore(Scene scene)
        {
            if (VisibleSelectVertex == true)
            {
                Draw(scene, PolygonType.Points, selectVertexBuffer);
            }

            if (VisibleSelectLine)
            {
                GL.LineWidth(5);
                Draw(scene, PolygonType.Lines, selectLineBuffer);
                GL.LineWidth(1);
            }

            if (VisibleSelectTriangle)
            {
                Draw(scene, PolygonType.Triangles, selectTriangleBuffer);
            }
        }

        private void Draw(Scene scene, PolygonType type, VertexBuffer buffer)
        {
            ShaderHelper.InitializeState(scene, this, buffer, material);
            material.Shader.BindBuffer();
            if (buffer.EnableIndexBuffer)
            {
                DeviceContext.Instance.DrawElements(type, buffer.Num, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                DeviceContext.Instance.DrawArrays(type, 0, buffer.Num);
            }

            material.Shader.UnBindBuffer();
        }
    }
}
