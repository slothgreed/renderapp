using System;
using System.Collections.Generic;
using System.Linq;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Renderer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
        private RenderObject renderObject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreatePolygonCommand(KIObject asset)
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
            var polygon =  renderObject.Polygon;
            List<Vertex> vertex = new List<Vertex>();
            List<int> index = new List<int>(polygon.Index[PrimitiveType.Triangles]);
            RenderObject polygonObject = RenderObjectFactory.Instance.CreateRenderObject("Polygon :" + polygon.Name);
            var color = new Vector3(0.7f);
            for (int i = 0; i < polygon.Vertexs.Count; i++)
            {
                vertex.Add(new Vertex(i, polygon.Vertexs[i].Position, polygon.Vertexs[i].Normal, color));
            }

            polygonObject.SetPolygon(new Polygon("Polygon :" + polygon.Name, vertex, index, PrimitiveType.Triangles));
            Global.RenderSystem.ActiveScene.AddObject(polygon);

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
