using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.GfxUtility;
using KI.Gfx.KIShader;
using KI.Gfx.KIAsset;
namespace RenderApp.AssetModel
{
    /// <summary>
    /// 任意形状(triangle,quad,line,patchのみ対応)
    /// </summary>
    public class RenderObject : Geometry
    {
        private void Initialize()
        {
            GenBuffer();

            string vert = ShaderCreater.Instance.GetVertexShader(this);
            string frag = ShaderCreater.Instance.GetFragShader(this);
            this.Shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RenderObject(string name)
            :base(name)
        {
        }

        public void CreateGeometryInfo(GeometryInfo info, PrimitiveType prim)
        {
            geometryInfo.Position = info.Position;
            geometryInfo.Normal = info.Normal;
            geometryInfo.Color = info.Color;
            geometryInfo.TexCoord = info.TexCoord;
            geometryInfo.Index = info.Index;

            for (int i = 0; i < geometryInfo.Position.Count; i++)
            {
                geometryInfo.Color.Add(Vector3.UnitY);
            }
            if (geometryInfo.Normal != null)
            {
                if (geometryInfo.Normal.Count == 0)
                {
                    CalcNormal(geometryInfo.Position, RenderType);
                }
            }

            RenderType = prim;
            Initialize();
        }

        public void CreatePNC(List<Vector3> position, List<Vector3> normal, List<Vector3> color, PrimitiveType prim)
        {
            geometryInfo.Position = new List<Vector3>(position);
            geometryInfo.Normal = new List<Vector3>(normal);
            geometryInfo.Color = new List<Vector3>(color);
            RenderType = prim;
            Initialize();
        }
        internal void CreatePNC(List<Vector3> position, List<Vector3> normal, Vector3 color, PrimitiveType prim)
        {
            geometryInfo.Position = new List<Vector3>(position);
            geometryInfo.Normal = new List<Vector3>(normal);
            RenderType = prim;
            for (int i = 0; i < geometryInfo.Position.Count; i++)
            {
                geometryInfo.Color.Add(Vector3.UnitY);
            } 
            Initialize();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public void CreateP(List<Vector3> position, PrimitiveType prim)
        {
            geometryInfo.Position = new List<Vector3>(position);
            RenderType = prim;
            CalcNormal(geometryInfo.Position, prim);
            Initialize();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public void CreatePN(List<Vector3> position, List<Vector3> normal, PrimitiveType prim)
        {
            geometryInfo.Position = new List<Vector3>(position);
            geometryInfo.Normal = new List<Vector3>(normal);
            RenderType = prim;
            for (int i = 0; i < geometryInfo.Position.Count; i++)
            {
                geometryInfo.Color.Add(Vector3.UnitY);
            }
            Initialize();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public void CreatePNC(string name, List<Vector3> position, List<Vector3> normal, Vector3 color, PrimitiveType prim)
        {
            geometryInfo.Position = new List<Vector3>(position);
            geometryInfo.Normal = new List<Vector3>(normal);
            RenderType = prim;
            for (int i = 0; i < geometryInfo.Position.Count; i++)
            {
                geometryInfo.Color.Add(color);
            }
            Initialize();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public void CreatePNC(string name, List<Vector3> position, List<Vector3> normal, List<Vector3> color, PrimitiveType prim)
        {
            geometryInfo.Position = new List<Vector3>(position);
            geometryInfo.Normal = new List<Vector3>(normal);
            geometryInfo.Color = new List<Vector3>(color);
            RenderType = prim;
            Initialize();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public void CreatePC(List<Vector3> position, Vector3 color, PrimitiveType prim)
        {
            geometryInfo.Position = position;
            RenderType = prim;
            for (int i = 0; i < geometryInfo.Position.Count; i++)
            {
                geometryInfo.Color.Add(color);
            }
            Initialize();
        }
        internal void CreatePC(List<Vector3> position, List<Vector3> color, PrimitiveType prim)
        {
            geometryInfo.Position = new List<Vector3>(position);
            geometryInfo.Color = new List<Vector3>(color);
            RenderType = prim;
            Initialize();
        }
        public void CreatePNT(List<Vector3> position, List<Vector3> normal,List<Vector2> texcoord,PrimitiveType prim)
        {
            geometryInfo.Position = position;
            geometryInfo.Normal = normal;
            if (geometryInfo.Normal.Count == 0)
            {
                CalcNormal(position, prim);
            }
            RenderType = prim;
            geometryInfo.TexCoord = texcoord;
            Initialize();
        }

        public void CreatePT(List<Vector3> position, List<Vector2> texcoord, PrimitiveType prim)
        {
            geometryInfo.Position = position;
            geometryInfo.TexCoord = texcoord;
            RenderType = prim;
            CalcNormal(position, prim);
            Initialize();
        }
        private void CalcNormal(List<Vector3> position, PrimitiveType prim)
        {
            if (PrimitiveType.Triangles == prim)
            {
                for (int i = 0; i < position.Count; i += 3)
                {
                    Vector3 normal = Vector3.Cross(position[i + 2] - position[i + 1], position[i] - position[i + 1]).Normalized();
                    geometryInfo.Normal.Add(normal);
                    geometryInfo.Normal.Add(normal);
                    geometryInfo.Normal.Add(normal);
                }
            }
            else
            {
                for (int i = 0; i < position.Count; i += 4)
                {
                    Vector3 normal = Vector3.Cross(position[i + 2] - position[i + 1], position[i] - position[i + 1]).Normalized();
                    geometryInfo.Normal.Add(normal);
                    geometryInfo.Normal.Add(normal);
                    geometryInfo.Normal.Add(normal);
                    geometryInfo.Normal.Add(normal);
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
            geometryInfo.Position.AddRange(addVertex);
            CalcNormal(addVertex,RenderType);
            foreach(var position in addVertex)
            {
                geometryInfo.Color.Add(_color);
            }
        }
    }
}
