using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.AssetModel.RA_Geometry
{
    class GridPlane : IRenderObjectConverter
    {
        /// <summary>
        /// グリッドの範囲
        /// </summary>
        public float Area
        {
            get;
            set;
        }
        /// <summary>
        /// グリッドの幅
        /// </summary>
        public float Space
        {
            get;
            set;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GridPlane(string name,float area,float space)
        {
            Area = area;
            Space = space;
        }


        public List<RenderObject> CreateRenderObject()
        {
            List<Vector3> Position = new List<Vector3>();
            List<Vector3> Color = new List<Vector3>();

            Vector3 line_start1 = new Vector3();
            Vector3 line_fin1 = new Vector3();
            Vector3 line_start2 = new Vector3();
            Vector3 line_fin2 = new Vector3();
            float world = Area;

            for (float i = -world; i < world; i+=Space)
            {
                if (i != 0)
                {
                    line_start1 = new Vector3(-world, 0, i);
                    line_fin1 = new Vector3(world, 0, i);

                    line_start2 = new Vector3(i, 0, -world);
                    line_fin2 = new Vector3(i, 0, world);

                    Position.Add(line_start1);
                    Position.Add(line_fin1);
                    Position.Add(line_start2);
                    Position.Add(line_fin2);
                    Color.Add(Vector3.One);
                    Color.Add(Vector3.One);
                    Color.Add(Vector3.One);
                    Color.Add(Vector3.One);
                }
            }
            RenderObject geometry = new RenderObject("gridPlane");
            geometry.CreatePC(Position, Color, PrimitiveType.Triangles);
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
