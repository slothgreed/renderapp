using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.Analyzer;
using KI.Gfx.KIAsset;
using RenderApp.AssetModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.RACommand
{
    class CreateHalfEdgeCommand : CreateModelCommandBase, ICommand
    {
        Geometry geometry;
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
            HalfEdge half = new HalfEdge(geometry.geometryInfo);
            GeometryInfo info = half.CreateGeometryInfo();
            RenderObject halfEdge = AssetFactory.Instance.CreateRenderObject("HalfEdge :" + geometry.Name);
            halfEdge.SetGeometryInfo(info);
            halfEdge.ModelMatrix = geometry.ModelMatrix;
            SceneManager.Instance.ActiveScene.AddObject(halfEdge);

            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
