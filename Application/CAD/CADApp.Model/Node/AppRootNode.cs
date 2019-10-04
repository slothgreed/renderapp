using System.Collections.Generic;
using System.Linq;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Renderer;
using OpenTK;
using KI.Analyzer;
using KI.Asset.Gizmo;
using KI.Gfx.KIMaterial;
using KI.Gfx.Buffer;

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

        private bool VisibleSelectVertex;
        private bool VisibleSelectLine;
        private bool VisibleSelectTriangle;

        private MoveGizmo moveGizmo;
        private ScaleGizmo scaleGizmo;
        private RotateGizmo rotateGizmo;


        /// <summary>
        /// 選択しているかどうか
        /// </summary>
        public bool HasSelectObject
        {
            get
            {
                return
                    VisibleSelectVertex ||
                    VisibleSelectLine ||
                    VisibleSelectTriangle;
            }
        }

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

        /// <summary>
        /// 移動GizmoのVBO
        /// </summary>
        VertexBuffer moveGizmoBuffer;


        /// <summary>
        /// 移動GizmoのVBO
        /// </summary>
        VertexBuffer scaleGizmoBuffer;


        /// <summary>
        /// 選択中の形状全体のバウンディングボックス
        /// </summary>
        private BDB selectBDB;

        /// <summary>
        /// 選択中の形状全体のバウンディングボックスのゲッタ
        /// </summary>
        public BDB SelectBDB
        {
            get
            {
                return selectBDB;
            }
        }

        private Material material;

        private Material lineMaterial;

        public AppRootNode(string name)
            : base(name)
        {
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            material = new Material(shader);
            lineMaterial = new LineMaterial(shader, 5);
            selectBDB = new BDB();
            GenerateGizmo();
            GenerateSelectVertexBuffer();
        }

        private void GenerateGizmo()
        {
            moveGizmo = new MoveGizmo();
            moveGizmoBuffer = new VertexBuffer();
            moveGizmoBuffer.SetBuffer(KIPrimitiveType.Triangles, moveGizmo.Vertex, null, moveGizmo.Color, null, moveGizmo.Index);

            scaleGizmo = new ScaleGizmo();
            scaleGizmoBuffer = new VertexBuffer();
            scaleGizmoBuffer.SetBuffer(KIPrimitiveType.Triangles, scaleGizmo.Vertex, null, scaleGizmo.Color, null, scaleGizmo.Index);

        }

        private void GenerateSelectVertexBuffer()
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
                selectVertexBuffer.SetBuffer(KIPrimitiveType.Points, vertexs.ToArray(), Enumerable.Range(0, vertexs.Count).ToArray());
                selectBDB.Update(vertexs.Select(p => p.Position).ToList());
                VisibleSelectVertex = true;
            }

            if(lineVertex.Count >0)
            {
                selectLineBuffer.SetBuffer(KIPrimitiveType.Lines, lineVertex.ToArray(), Enumerable.Range(0, lineVertex.Count).ToArray());
                selectBDB.Update(lineVertex.Select(p => p.Position).ToList());
                VisibleSelectLine = true;
            }

            if (triangleVertex.Count > 0)
            {
                selectTriangleBuffer.SetBuffer(KIPrimitiveType.Triangles, triangleVertex.ToArray(), Enumerable.Range(0, triangleVertex.Count).ToArray());
                selectBDB.Update(triangleVertex.Select(p => p.Position).ToList());
                VisibleSelectTriangle = true;
            }
        }

        public override void RenderCore(Scene scene, RenderInfo renderInfo)
        {
            if (VisibleSelectVertex == true)
            {
                Draw(scene, selectVertexBuffer, material);
            }

            if (VisibleSelectLine)
            {
                Draw(scene, selectLineBuffer, lineMaterial);
            }

            if (VisibleSelectTriangle)
            {
                Draw(scene, selectTriangleBuffer, material);
            }

            //Draw(scene, moveGizmoBuffer, material);
            Draw(scene, scaleGizmoBuffer, material);
        }

        private void Draw(Scene scene, VertexBuffer buffer, Material material)
        {
            ShaderHelper.InitializeState(scene, this, buffer, material);
            material.BindToGPU();
            buffer.Render();

            material.UnBindToGPU();
        }
    }
}
