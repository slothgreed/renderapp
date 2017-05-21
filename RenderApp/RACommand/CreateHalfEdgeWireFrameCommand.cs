using KI.Foundation.Command;
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
            if(geometry.HalfEdge == null)
            {
                return RACommandResource.Failed;
            }
            return CanCreateGeometry(geometry);
        }

        public string Execute(string commandArg)
        {
            List<Vector3> position = new List<Vector3>();
            foreach (var mesh in geometry.HalfEdge.m_Mesh)
            {
                foreach (var edge in mesh.AroundEdge)
                {
                    var start = (edge.Start.Position - mesh.Gravity) * 0.8f;
                    var end = (edge.End.Position - mesh.Gravity) * 0.8f;
                    position.Add(start + mesh.Gravity);
                    position.Add(end + mesh.Gravity);
                }
            }

            GeometryInfo info = new GeometryInfo(position, null, Vector3.UnitX, null, null, GeometryType.Line);
            RenderObject convex = AssetFactory.Instance.CreateRenderObject("HalfEdgeWireFrame :" + geometry.Name);
            convex.SetGeometryInfo(info);
            convex.ModelMatrix = geometry.ModelMatrix;
            SceneManager.Instance.ActiveScene.AddObject(convex);

            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
