using System;
using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// ポリゴンの作成
    /// </summary>
    public class CreatePolygonCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private Geometry geometry;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreatePolygonCommand(KIObject asset)
        {
            geometry = asset as Geometry;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState CanExecute(string commandArg)
        {
            return CanCreateGeometry(geometry);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Execute(string commandArg)
        {
            List<Vector3> position = new List<Vector3>(geometry.GeometryInfo.Position);
            List<Vector3> normal = new List<Vector3>(geometry.GeometryInfo.Normal);
            List<int> index = new List<int>(geometry.GeometryInfo.Index);
            RenderObject polygon = RenderObjectFactory.Instance.CreateRenderObject("Polygon :" + geometry.Name);
            polygon.SetGeometryInfo(new Geometry("Polygon :" + geometry.Name, position, normal, new Vector3(0.7f, 0.7f, 0.7f), null, index, GeometryType.Triangle));
            Global.Scene.AddObject(polygon);

            return CommandState.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
