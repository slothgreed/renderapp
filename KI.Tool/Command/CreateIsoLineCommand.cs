using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil.Buffer;
using KI.Mathmatics;

namespace KI.Tool.Command
{
    /// <summary>
    /// 等値線の作成コマンド
    /// </summary>
    public class CreateIsoLineCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// シーン
        /// </summary>
        private Scene scene;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateIsoLineCommand(Scene scene, KIObject asset)
        {
            this.scene = scene;
            renderObject = asset as RenderObject;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult CanExecute(string commandArg)
        {
            if (renderObject == null)
            {
                return CommandResult.Failed;
            }

            if (renderObject.Polygon is HalfEdgeDS)
            {
                return CommandResult.Success;
            }

            return CommandResult.Failed;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Execute(string commandArg)
        {
            var halfDS = renderObject.Polygon as HalfEdgeDS;
            IsoLineAlgorithm algorithm = new IsoLineAlgorithm(halfDS);

            algorithm.Calculate(0.1f);

            List<Mesh> createMesh = new List<Mesh>();
            for (int i = 0; i < algorithm.isoSpace.Length; i++)
            {
                var color = RandamValue.Color();
                var meshes = algorithm.isoSpace[i].Edges.Select(p => p.Mesh);
                foreach (var mesh in meshes)
                {
                    var vertexs = mesh.Vertexs;
                    Vertex[] createVertexs = new Vertex[vertexs.Count];

                    createVertexs[0] = new Vertex(0, vertexs[0].Position, color);
                    createVertexs[1] = new Vertex(0, vertexs[1].Position, color);
                    createVertexs[2] = new Vertex(0, vertexs[2].Position, color);

                    createMesh.Add(new Mesh(createVertexs[0], createVertexs[1], createVertexs[2]));
                }
            }

            Polygon polygon = new Polygon("IsoLinePoly_forDebug", createMesh, OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);
            VertexBuffer vertexBuffer = new VertexBuffer();
            vertexBuffer.SetupBuffer(polygon);

            var polyAttribute = new PolygonAttribute("IsoLineAttribute", vertexBuffer, OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, renderObject.Shader);
            renderObject.Attributes.Add(polyAttribute);

            var parentNode = Global.Renderer.ActiveScene.FindNode(renderObject);
            Global.Renderer.ActiveScene.AddObject(polyAttribute, parentNode);


            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
