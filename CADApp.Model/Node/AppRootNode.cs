using System.Collections.Generic;
using System.Linq;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
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

        Shader shader;
        public AppRootNode(string name)
            : base(name)
        {
            shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
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
            foreach (var node in AllChildren().OfType<AssemblyNode>())
            {
                foreach (var index in node.Assembly.SelectVertexs)
                {
                    vertexs.Add(new Vertex(vertexs.Count, node.Assembly.Vertex[index].Position, Vector3.UnitZ));
                }

                foreach (var index in node.Assembly.SelectLines)
                {
                }

                foreach (var index in node.Assembly.SelectTriangles)
                {
                }

                foreach (var index in node.Assembly.SelectControlPoints)
                {
                    vertexs.Add(new Vertex(vertexs.Count, node.Assembly.ControlPoint[index].Position, Vector3.UnitZ));
                }
            }

            if(vertexs.Count > 0)
            {
                selectVertexBuffer.SetBuffer(vertexs.ToArray(), Enumerable.Range(0, vertexs.Count).ToArray());
            }
        }

        public override void RenderCore(Scene scene)
        {
            Draw(scene, PolygonType.Points, selectVertexBuffer);
        }

        private void Draw(Scene scene, PolygonType type, VertexBuffer buffer)
        {
            ShaderHelper.InitializeState(shader, scene, this, buffer, null);
            shader.BindBuffer();
            if (buffer.EnableIndexBuffer)
            {
                DeviceContext.Instance.DrawElements(type, buffer.Num, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                DeviceContext.Instance.DrawArrays(type, 0, buffer.Num);
            }

            shader.UnBindBuffer();
        }
    }
}
