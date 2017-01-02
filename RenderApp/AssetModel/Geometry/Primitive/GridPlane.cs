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
    class GridPlane : VertexInfo
    {

        /// <summary>
        /// グリッドの範囲
        /// </summary>
        private float m_Area;
        /// <summary>
        /// グリッドの幅
        /// </summary>
        private float m_Space;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GridPlane(string name,float Area,float Space)
        {
            m_Area = Area;
            m_Space = Space;
        }
        

        private void SetObjectData()
        {

            Vector3 line_start1 = new Vector3();
            Vector3 line_fin1 = new Vector3();
            Vector3 line_start2 = new Vector3();
            Vector3 line_fin2 = new Vector3();
            float world = m_Area;

            for (float i = -world; i < world; i+=m_Space)
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
                    Color.Add(new Vector3(1.0f,1.0f,1.0f));
                    Color.Add(new Vector3(1.0f,1.0f,1.0f));
                    Color.Add(new Vector3(1.0f,1.0f,1.0f));
                    Color.Add(new Vector3(1.0f,1.0f,1.0f));
                }
            }
        }

        public void Remake(float Area, float Space)
        {
            m_Area = Area;
            m_Space = Space;
            SetObjectData();
        }

    }
}
