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
        PolygonNode pointObject;

        PolygonNode lineObject;

        PolygonNode polygonObject;

        VertexBuffer vertexBuffer;

        List<Vertex> vertexs;

        bool currentEdit = false;
             
        public Sketch(string name)
           : base(name)
        {
            vertexs = new List<Vertex>();
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
    }
}
