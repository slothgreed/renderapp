using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using KI.Foundation.Core;
using KI.Gfx.KIGeometry;
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
            RenderObject geometry = new RenderObject(stlData.FileName);
            geometry.CreatePN(stlData.vertexInfo.Position, stlData.vertexInfo.Normal, PrimitiveType.Triangles);
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
