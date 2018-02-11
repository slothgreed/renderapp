﻿using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// ハーフエッジのワイヤフレーム作成
    /// </summary>
    public class CreateHalfEdgeWireFrameCommand : CreateModelCommandBase, ICommand
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
        public CreateHalfEdgeWireFrameCommand(Scene scene, KIObject asset)
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
            if (renderObject == null)
            {
                return CommandResult.Failed;
            }

            if (renderObject.Polygon is HalfEdgeDS)
            {
                return CanCreatePolygon(renderObject);
            }

            return CommandResult.Failed;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Execute(string commandArg)
        {
            List<Vector3> position = new List<Vector3>();
            var color = new List<Vector3>();
            List<Line> lines = new List<Line>();
            var halfEdgeDS = renderObject.Polygon as HalfEdgeDS;
            foreach (var mesh in halfEdgeDS.HalfEdgeMeshs)
            {
                foreach (var edge in mesh.AroundEdge)
                {
                    var start = (edge.Start.Position - mesh.Gravity) * 0.8f;
                    var end = (edge.End.Position - mesh.Gravity) * 0.8f;

                    position.Add(start + mesh.Gravity);
                    position.Add(end + mesh.Gravity);

                    color.Add(Vector3.UnitZ);
                    color.Add(Vector3.UnitZ);

                    lines.Add(
                        new Line(
                            new Vertex(2 * lines.Count, start + mesh.Gravity, Vector3.UnitZ), 
                            new Vertex(2 * lines.Count + 1, end + mesh.Gravity, Vector3.UnitZ)));
                }
            }

            var polygon = new Polygon("HalfEdgeWireFrame :" + renderObject.Name, lines);
            RenderObject wireframe = RenderObjectFactory.Instance.CreateRenderObject("HalfEdgeWireFrame :" + renderObject.Name, polygon);
            wireframe.ModelMatrix = renderObject.ModelMatrix;
            scene.AddObject(wireframe);

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
