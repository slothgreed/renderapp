using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.KIAsset;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.AssetModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.RACommand
{
    class CreatePolygonCommand : CreateModelCommandBase, ICommand
    {
        Geometry geometry;
        public CreatePolygonCommand(KIObject asset)
        {
            geometry = asset as Geometry;
        }
        public string CanExecute()
        {
            return CanCreateGeometry(geometry);
        }

        public string Execute()
        {
            List<Vector3> position = new List<Vector3>(geometry.geometryInfo.Position);
            List<Vector3> normal = new List<Vector3>(geometry.geometryInfo.Normal);
            RenderObject polygon = AssetFactory.Instance.CreateRenderObject("Polygon :" + geometry.Name);
            polygon.CreateGeometryInfo(new GeometryInfo(position, normal, new Vector3(0.7f, 0.7f, 0.7f), null, null, GeometryType.Triangle), PrimitiveType.Triangles);
            SceneManager.Instance.ActiveScene.AddObject(polygon);

            return RACommandResource.Success;
        }

        public string Undo()
        {
            throw new NotImplementedException();
        }
    }
}
