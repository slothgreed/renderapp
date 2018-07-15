using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Asset.Primitive;
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
    public class CreateIsoLineCommand : CommandBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateIsoLineCommand(IsoLineCommandArgs commandArgs)
            : base(commandArgs)
        {
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute(CommandArgsBase commandArg)
        {
            var isoLineCommandArgs = commandArg as IsoLineCommandArgs;
            return CommandUtility.CanCreatePolygon(isoLineCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult Execute(CommandArgsBase commandArg)
        {
            var isoLineCommandArgs = commandArg as VertexCurvatureCommandArgs;
            var targetObject = isoLineCommandArgs.TargetObject;
            var scene = isoLineCommandArgs.Scene;

            var halfDS = targetObject.Polygon as HalfEdgeDS;
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
                        sum += (line.Start.Position - line.End.Position).Length;
                        var vertex2 = new Vertex(0, line.End.Position, color);

                        createLine.Add(new Line(vertex1, vertex2));
                    }
                }
            }

            Polygon isoLines = new Polygon("IsoLines", createLine);
            VertexBuffer vertexBuffer = new VertexBuffer();
            vertexBuffer.SetupLineBuffer(isoLines.Vertexs, isoLines.Index, isoLines.Lines);
            var polyAttriute = new PolygonAttribute("IsoLines", vertexBuffer, PolygonType.Lines, targetObject.Shader);
            targetObject.Attributes.Add(polyAttriute);

            var parentNode = Global.Renderer.ActiveScene.FindNode(targetObject);
            scene.AddObject(polyAttriute, parentNode);

            return CommandResult.Success;
        }

        public override CommandResult Undo(CommandArgsBase commandArg)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 等値線の作成コマンド
    /// </summary>
    public class IsoLineCommandArgs : CommandArgsBase
    {
        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        public RenderObject TargetObject { get; private set; }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="loopNum">ループ回数</param>
        public IsoLineCommandArgs(RenderObject targetNode, Scene scene)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
        }
    }
}
