using System;
using System.Collections.Generic;
using KI.Asset.Attribute;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Renderer;
using OpenTK;

namespace CADApp.Model.Assembly
{
    public class SketchNode : SceneNode
    {
        /// <summary>
        /// GL_POINTS のノード
        /// </summary>
        PolygonNode sketchNode;

        /// <summary>
        /// GL_LINES の Attribute
        /// </summary>
        PolygonAttribute lineAttribute;

        /// <summary>
        /// GL_TRIANGLES の Attribute
        /// </summary>
        PolygonAttribute polygonAttribute;

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        VertexBuffer vertexBuffer;

        /// <summary>
        /// スケッチの頂点情報
        /// </summary>
        List<Vertex> vertexs;

        bool currentEdit = false;
             
        public SketchNode(string name)
           : base(name)
        {
            vertexs = new List<Vertex>();
            sketchNode = new PolygonNode(name + " : Point");
            lineAttribute = new PolygonAttribute(name + " : Line", PolygonType.Lines);
            polygonAttribute = new PolygonAttribute(name + " : Mesh", PolygonType.Triangles);
        }

        public void AddVertex(Vector3 position)
        {
            if (currentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs.Add(new Vertex(vertexs.Count, position, Vector3.UnitX));
        }

        public void SetVertex(int index, Vector3 position)
        {
            if (currentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs[index].Position = position;
        }


        public void RemoveVertex(int index)
        {
            if (currentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs.RemoveAt(index);
        }


        public void ClearVertex()
        {
            if (currentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs.Clear();
        }

        public void BeginEdit()
        {
            currentEdit = true;
        }

        public void EndEdit()
        {
            currentEdit = false;
            UpdatePolygonData();
        }

        /// <summary>
        /// 点・線・ポリゴンのレンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(Scene scene)
        {
            if (currentEdit == true)
            {
                throw new Exception();
            }

            sketchNode.Render(scene);
        }


        private void UpdatePolygonData()
        {

        }
    }
}
