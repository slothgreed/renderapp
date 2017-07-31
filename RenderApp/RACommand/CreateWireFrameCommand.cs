using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using RenderApp.AssetModel;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using KI.Asset;
using KI.Gfx.GLUtil;
using RenderApp.Globals;
using KI.Renderer;

namespace RenderApp.RACommand
{
    class CreateWireFrameCommand : CreateModelCommandBase, ICommand
    {
        private Geometry geometry;

        public CreateWireFrameCommand(KIObject asset)
        {
            geometry = asset as Geometry;

        }
        public string CanExecute(string commandArg)
        {
            return CanCreateGeometry(geometry);
        }

        public string Execute(string commandArg)
        {
            List<Vector3> position = new List<Vector3>();
            if (geometry.geometryInfo.Index.Count != 0)
            {
                for (int i = 0; i < geometry.geometryInfo.Index.Count / 3; i++)
                {
                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i]]);
                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i + 1]]);

                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i + 1]]);
                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i + 2]]);

                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i + 2]]);
                    position.Add(geometry.geometryInfo.Position[geometry.geometryInfo.Index[3 * i]]);
                }
            }
            else
            {
                for (int i = 0; i < geometry.geometryInfo.Position.Count / 3; i++)
                {
                    position.Add(geometry.geometryInfo.Position[3 * i]);
                    position.Add(geometry.geometryInfo.Position[3 * i + 1]);

                    position.Add(geometry.geometryInfo.Position[3 * i + 1]);
                    position.Add(geometry.geometryInfo.Position[3 * i + 2]);

                    position.Add(geometry.geometryInfo.Position[3 * i + 2]);
                    position.Add(geometry.geometryInfo.Position[3 * i]);

                }
            }
            RenderObject wireframe = RenderObjectFactory.Instance.CreateRenderObject("WireFrame :" + geometry.Name);
            wireframe.SetGeometryInfo(new GeometryInfo(position, null, KICalc.RandomColor(), null, null, GeometryType.Line));
            wireframe.ModelMatrix = geometry.ModelMatrix;
            Workspace.SceneManager.ActiveScene.AddObject(wireframe);

            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            return RACommand.RACommandResource.Failed;
        }
    }
}
