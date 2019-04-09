using System.Collections.Generic;
using KI.Asset.Attribute;
using KI.Renderer;
using KI.Tool.Command;
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
            var scene = wireCommandArgs.Scene;
            var color = wireCommandArgs.Color;

            List<int> lineIndex = new List<int>();
            List<Vector3> wireFrameColors = new List<Vector3>();

            //if (targetObject.Polygon.Index.Count != 0)
            //{
            //    foreach (var index in targetObject.Polygon.Index)
            //    {
            //        lineIndex.Add(index);
            //        wireFrameColors.Add(color);
            //    }

            //    for (int i = 0; i < targetObject.Polygon.Index.Count / 3; i++)
            //    {
            //        lineIndex.Add(targetObject.Polygon.Index[3 * i]);
            //        lineIndex.Add(targetObject.Polygon.Index[3 * i + 1]);

            //        lineIndex.Add(targetObject.Polygon.Index[3 * i + 1]);
            //        lineIndex.Add(targetObject.Polygon.Index[3 * i + 2]);

            //        lineIndex.Add(targetObject.Polygon.Index[3 * i + 2]);
            //        lineIndex.Add(targetObject.Polygon.Index[3 * i]);

            //        wireFrameColors.Add(color);
            //        wireFrameColors.Add(color);
            //        wireFrameColors.Add(color);
            //    }
            //}
            //else
            {
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
            }

            var parentNode = Workspace.Instance.RenderSystem.ActiveScene.FindNode(targetObject);
            WireFrameAttribute material = new WireFrameAttribute(
                targetObject.Name + ": WireFrame",
                targetObject.VertexBuffer.ShallowCopy(),
                targetObject.Shader,
                wireFrameColors.ToArray(),
                lineIndex.ToArray());

            targetObject.Attributes.Add(material);

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
        public Vector3 Color { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="scene">シーン</param>
        /// <param name="color">カラー</param>
        public WireFrameCommandArgs(AnalyzePolygonNode targetNode, Scene scene, Vector3 color)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
            this.Color = color;
        }
    }
}
