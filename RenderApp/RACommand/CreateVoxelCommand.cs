﻿using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using OpenTK;
using System;
using System.Collections.Generic;
using KI.Asset;
using KI.Analyzer;
using KI.Gfx.GLUtil;
using RenderApp.Globals;
using KI.Renderer;

namespace RenderApp.RACommand
{
    class CreateVoxelCommand : CreateModelCommandBase, ICommand
    {
        Geometry geometry = null;
        int Partition = 0;
        public CreateVoxelCommand(KIObject asset, int partition)
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
            VoxelSpace voxel = new VoxelSpace(geometry.geometryInfo.Position, geometry.geometryInfo.Index, Partition);
            RenderObject voxelObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel :" + geometry.Name);
            GeometryInfo info = new GeometryInfo(voxel.vPosition, voxel.vNormal, KICalc.RandomColor(), null, null, GeometryType.Quad);
            voxelObject.SetGeometryInfo(info);
            voxelObject.Transformation(geometry.ModelMatrix);
            Workspace.SceneManager.ActiveScene.AddObject(voxelObject);

            RenderObject innerObject = RenderObjectFactory.Instance.CreateRenderObject("Voxel Inner : " + geometry.Name);


            var voxels = voxel.GetVoxel(VoxelSpace.VoxelState.Inner);
            var colors = new List<Vector3>();
            foreach (var v in voxels)
            {
                colors.Add(KICalc.GetPseudoColor(v.Value, 0, 100));
            }

            GeometryInfo innerInfo = new GeometryInfo(voxel.GetPoint(VoxelSpace.VoxelState.Inner), null, colors, null, null, GeometryType.Point);
            innerObject.SetGeometryInfo(innerInfo);
            innerObject.Transformation(geometry.ModelMatrix);
            Workspace.SceneManager.ActiveScene.AddObject(innerObject);

            return RACommandResource.Success;
        }

        public string Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
