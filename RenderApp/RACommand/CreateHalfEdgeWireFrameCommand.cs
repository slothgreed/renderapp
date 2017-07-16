﻿using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Analyzer;
using KI.Gfx.KIAsset;
using OpenTK;
using RenderApp.AssetModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.RACommand
{
    class CreateHalfEdgeWireFrameCommand : CreateModelCommandBase, ICommand
    {
        Geometry geometry;
        public CreateHalfEdgeWireFrameCommand(KIObject asset)
        {
            geometry = asset as Geometry;
        }

        public string CanExecute(string commandArg)
        {
            if (geometry == null)
            {
                return RACommandResource.Failed;
            }
            if(geometry.HalfEdge == null)
            {
                return RACommandResource.Failed;
            }
            return CanCreateGeometry(geometry);
        }
        private static GeometryInfo info2;
        private static int counter = -1;
        private static RenderObject obj;
        public string Execute(string commandArg)
        {
            if(counter > -1)
            {
                //geometry.HalfEdge.VertexDecimation(geometry.HalfEdge.m_Edge[counter]);
                geometry.HalfEdge.EdgeFlips(geometry.HalfEdge.m_Edge[counter]);
            }
            counter++;
            List<Vector3> position = new List<Vector3>();
            var color = new List<Vector3>();
            foreach (var mesh in geometry.HalfEdge.m_Mesh)
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


                    if (edge == geometry.HalfEdge.m_Edge[counter])
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

            if(info2 != null)
            {
                info2.Color = color;
                info2.Position = position;
                info2.Update(position, info2.Normal, info2.Color, null, null, GeometryType.Line);
                obj.SetupBuffer();

                var update = geometry.HalfEdge.CreateGeometryInfo();
                geometry.geometryInfo.Update(update.Position, update.Normal, update.Color, null, update.Index, GeometryType.Triangle);
                ((RenderObject)geometry).SetupBuffer();


                return RACommandResource.Success;
            }
            GeometryInfo info = new GeometryInfo(position, null, color, null, null, GeometryType.Line);
            RenderObject halfWire = AssetFactory.Instance.CreateRenderObject("HalfEdgeWireFrame :" + geometry.Name);
            halfWire.SetGeometryInfo(info);
            halfWire.ModelMatrix = geometry.ModelMatrix;
            SceneManager.Instance.ActiveScene.AddObject(halfWire);
            info2 = info;
            obj = halfWire;

            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
