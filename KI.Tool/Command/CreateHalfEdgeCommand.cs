using System;
using System.Linq;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Renderer;
using OpenTK.Graphics.OpenGL;

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
            return CanCreatePolygon(renderObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Execute(string commandArg)
        {
            var half = AssetFactory.Instance.CreateHalfEdge("HalfEdge :" + renderObject.Name,renderObject.Polygon.Vertexs.Select(p => p.Position).ToList(), renderObject.Polygon.Index[PrimitiveType.Triangles]);
            RenderObject halfEdge = RenderObjectFactory.Instance.CreateRenderObject("HalfEdge :" + renderObject.Name);
            halfEdge.SetPolygon(half.Polygons[0]);
            halfEdge.ModelMatrix = renderObject.ModelMatrix;
            Global.RenderSystem.ActiveScene.AddObject(halfEdge);

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
