using System;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Renderer;
using RenderApp.Globals;

namespace RenderApp.RACommand
{
    /// <summary>
    /// ハーフエッジの作成
    /// </summary>
    public class CreateHalfEdgeCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private Geometry geometry;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateHalfEdgeCommand(KIObject asset)
        {
            geometry = asset as Geometry;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public string CanExecute(string commandArg)
        {
            return CanCreateGeometry(geometry);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public string Execute(string commandArg)
        {
            var half = AssetFactory.Instance.CreateHalfEdge(geometry.GeometryInfo.Position, geometry.GeometryInfo.Index);
            RenderObject halfEdge = RenderObjectFactory.Instance.CreateRenderObject("HalfEdge :" + geometry.Name);
            halfEdge.SetGeometryInfo(half.Geometrys[0]);
            halfEdge.Geometry.ModelMatrix = geometry.ModelMatrix;
            Workspace.SceneManager.ActiveScene.AddObject(halfEdge);

            return RACommandResource.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
