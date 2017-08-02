using System;
using KI.Analyzer.Algorithm.MarchingCube;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Renderer;
using RenderApp.Globals;

namespace RenderApp.RACommand
{
    /// <summary>
    /// Marching Cube の作成
    /// </summary>
    class CreateMarchingCubeCommand : CreateModelCommandBase, ICommand
    {
        private int Partition = 0;

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
            RenderObject marchingObject = RenderObjectFactory.Instance.CreateRenderObject("Marching Sphere");
            GeometryInfo info = new GeometryInfo(marching.PositionList, null, marching.ColorList, null, null, GeometryType.Triangle);
            marchingObject.SetGeometryInfo(info);
            Workspace.SceneManager.ActiveScene.AddObject(marchingObject);
            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
