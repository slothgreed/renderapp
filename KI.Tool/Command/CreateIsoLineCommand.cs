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
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil.Buffer;
using KI.Mathmatics;
using OpenTK;

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

            algorithm.Calculate(0.05f);

            List<Line> createLine = new List<Line>();
            foreach (var isoSpace in algorithm.IsoSpace)
            {
                foreach (var isoLine in isoSpace.IsoLines)
                {
                    float length = isoLine.Length;
                    float sum = 0;
                    foreach(var line in isoLine.Lines)
                    {
                        Vector3 color = PseudoColor.GetColor(sum, 0, length);
                        var vertex1 = new Vertex(0, line.Start.Position, color);

                        Vector3 color2 = PseudoColor.GetColor(sum, 0, length);
                        sum += line.Length;
                        var vertex2 = new Vertex(0, line.End.Position, color);

                        createLine.Add(new Line(vertex1, vertex2));
                    }
                }
            }

            Polygon isoLines = new Polygon("IsoLines", createLine);
            VertexBuffer vertexBuffer = new VertexBuffer();
            vertexBuffer.SetupLineBuffer(isoLines.Vertexs, isoLines.Index, isoLines.Lines);
            var polyAttriute = new PolygonAttribute("IsoLines", vertexBuffer, PolygonType.Lines, renderObject.Shader);
            renderObject.Attributes.Add(polyAttriute);

            var parentNode = Global.Renderer.ActiveScene.FindNode(renderObject);
            Global.Renderer.ActiveScene.AddObject(polyAttriute, parentNode);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
