using System;
using System.Collections.Generic;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Tool.Command;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// Convexhullの作成コマンド
    /// </summary>
    public class CreateConvexHullCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private ConvexHullCommandArgs convexCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CreateConvexHullCommand(ConvexHullCommandArgs commandArgs)
        {
            convexCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(convexCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Execute()
        {
            var targetObject = convexCommandArgs.TargetObject;
            var scene = convexCommandArgs.Scene;

            ConvexHullAlgorithm convexHull = new ConvexHullAlgorithm(targetObject.Polygon.Vertexs);
            List<Mesh> meshs = new List<Mesh>();
            foreach (var mesh in convexHull.Meshs)
            {
                Vertex ver0 = null;
                Vertex ver1 = null;
                Vertex ver2 = null;
                foreach (var vertex in mesh.AroundVertex)
                {
                    if (ver0 == null)
                    {
                        ver0 = vertex;
                        continue;
                    }

                    if (ver1 == null)
                    {
                        ver1 = vertex;
                        continue;
                    }

                    if (ver2 == null)
                    {
                        ver2 = vertex;
                        continue;
                    }
                }

                meshs.Add(new Mesh(ver0, ver1, ver2));
            }

            Polygon polygon = new Polygon("ConvexHull:" + targetObject.Name, meshs, PolygonType.Triangles);
            RenderObject convex = RenderObjectFactory.Instance.CreateRenderObject("ConvexHull :" + targetObject.Name, polygon);
            convex.ModelMatrix = targetObject.ModelMatrix;
            scene.AddObject(convex);

            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 曲率コマンド
    /// </summary>
    public class ConvexHullCommandArgs
    {
        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        public RenderObject TargetObject { get; private set; }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="loopNum">ループ回数</param>
        public ConvexHullCommandArgs(RenderObject targetNode, Scene scene)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
        }
    }
}
