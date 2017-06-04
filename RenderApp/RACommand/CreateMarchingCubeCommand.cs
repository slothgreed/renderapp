using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.Analyzer;
using KI.Gfx.KIAsset;
using RenderApp.AssetModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.Analyzer.Algorithm;

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
            var marching = new MarchingCubesAlgorithm(100, 64);
            RenderObject marchingObject = AssetFactory.Instance.CreateRenderObject("Marching Sphere");
            GeometryInfo info = new GeometryInfo(marching.PositionList, null, null, null, null, GeometryType.Triangle);
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
