using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Asset;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// ハーフエッジのワイヤフレーム作成
    /// </summary>
    public class CreateHalfEdgeWireFrameCommand : CommandBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateHalfEdgeWireFrameCommand(HalfEdgeWireFrameCommandArgs commandArgs)
            :base(commandArgs)
        {
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute(CommandArgsBase commandArg)
        {
            var wireframeCommandArgs = commandArg as HalfEdgeWireFrameCommandArgs;
            return CommandUtility.CanCreatePolygon(wireframeCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult Execute(CommandArgsBase commandArg)
        {
            var wireFrameCommandArgs = commandArg as HalfEdgeWireFrameCommandArgs;
            var targetObject = wireFrameCommandArgs.TargetObject;
            var scene = wireFrameCommandArgs.Scene;

            List<Vector3> position = new List<Vector3>();
            var color = new List<Vector3>();
            List<Line> lines = new List<Line>();
            var halfEdgeDS = targetObject.Polygon as HalfEdgeDS;
            foreach (var mesh in halfEdgeDS.HalfEdgeMeshs)
            {
                foreach (var edge in mesh.AroundEdge)
                {
                    var start = (edge.Start.Position - mesh.Gravity) * 0.8f;
                    var end = (edge.End.Position - mesh.Gravity) * 0.8f;

                    position.Add(start + mesh.Gravity);
                    position.Add(end + mesh.Gravity);

                    color.Add(Vector3.UnitZ);
                    color.Add(Vector3.UnitZ);

                    lines.Add(
                        new Line(
                            new Vertex(2 * lines.Count, start + mesh.Gravity, Vector3.UnitZ), 
                            new Vertex(2 * lines.Count + 1, end + mesh.Gravity, Vector3.UnitZ)));
                }
            }

            var polygon = new Polygon("HalfEdgeWireFrame :" + targetObject.Name, lines);
            RenderObject wireframe = RenderObjectFactory.Instance.CreateRenderObject("HalfEdgeWireFrame :" + targetObject.Name, polygon);
            wireframe.ModelMatrix = targetObject.ModelMatrix;
            scene.AddObject(wireframe);

            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult Undo(CommandArgsBase commandArg)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// ハーフエッジのワイヤフレームコマンド
    /// </summary>
    public class HalfEdgeWireFrameCommandArgs : CommandArgsBase
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
        public HalfEdgeWireFrameCommandArgs(RenderObject targetNode, Scene scene)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
        }
    }


}
