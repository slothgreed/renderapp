using System;
using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
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
        private Geometry geometry;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateHalfEdgeWireFrameCommand(KIObject asset)
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
            if (geometry == null)
            {
                return CommandState.Failed;
            }

            if (geometry.HalfEdge == null)
            {
                return CommandState.Failed;
            }

            return CanCreateGeometry(geometry);
        }

        private static Geometry info2;
        private static int counter = -1;
        private static RenderObject obj;

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Execute(string commandArg)
        {
            if (counter > -1)
            {
                //geometry.HalfEdge.VertexDecimation(geometry.HalfEdge.m_Edge[counter]);
                geometry.HalfEdge.EdgeFlips(geometry.HalfEdge.Edges[counter]);
            }

            counter++;
            List<Vector3> position = new List<Vector3>();
            var color = new List<Vector3>();
            foreach (var mesh in geometry.HalfEdge.Meshs)
            {
                foreach (var edge in mesh.AroundEdge)
                {
                    var start = (edge.Start.Position - mesh.Gravity) * 0.8f;
                    var end = (edge.End.Position - mesh.Gravity) * 0.8f;

                    position.Add(start + mesh.Gravity);
                    position.Add(end + mesh.Gravity);

                    //position.Add(start + mesh.Gravity + mesh.Normal * 0.1f);
                    //position.Add(end + mesh.Gravity + mesh.Normal * 0.1f);

                    //if (geometry.HalfEdge.m_Edge[counter].Mesh.Index == mesh.Index
                    //    || geometry.HalfEdge.m_Edge[counter].Opposite.Mesh.Index == mesh.Index)
                    //{
                    //    if (edge.Index == geometry.HalfEdge.m_Edge[counter].Index)
                    //    {
                    //        color.Add(Vector3.UnitX + Vector3.UnitY);
                    //        color.Add(Vector3.UnitX + Vector3.UnitY);
                    //    }
                    //    else
                    //    {
                    //        color.Add(Vector3.UnitX);
                    //        color.Add(Vector3.UnitX);
                    //    }
                    //}
                    //else
                    //{
                    //    color.Add(Vector3.UnitZ);
                    //    color.Add(Vector3.UnitZ);
                    //}

                    //if (edge.Start.Index == geometry.HalfEdge.m_Edge[0].Start.Index && edge.End.Index == geometry.HalfEdge.m_Edge[0].End.Index)
                    //{
                    //    color.Add(Vector3.UnitZ);
                    //    color.Add(Vector3.UnitZ);
                    //}
                    //else
                    //{
                    //    color.Add(Vector3.UnitX);
                    //    color.Add(Vector3.UnitX);
                    //}

                    if (edge == geometry.HalfEdge.Edges[counter])
                    {
                        color.Add(Vector3.UnitY);
                        color.Add(Vector3.UnitX);
                    }
                    else
                    {
                        color.Add(Vector3.UnitZ);
                        color.Add(Vector3.UnitZ);
                    }
                }
            }

            //if(info2 != null)
            //{
            //    info2.Color = color;
            //    info2.Position = position;
            //    info2.Update(position, info2.Normal, info2.Color, null, null, GeometryType.Line);
            //    obj.SetupBuffer();

            //    var update = ((HalfEdge)geometry.HalfEdge).CreateGeometryInfo();
            //    geometry.geometryInfo.Update(update.Position, update.Normal, update.Color, null, update.Index, GeometryType.Triangle);
            //    ((RenderObject)geometry).SetupBuffer();

            //    return "Success";
            //}
            //GeometryInfo info = new GeometryInfo(position, null, color, null, null, GeometryType.Line);
            //RenderObject halfWire = RenderObjectFactory.Instance.CreateRenderObject("HalfEdgeWireFrame :" + geometry.Name);
            //halfWire.SetGeometryInfo(info);
            //halfWire.ModelMatrix = geometry.ModelMatrix;
            //Global.Scene.AddObject(halfWire);
            //info2 = info;
            //obj = halfWire;

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
