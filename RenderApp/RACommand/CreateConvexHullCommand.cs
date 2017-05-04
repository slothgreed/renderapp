using KI.Foundation.Command;
using KI.Foundation.Core;
using RenderApp.AssetModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.Analyzer.Algorithm;
using KI.Gfx.KIAsset;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace RenderApp.RACommand
{
    class CreateConvexHullCommand : CreateModelCommandBase, ICommand
    {
        Geometry geometry;
        public CreateConvexHullCommand(KIObject asset)
        {
            geometry = asset as Geometry;
        }
        public string CanExecute()
        {
            return CanCreateGeometry(geometry);
        }

        public string Execute()
        {
            ConvexHullAlgorithm convexHull = new ConvexHullAlgorithm(geometry.geometryInfo.Position);
            algorithm = convexHull;
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
            
            GeometryInfo info = new GeometryInfo(position,null,Vector3.UnitZ,null,null,GeometryType.Line);
            RenderObject convex = AssetFactory.Instance.CreateRenderObject("ConvexHull :" + geometry.Name);
            convex.CreateGeometryInfo(info, PrimitiveType.Lines);
            convex.ModelMatrix = geometry.ModelMatrix;
            SceneManager.Instance.ActiveScene.AddObject(convex);
            renderObject = convex;


            RenderObject point = AssetFactory.Instance.CreateRenderObject("ConvexHull : Points" + geometry.Name);
            GeometryInfo info2 = new GeometryInfo(convexHull.Points, null, null, null, null, GeometryType.Point);
            point.CreateGeometryInfo(info2, PrimitiveType.Points);
            point.ModelMatrix = geometry.ModelMatrix;
            SceneManager.Instance.ActiveScene.AddObject(point);
            pointObject = point;

            return RACommandResource.Success;
        }
        static RenderObject pointObject;
        static RenderObject renderObject;
        static ConvexHullAlgorithm algorithm;

        public static void Update()
        {
            algorithm.Update();
            List<Vector3> position = new List<Vector3>();
            List<Vector3> normal = new List<Vector3>();
            List<Vector3> color = new List<Vector3>();
            bool First = true;
            foreach (var mesh in algorithm.Meshs)
            {
                foreach (var vertex in mesh.AroundVertex)
                {
                    position.Add(vertex.Position);
                    normal.Add(mesh.Normal);
            
                    if(First && !(bool)mesh.CalcFlag)
                    {
                        First = false;
                        color.Add(Vector3.UnitY);
                    }
                    else
                    {
                        color.Add(Vector3.UnitZ);

                    }
                }
            }

            renderObject.geometryInfo.Update(position, normal, color, null, null, GeometryType.Triangle);
            renderObject.SetupBuffer();

            pointObject.geometryInfo.Update(algorithm.Points, null, null, null, null, GeometryType.Point);
            pointObject.SetupBuffer();
        }
        public string Undo()
        {
            throw new NotImplementedException();
        }
    }
}
