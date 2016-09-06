﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace RenderApp.AssetModel
{
    /// <summary>
    /// 任意形状(triangle,quad,line,patchのみ対応)
    /// </summary>
    public class Primitive : Geometry
    {

        public string m_Name = "";
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Primitive(string name,List<Vector3> position,List<Vector3> normal,List<Vector3> color,PrimitiveType prim)
            :base(name,prim)
        {
            Position = new List<Vector3>(position);
            Normal = new List<Vector3>(normal);
            Color = new List<Vector3>(color);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Primitive(string name, List<Vector3> position, PrimitiveType prim)
            : base(name, prim)
        {
            Position = new List<Vector3>(position);
            CalcNormal(Position, prim);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Primitive(string name, List<Vector3> position, List<Vector3> normal, PrimitiveType prim)
            : base(name, prim)
        {
            Position = new List<Vector3>(position);
            Normal = new List<Vector3>(normal);
            for (int i = 0; i < Position.Count; i++)
            {
                Color.Add(Vector3.UnitY);
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Primitive(string name, List<Vector3> position, List<Vector3> normal, Vector3 color, PrimitiveType prim)
            : base(name, prim)
        {
            Position = new List<Vector3>(position);
            Normal = new List<Vector3>(normal);
            for (int i = 0; i < Position.Count; i++)
            {
                Color.Add(color);
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Primitive(string name, List<Vector3> position, Vector3 color, PrimitiveType prim)
            : base(name, prim)
        {
            Position = position;
            CalcNormal(Position, prim); 
            for (int i = 0; i < Position.Count; i++)
            {
                Color.Add(color);
            }
        }
        public Primitive(string name,List<Vector3> position, List<Vector3> normal,List<Vector2> texcoord,PrimitiveType prim = PrimitiveType.Triangles)
            :base(name,prim)
        {
            Position = position;
            Normal = normal;
            TexCoord = texcoord;
        }
        public Primitive(string name, List<Vector3> position, List<Vector2> texcoord, PrimitiveType prim)
            : base(name, prim)
        {
            //TODO:Normal
            Position = position;
            TexCoord = texcoord;
            CalcNormal(Position,prim);
            for (int i = 0; i < Position.Count; i++)
            {
                Color.Add(Vector3.UnitY);
            }
        }
        private void CalcNormal(List<Vector3> position,PrimitiveType prim)
        {
            if(PrimitiveType.Triangles == prim)
            {
                for(int i = 0; i < position.Count; i+=3)
                {
                    Vector3 normal = Vector3.Cross(position[0] - position[1], position[2] - position[1]);
                    Normal.Add(normal);
                    Normal.Add(normal);
                    Normal.Add(normal);
                }
            }else{
                for (int i = 0; i < position.Count; i+=4)
                {
                    Vector3 normal = Vector3.Cross(position[0] - position[1], position[2] - position[1]).Normalized();
                    Normal.Add(normal);
                    Normal.Add(normal);
                    Normal.Add(normal);
                    Normal.Add(normal);
                }
            }
        }
        public void AddVertex(List<Vector3> addVertex, Vector3 _color)
        {
            switch(RenderType)
            {
                case PrimitiveType.Triangles:
                    if(addVertex.Count % 3 != 0)
                    {
                        return;
                    }
                    break;
                case PrimitiveType.Quads:
                    if (addVertex.Count % 4 != 0)
                    {
                        return;
                    }
                    break;
                case PrimitiveType.Lines:
                    if (addVertex.Count % 2 != 0)
                    {
                        return;
                    }
                    break;
            }
            Position.AddRange(addVertex);
            CalcNormal(addVertex,RenderType);
            foreach(var position in addVertex)
            {
                Color.Add(_color);
            }
        }
        
    }
}
