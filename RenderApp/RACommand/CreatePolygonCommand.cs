using System;
using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;
using RenderApp.Globals;

namespace RenderApp.RACommand
{
    class CreatePolygonCommand : CreateModelCommandBase, ICommand
    {
        Geometry geometry;
        public CreatePolygonCommand(KIObject asset)
        {
            geometry = asset as Geometry;
        }

        public string CanExecute(string commandArg)
        {
            return CanCreateGeometry(geometry);
        }

        public string Execute(string commandArg)
        {
            List<Vector3> position = new List<Vector3>(geometry.GeometryInfo.Position);
            List<Vector3> normal = new List<Vector3>(geometry.GeometryInfo.Normal);
            List<int> index = new List<int>(geometry.GeometryInfo.Index);
            RenderObject polygon = RenderObjectFactory.Instance.CreateRenderObject("Polygon :" + geometry.Name);
            polygon.SetGeometryInfo(new GeometryInfo(position, normal, new Vector3(0.7f, 0.7f, 0.7f), null, index, GeometryType.Triangle));
            Workspace.SceneManager.ActiveScene.AddObject(polygon);

            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
