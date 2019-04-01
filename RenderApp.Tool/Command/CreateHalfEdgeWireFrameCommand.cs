using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Asset;
using OpenTK;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// ハーフエッジのワイヤフレーム作成
    /// </summary>
    public class CreateHalfEdgeWireFrameCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private HalfEdgeWireFrameCommandArgs wireframeCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CreateHalfEdgeWireFrameCommand(HalfEdgeWireFrameCommandArgs commandArgs)
        {
            wireframeCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(wireframeCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Execute()
        {
            var targetObject = wireframeCommandArgs.TargetObject;
            var scene = wireframeCommandArgs.Scene;

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
        /// <returns>成功値</returns>
        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// ハーフエッジのワイヤフレームコマンド
    /// </summary>
    public class HalfEdgeWireFrameCommandArgs
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
