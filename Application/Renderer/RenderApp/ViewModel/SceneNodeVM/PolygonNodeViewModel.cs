using KI.Analyzer;
using KI.Asset;
using KI.Asset.Loader.Importer;
using KI.Gfx;
using KI.Gfx.KIShader;
using KI.Presenter.ViewModel;
using KI.Renderer;
using RenderApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RenderApp.ViewModel
{
    class PolygonNodeViewModel : SceneNodeViewModel
    {
        public PolygonNodeViewModel(ViewModelBase parent, PolygonNode model)
            : base(parent, model, model.ToString(), Place.RightUp)
        {
            polygonNode = model;
        }

        PolygonNode polygonNode;


        public bool CanConvertHalfEdgeDS
        {
            get
            {
                return polygonNode.Type == KI.Gfx.KIPrimitiveType.Triangles;
            }
        }

        private ICommand _ConvertHalfEdge;
        public ICommand ConvertHalfEdge
        {
            get
            {
                if (_ConvertHalfEdge == null)
                {
                    return _ConvertHalfEdge = CreateCommand(ConvertHalfEdgeCommand);
                }

                return _ConvertHalfEdge;
            }
        }

        private void ConvertHalfEdgeCommand()
        {
            PolygonNode node = Model as PolygonNode;
            var halfEdgeDS = new HalfEdgeDS(Model.Name, 
                node.Polygon.Vertexs.Select(p => p.Position).ToList(),
                node.Polygon.Index);

            var importer = new HalfEdgeImporter(halfEdgeDS);
            string vert = ShaderCreater.Instance.GetVertexShader(importer.Model.Type, null);
            string frag = ShaderCreater.Instance.GetFragShaderFilePath(importer.Model.Type, null);
            var shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
            importer.Model.Material = new Material(shader);
            AnalyzePolygonNode analyze = new AnalyzePolygonNode(importer.Model.Name, importer.Model);
            Workspace.Instance.MainScene.AddObject(analyze);
            Workspace.Instance.MainScene.DeleteObject(polygonNode);
        }
    }
}
