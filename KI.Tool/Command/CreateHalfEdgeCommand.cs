using System;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Renderer;

namespace KI.Tool.Command
{
    /// <summary>
    /// ハーフエッジの作成
    /// </summary>
    public class CreateHalfEdgeCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateHalfEdgeCommand(KIObject asset)
        {
            renderObject = asset as RenderObject;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult CanExecute(string commandArg)
        {
            return CanCreateGeometry(renderObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Execute(string commandArg)
        {
            var half = AssetFactory.Instance.CreateHalfEdge(renderObject.Geometry.Position, renderObject.Geometry.Index);
            RenderObject halfEdge = RenderObjectFactory.Instance.CreateRenderObject("HalfEdge :" + renderObject.Name);
            halfEdge.SetGeometryInfo(half.Geometrys[0]);
            halfEdge.ModelMatrix = renderObject.ModelMatrix;
            Global.Scene.AddObject(halfEdge);

            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
