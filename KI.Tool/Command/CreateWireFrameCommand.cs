using System.Collections.Generic;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Asset;
using KI.Asset.Attribute;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// ワイヤフレームの作成
    /// </summary>
    public class CreateWireFrameCommand : CommandBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateWireFrameCommand(WireFrameCommandArgs commandArgs)
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
            var wireCommandArgs = commandArg as WireFrameCommandArgs;
            return CommandUtility.CanCreatePolygon(wireCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult Execute(CommandArgsBase commandArg)
        {
            var wireCommandArgs = commandArg as WireFrameCommandArgs;
            var targetObject = wireCommandArgs.TargetObject;
            var scene = wireCommandArgs.Scene;
            var color = wireCommandArgs.Color;

            List<int> lineIndex = new List<int>();
            List<Vector3> wireFrameColors = new List<Vector3>();
            foreach (var mesh in targetObject.Polygon.Meshs)
            {
                for (int j = 0; j < mesh.Vertexs.Count - 1; j++)
                {
                    lineIndex.Add(mesh.Vertexs[j].Index);
                    lineIndex.Add(mesh.Vertexs[j + 1].Index);
                }

                lineIndex.Add(mesh.Vertexs[mesh.Vertexs.Count - 1].Index);
                lineIndex.Add(mesh.Vertexs[0].Index);
                wireFrameColors.Add(color);
                wireFrameColors.Add(color);
            }

            var parentNode = Global.Renderer.ActiveScene.FindNode(targetObject);
            WireFrameAttribute material = new WireFrameAttribute(
                targetObject.Name + ": WireFrame",
                targetObject.VertexBuffer.ShallowCopy(),
                targetObject.Shader,
                wireFrameColors.ToArray(),
                lineIndex.ToArray());

            targetObject.Attributes.Add(material);
            scene.AddObject(material, parentNode);

            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public override CommandResult Undo(CommandArgsBase commandArg)
        {
            return CommandResult.Failed;
        }
    }


    /// <summary>
    /// ワイヤフレームコマンド
    /// </summary>
    public class WireFrameCommandArgs : CommandArgsBase
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
        /// カラー
        /// </summary>
        public Vector3 Color { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="scene">シーン</param>
        /// <param name="color">カラー</param>
        public WireFrameCommandArgs(RenderObject targetNode, Scene scene, Vector3 color)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
            this.Color = color;
        }
    }
}
