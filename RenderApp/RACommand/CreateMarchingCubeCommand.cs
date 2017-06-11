using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.KIAsset;
using KI.Gfx.Analyzer.Algorithm.MarchingCube;
using RenderApp.AssetModel;
using System;

namespace RenderApp.RACommand
{
    class CreateMarchingCubeCommand : CreateModelCommandBase, ICommand
    {
        int Partition = 0;
        public CreateMarchingCubeCommand(KIObject asset, int partition)
        {
            Partition = partition;
        }
        public string CanExecute(string commandArg)
        {
            return RACommandResource.Success;
        }

        public string Execute(string commandArg)
        {
            var marching = new MarchingCubesAlgorithm(200, 50);
            RenderObject marchingObject = AssetFactory.Instance.CreateRenderObject("Marching Sphere");
            GeometryInfo info = new GeometryInfo(marching.PositionList, null, marching.ColorList, null, null, GeometryType.Triangle);
            marchingObject.SetGeometryInfo(info);
            SceneManager.Instance.ActiveScene.AddObject(marchingObject);
            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
