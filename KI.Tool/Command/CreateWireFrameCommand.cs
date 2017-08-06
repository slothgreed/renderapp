using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// ワイヤフレームの作成
    /// </summary>
    public class CreateWireFrameCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateWireFrameCommand(KIObject asset)
        {
            renderObject = asset as RenderObject;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState CanExecute(string commandArg)
        {
            return CanCreateGeometry(renderObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Execute(string commandArg)
        {
            List<Vector3> position = new List<Vector3>();
            if (renderObject.Geometry.GeometryInfo.Index.Count != 0)
            {
                for (int i = 0; i < renderObject.Geometry.GeometryInfo.Index.Count / 3; i++)
                {
                    position.Add(renderObject.Geometry.GeometryInfo.Position[renderObject.Geometry.GeometryInfo.Index[3 * i]]);
                    position.Add(renderObject.Geometry.GeometryInfo.Position[renderObject.Geometry.GeometryInfo.Index[3 * i + 1]]);

                    position.Add(renderObject.Geometry.GeometryInfo.Position[renderObject.Geometry.GeometryInfo.Index[3 * i + 1]]);
                    position.Add(renderObject.Geometry.GeometryInfo.Position[renderObject.Geometry.GeometryInfo.Index[3 * i + 2]]);

                    position.Add(renderObject.Geometry.GeometryInfo.Position[renderObject.Geometry.GeometryInfo.Index[3 * i + 2]]);
                    position.Add(renderObject.Geometry.GeometryInfo.Position[renderObject.Geometry.GeometryInfo.Index[3 * i]]);
                }
            }
            else
            {
                for (int i = 0; i < renderObject.Geometry.GeometryInfo.Position.Count / 3; i++)
                {
                    position.Add(renderObject.Geometry.GeometryInfo.Position[3 * i]);
                    position.Add(renderObject.Geometry.GeometryInfo.Position[3 * i + 1]);

                    position.Add(renderObject.Geometry.GeometryInfo.Position[3 * i + 1]);
                    position.Add(renderObject.Geometry.GeometryInfo.Position[3 * i + 2]);

                    position.Add(renderObject.Geometry.GeometryInfo.Position[3 * i + 2]);
                    position.Add(renderObject.Geometry.GeometryInfo.Position[3 * i]);
                }
            }

            RenderObject wireframe = RenderObjectFactory.Instance.CreateRenderObject("WireFrame :" + renderObject.Name);
            wireframe.SetGeometryInfo(new Geometry("WireFrame :" + renderObject.Name, position, null, KICalc.RandomColor(), null, null, GeometryType.Line));
            wireframe.ModelMatrix = renderObject.ModelMatrix;
            Global.Scene.AddObject(wireframe);

            return CommandState.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Undo(string commandArg)
        {
            return CommandState.Failed;
        }
    }
}
