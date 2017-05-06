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

namespace RenderApp.RACommand
{
    class CreateVoxelCommand : CreateModelCommandBase, ICommand
    {
        Geometry geometry = null;
        int Partition = 0;
        public CreateVoxelCommand(KIObject asset,int partition)
        {
            geometry = asset as Geometry;
            Partition = partition;
        }
        public string CanExecute(string commandArg)
        {
            return CanCreateGeometry(geometry);
        }

        public string Execute(string commandArg)
        {
            Voxel voxel = new KI.Gfx.Analyzer.Voxel(geometry.geometryInfo.Position, geometry.geometryInfo.Index, geometry.ModelMatrix, Partition);
            RenderObject voxelObject = AssetFactory.Instance.CreateRenderObject("Voxel :" + geometry.Name);
            GeometryInfo info = new GeometryInfo(voxel.vPosition, voxel.vNormal, KICalc.RandomColor(), null, null, GeometryType.Quad);
            voxelObject.SetGeometryInfo(info);
            voxelObject.Transformation(geometry.ModelMatrix);
            SceneManager.Instance.ActiveScene.AddObject(voxelObject);
            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
