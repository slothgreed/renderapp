﻿using KI.Renderer;

namespace CADApp.Model.Assembly
{
    public class Sketch : SceneNode
    {
        PolygonNode pointObject;

        PolygonNode lineObject;

        PolygonNode polygonObject;

        public Sketch(string name)
           : base(name)
        {

        }

        /// <summary>
        /// 点・線・ポリゴンのレンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(Scene scene)
        {
            pointObject.Render(scene);

            lineObject.Render(scene);

            polygonObject.Render(scene);
        }
    }
}
