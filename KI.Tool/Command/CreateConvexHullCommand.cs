using System;
using System.Collections.Generic;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Renderer;
using OpenTK.Graphics.OpenGL;

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
        /// シーン
        /// </summary>
        private Scene scene;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateConvexHullCommand(Scene scene, KIObject asset)
        {
            this.scene = scene;
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
            ConvexHullAlgorithm convexHull = new ConvexHullAlgorithm(renderObject.Polygon.Vertexs);
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

            Polygon polygon = new Polygon("ConvexHull:" + renderObject.Name, meshs, PrimitiveType.Triangles);
            RenderObject convex = RenderObjectFactory.Instance.CreateRenderObject("ConvexHull :" + renderObject.Name);
            convex.SetPolygon(polygon);
            convex.ModelMatrix = renderObject.ModelMatrix;
            scene.AddObject(convex);

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
