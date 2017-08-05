using System;
using KI.Analyzer.Algorithm.MarchingCube;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Renderer;
using RenderApp.Globals;

namespace RenderApp.RACommand
{
    /// <summary>
    /// Marching Cube の作成
    /// </summary>
    public class CreateMarchingCubeCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 分割数
        /// </summary>
        private int partition = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        /// <param name="part">分割数</param>
        public CreateMarchingCubeCommand(KIObject asset, int part)
        {
            partition = part;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public string CanExecute(string commandArg)
        {
            return RACommandResource.Success;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public string Execute(string commandArg)
        {
            var marching = new MarchingCubesAlgorithm(200, 50);
            RenderObject marchingObject = RenderObjectFactory.Instance.CreateRenderObject("Marching Sphere");
            Geometry info = new Geometry(marching.PositionList, null, marching.ColorList, null, null, GeometryType.Triangle);
            marchingObject.SetGeometryInfo(info);
            Workspace.SceneManager.ActiveScene.AddObject(marchingObject);
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
