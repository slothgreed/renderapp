using System;
using System.Collections.Generic;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// Convexhullの作成コマンド
    /// </summary>
    public class CreateConvexHullCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateConvexHullCommand(KIObject asset)
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
            ConvexHullAlgorithm convexHull = new ConvexHullAlgorithm(renderObject.Geometry.Position);
            List<Vector3> position = new List<Vector3>();
            foreach (var mesh in convexHull.Meshs)
            {
                Vector3 pos0 = Vector3.Zero;
                Vector3 pos1 = Vector3.Zero;
                Vector3 pos2 = Vector3.Zero;
                foreach (var vertex in mesh.AroundVertex)
                {
                    if (pos0 == Vector3.Zero)
                    {
                        pos0 = vertex.Position;
                        continue;
                    }

                    if (pos1 == Vector3.Zero)
                    {
                        pos1 = vertex.Position;
                        continue;
                    }

                    if (pos2 == Vector3.Zero)
                    {
                        pos2 = vertex.Position;
                        continue;
                    }
                }

                position.Add(pos0);
                position.Add(pos1);

                position.Add(pos1);
                position.Add(pos2);

                position.Add(pos2);
                position.Add(pos0);
            }

            Geometry info = new Geometry("ConvexHull:" + renderObject.Name, position, null, Vector3.UnitZ, null, null, GeometryType.Line);
            RenderObject convex = RenderObjectFactory.Instance.CreateRenderObject("ConvexHull :" + renderObject.Name);
            convex.SetGeometryInfo(info);
            convex.ModelMatrix = renderObject.ModelMatrix;
            Global.RenderSystem.ActiveScene.AddObject(convex);

            RenderObject point = RenderObjectFactory.Instance.CreateRenderObject("ConvexHull : Points" + renderObject.Name);
            Geometry info2 = new Geometry("ConvexHull:Points" + renderObject.Name, convexHull.Points, null, null, null, null, GeometryType.Point);
            point.SetGeometryInfo(info2);
            point.ModelMatrix = renderObject.ModelMatrix;
            Global.RenderSystem.ActiveScene.AddObject(point);

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
