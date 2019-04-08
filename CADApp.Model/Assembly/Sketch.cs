using System;
using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Renderer;
using OpenTK;

namespace CADApp.Model.Assembly
{
    public class Sketch : SceneNode
    {
        /// <summary>
        /// GL_POINTS のノード
        /// </summary>
        PolygonNode pointObject;

        /// <summary>
        /// GL_LINES のノード
        /// </summary>
        PolygonNode lineObject;

        /// <summary>
        /// GL_TRIANGLES のノード
        /// </summary>
        PolygonNode polygonObject;

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        VertexBuffer vertexBuffer;

        /// <summary>
        /// スケッチの頂点情報
        /// </summary>
        List<Vertex> vertexs;

        bool currentEdit = false;
             
        public Sketch(string name)
           : base(name)
        {
            vertexs = new List<Vertex>();
            pointObject = new PolygonNode(name + " : Point");
            lineObject = new PolygonNode(name + " : Line");
            polygonObject = new PolygonNode(name + " : Mesh");
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
            if(currentEdit == true)
            {
                throw new Exception();
            }

            pointObject.Render(scene);

            lineObject.Render(scene);

            polygonObject.Render(scene);
        }


        private void UpdatePolygonData()
        {

        }
    }
}
