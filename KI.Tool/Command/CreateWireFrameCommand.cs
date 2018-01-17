using System.Collections.Generic;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Renderer;
using KI.Renderer.Attribute;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
        /// ワイヤフレーム色
        /// </summary>
        private Vector3 wireFrameColor;

        /// <summary>
        /// シーン
        /// </summary>
        private Scene scene;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateWireFrameCommand(Scene scene, KIObject asset)
        {
            renderObject = asset as RenderObject;
            wireFrameColor = Vector3.Zero;
            this.scene = scene;
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
            List<int> lineIndex = new List<int>();
            List<Vector3> wireFrameColors = new List<Vector3>();
            foreach (var mesh in renderObject.Polygon.Meshs)
            {
                for (int j = 0; j < mesh.Vertexs.Count - 1; j++)
                {
                    lineIndex.Add(mesh.Vertexs[j].Index);
                    lineIndex.Add(mesh.Vertexs[j + 1].Index);
                }

                lineIndex.Add(mesh.Vertexs[mesh.Vertexs.Count - 1].Index);
                lineIndex.Add(mesh.Vertexs[0].Index);
                wireFrameColors.Add(wireFrameColor);
                wireFrameColors.Add(wireFrameColor);
            }

            var parentNode = Global.Renderer.ActiveScene.FindNode(renderObject);
            WireFrameAttribute material = new WireFrameAttribute(
                renderObject.Name + ": WireFrame",
                renderObject.PolygonAttribute.VertexBuffer.ShallowCopy(),
                renderObject.Shader,
                wireFrameColors.ToArray(),
                lineIndex.ToArray());

            renderObject.Attributes.Add(material);
            scene.AddObject(material, parentNode);

            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Undo(string commandArg)
        {
            return CommandResult.Failed;
        }
    }
}
