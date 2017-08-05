using System;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Renderer;
using RenderApp.AssetModel;
using RenderApp.Globals;

namespace RenderApp.RACommand
{
    class CreateHalfEdgeCommand : CreateModelCommandBase, ICommand
    {
        private Geometry geometry;

        public CreateHalfEdgeCommand(KIObject asset)
        {
            geometry = asset as Geometry;
        }

        public string CanExecute(string commandArg)
        {
            return CanCreateGeometry(geometry);
        }

        public string Execute(string commandArg)
        {
            var half = AssetFactory.Instance.CreateHalfEdge(geometry.GeometryInfo.Position, geometry.GeometryInfo.Index);
            RenderObject halfEdge = RenderObjectFactory.Instance.CreateRenderObject("HalfEdge :" + geometry.Name);
            halfEdge.SetGeometryInfo(half.GeometryInfos[0]);
            halfEdge.ModelMatrix = geometry.ModelMatrix;
            Workspace.SceneManager.ActiveScene.AddObject(halfEdge);

            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
