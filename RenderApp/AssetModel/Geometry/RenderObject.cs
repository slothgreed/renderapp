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
            RenderType = prim;

            for (int i = 0; i < geometryInfo.Position.Count; i++)
            {
                geometryInfo.Color.Add(Vector3.UnitY);
            }
            if(geometryInfo.Index.Count == 0)
            {
                if (geometryInfo.Normal != null)
                {
                    if (geometryInfo.Normal.Count == 0)
                    {
                        CalcNormal(geometryInfo.Position, RenderType);
                    }
                }
            }

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
