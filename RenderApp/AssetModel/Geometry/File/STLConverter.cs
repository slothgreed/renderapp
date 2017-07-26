using System.Collections.Generic;
using KI.Asset;
using KI.Asset.Loader;
using KI.Gfx.GLUtil;

namespace RenderApp.AssetModel.RA_Geometry
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class STLConverter : IRenderObjectConverter
    {
        STLLoader stlData;

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="position"></param>
        public STLConverter(string name,string filePath)
        {
            stlData = new STLLoader(name, filePath);
        }

        public List<RenderObject> CreateRenderObject()
        {
            RenderObject geometry = AssetFactory.Instance.CreateRenderObject(stlData.FileName);
            GeometryInfo info = new GeometryInfo(stlData.vertexInfo.Position, stlData.vertexInfo.Normal, null, null, null,GeometryType.Triangle);
            geometry.SetGeometryInfo(info);
            _renderObject = new List<RenderObject>() { geometry };
            return _renderObject;
        }
        private List<RenderObject> _renderObject;
        public List<RenderObject> RenderObject
        {
            get
            {
                return _renderObject;
            }
        }
    }
}
