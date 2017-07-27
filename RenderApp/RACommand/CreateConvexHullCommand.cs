using KI.Foundation.Command;
using KI.Foundation.Core;
using RenderApp.AssetModel;
using System;
using System.Collections.Generic;
using OpenTK;
using KI.Asset;
using KI.Analyzer.Algorithm;
using KI.Gfx.GLUtil;
using RenderApp.Globals;

namespace RenderApp.RACommand
{
    class CreateConvexHullCommand : CreateModelCommandBase, ICommand
    {
        Geometry geometry;
        public CreateConvexHullCommand(KIObject asset)
        {
            geometry = asset as Geometry;
        }
        public string CanExecute(string commandArg)
        {
            return CanCreateGeometry(geometry);
        }

        public string Execute(string commandArg)
        {
            ConvexHullAlgorithm convexHull = new ConvexHullAlgorithm(geometry.geometryInfo.Position);
            List<Vector3> position = new List<Vector3>();
            foreach(var mesh in convexHull.Meshs)
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

            GeometryInfo info = new GeometryInfo(position, null, Vector3.UnitZ, null, null, GeometryType.Line);
            RenderObject convex = AssetFactory.Instance.CreateRenderObject("ConvexHull :" + geometry.Name);
            convex.SetGeometryInfo(info);
            convex.ModelMatrix = geometry.ModelMatrix;
            Workspace.SceneManager.ActiveScene.AddObject(convex);

            RenderObject point = AssetFactory.Instance.CreateRenderObject("ConvexHull : Points" + geometry.Name);
            GeometryInfo info2 = new GeometryInfo(convexHull.Points, null, null, null, null, GeometryType.Point);
            point.SetGeometryInfo(info2);
            point.ModelMatrix = geometry.ModelMatrix;
            Workspace.SceneManager.ActiveScene.AddObject(point);

            return RACommandResource.Success;
        }
        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
