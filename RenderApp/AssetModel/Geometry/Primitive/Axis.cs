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
    class Axis : IRenderObject
    {
        public Vector3 Min
        {
            get;
            set;
        }
        public Vector3 Max
        {
            get;
            set;
        }
        public Axis(string name,Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public List<RenderObject> CreateRenderObject()
        {
            List<Vector3> Position = new List<Vector3>();
            List<Vector3> Color = new List<Vector3>();
            Position.Add(new Vector3(Max.X, 0.0f, 0.0f));
            Position.Add(new Vector3(Min.X, 0.0f, 0.0f));
            Position.Add(new Vector3(0.0f, Max.Y, 0.0f));
            Position.Add(new Vector3(0.0f, Min.Y, 0.0f));
            Position.Add(new Vector3(0.0f, 0.0f, Max.Z));
            Position.Add(new Vector3(0.0f, 0.0f, Min.Z));

            Color.Add(new Vector3(1, 0, 0));
            Color.Add(new Vector3(1, 0, 0));
            Color.Add(new Vector3(0, 1, 0));
            Color.Add(new Vector3(0, 1, 0));
            Color.Add(new Vector3(0, 0, 1));
            Color.Add(new Vector3(0, 0, 1));
            _renderObject = new List<RenderObject>() { new RenderObject("axis", Position, Color, PrimitiveType.Lines) };
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
