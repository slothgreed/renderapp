using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// 曲率の算出コマンド
    /// </summary>
    public class CalculateVertexCurvature : ICommand
    {
        private RenderObject renderObject;

        public CalculateVertexCurvature(KIObject asset)
        {
            renderObject = asset as RenderObject;
        }

        public CommandResult CanExecute(string commandArg)
        {
            if(renderObject == null)
            {
                return CommandResult.Failed;
            }

            if(renderObject.Geometry.HalfEdgeDS == null)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
        }

        public CommandResult Execute(string commandArg)
        {
            var vertexInfo = new VertexCurvatureAlgorithm(renderObject.Geometry.HalfEdgeDS);
            var poly = renderObject.Geometry.HalfEdgeDS;

            List<Vector3> position = poly.Vertexs.Select(p => p.Position).ToList();
            List<Vector3> voronoi = new List<Vector3>();
            List<Vector3> mean = new List<Vector3>();
            List<Vector3> gauss = new List<Vector3>();
            List<Vector3> min = new List<Vector3>();
            List<Vector3> max = new List<Vector3>();

            var voronoiParam = vertexInfo.Parameters[VertexParam.Voronoi] as ScalarParameter;
            var meanParam = vertexInfo.Parameters[VertexParam.MeanCurvature] as ScalarParameter;
            var gaussParam = vertexInfo.Parameters[VertexParam.GaussCurvature] as ScalarParameter;
            var minParam = vertexInfo.Parameters[VertexParam.MaxCurvature] as ScalarParameter;
            var maxParam = vertexInfo.Parameters[VertexParam.MinCurvature] as ScalarParameter;

            for (int i = 0; i < poly.Vertexs.Count; i++)
            {
                voronoi.Add(KICalc.GetPseudoColor((float)voronoiParam.GetValue(i), voronoiParam.Min, voronoiParam.Max));
                mean.Add(KICalc.GetPseudoColor((float)meanParam.GetValue(i), meanParam.Min, meanParam.Max));
                gauss.Add(KICalc.GetPseudoColor((float)gaussParam.GetValue(i), gaussParam.Min, gaussParam.Max));
                min.Add(KICalc.GetPseudoColor((float)minParam.GetValue(i), minParam.Min, minParam.Max));
                max.Add(KICalc.GetPseudoColor((float)maxParam.GetValue(i), maxParam.Min, maxParam.Max));
            }

            AddObject("voronoi", position, voronoi);
            AddObject("mean", position, mean);
            AddObject("gauss", position, gauss);
            AddObject("min", position, min);
            AddObject("max", position, max);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }

        private void AddObject(string name, List<Vector3> position, List<Vector3> color)
        {
            Geometry geometry = new Geometry(name + " : " + renderObject.Name, position, null, color, null, null, GeometryType.Point);
            RenderObject render = RenderObjectFactory.Instance.CreateRenderObject(name);
            render.SetGeometryInfo(geometry);
            render.ModelMatrix = renderObject.ModelMatrix;
            Global.RenderSystem.ActiveScene.AddObject(render);
        }

    }
}
