using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.AssetModel
{
    class Axis : VertexInfo
    {
        public Axis(string name,Vector3 min, Vector3 max)
        {
            SetObjectData(min, max);
        }
        private void SetObjectData(Vector3 min, Vector3 max)
        {
            Position.Add(new Vector3(max.X, 0.0f, 0.0f));
            Position.Add(new Vector3(0.0f, 0.0f, 0.0f));
            Position.Add(new Vector3(0.0f, max.Y, 0.0f));
            Position.Add(new Vector3(0.0f, 0.0f, 0.0f));
            Position.Add(new Vector3(0.0f, 0.0f, max.Z));
            Position.Add(new Vector3(0.0f, 0.0f, 0.0f));

            Color.Add(new Vector3(1, 0, 0));
            Color.Add(new Vector3(1, 0, 0));
            Color.Add(new Vector3(0, 1, 0));
            Color.Add(new Vector3(0, 1, 0));
            Color.Add(new Vector3(0, 0, 1));
            Color.Add(new Vector3(0, 0, 1));

        }
    }
}
