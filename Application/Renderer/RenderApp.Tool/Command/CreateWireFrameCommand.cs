using KI.Asset.Attribute;
using KI.Renderer;
using KI.Foundation.Command;
using OpenTK;
using RenderApp.Model;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// ワイヤフレームの作成
    /// </summary>
    public class CreateWireFrameCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private WireFrameCommandArgs wireCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CreateWireFrameCommand(WireFrameCommandArgs commandArgs)
        {
            wireCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(wireCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Execute()
        {
            var targetObject = wireCommandArgs.TargetObject;

            var parentNode = Workspace.Instance.RenderSystem.ActiveScene.FindNode(targetObject);
            WireFrameAttribute attribute = new WireFrameAttribute(
                targetObject.Name + ": WireFrame",
                targetObject.VertexBuffer.ShallowCopy(),
                wireCommandArgs.Color);

            targetObject.Attributes.Add(attribute);

            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Undo()
        {
            return CommandResult.Failed;
        }
    }

    /// <summary>
    /// ワイヤフレームコマンド
    /// </summary>
    public class WireFrameCommandArgs
    {
        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        public AnalyzePolygonNode TargetObject { get; private set; }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// カラー
        /// </summary>
        public Vector4 Color { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="scene">シーン</param>
        /// <param name="color">カラー</param>
        public WireFrameCommandArgs(AnalyzePolygonNode targetNode, Scene scene, Vector4 color)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
            this.Color = color;
        }
    }
}
