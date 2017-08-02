using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIShader;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.GLUtil;
using KI.Foundation.Utility;
using KI.Asset;

namespace KI.Renderer
{
    /// <summary>
    /// 任意形状(triangle,quad,line,patchのみ対応)
    /// </summary>
    public class RenderObject : Geometry
    {
        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public ArrayBuffer PositionBuffer { get; private set; }

        /// <summary>
        /// 法線バッファ
        /// </summary>
        public ArrayBuffer NormalBuffer { get; private set; }
        
        /// <summary>
        /// カラーバッファ
        /// </summary>
        public ArrayBuffer ColorBuffer { get; private set; }

        /// <summary>
        /// テクスチャ座標バッファ
        /// </summary>
        public ArrayBuffer TexCoordBuffer { get; private set; }

        /// <summary>
        /// 頂点Indexバッファ
        /// </summary>
        public ArrayBuffer IndexBuffer { get; private set; }

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
        /// <param name="name">名前</param>
        public RenderObject(string name)
            : base(name)
        {
        }

        public void SetGeometryInfo(GeometryInfo info)
        {
            geometryInfo = info;

            if (geometryInfo.Index.Count == 0)
            {
                if (geometryInfo.Normal != null)
                {
                    if (geometryInfo.Normal.Count == 0)
                    {
                        CalcNormal(geometryInfo.Position, info.GeometryType);
                    }
                }
            }

            Initialize();
        }

        private void CalcNormal(List<Vector3> position, GeometryType type)
        {
            switch (type)
            {
                case GeometryType.None:
                case GeometryType.Point:
                case GeometryType.Line:
                case GeometryType.Mix:
                    return;
                case GeometryType.Triangle:
                    for (int i = 0; i < position.Count; i += 3)
                    {
                        Vector3 normal = Vector3.Cross(position[i + 2] - position[i + 1], position[i] - position[i + 1]).Normalized();
                        geometryInfo.Normal.Add(normal);
                        geometryInfo.Normal.Add(normal);
                        geometryInfo.Normal.Add(normal);
                    }

                    break;
                case GeometryType.Quad:
                    for (int i = 0; i < position.Count; i += 4)
                    {
                        Vector3 normal = Vector3.Cross(position[i + 2] - position[i + 1], position[i] - position[i + 1]).Normalized();
                        geometryInfo.Normal.Add(normal);
                        geometryInfo.Normal.Add(normal);
                        geometryInfo.Normal.Add(normal);
                        geometryInfo.Normal.Add(normal);
                    }

                    break;
                default:
                    break;
            }
        }

        public void AddVertex(List<Vector3> addVertex, Vector3 color)
        {
            switch (geometryInfo.GeometryType)
            {
                case GeometryType.Triangle:
                    if (addVertex.Count % 3 != 0)
                    {
                        return;
                    }

                    break;
                case GeometryType.Quad:
                    if (addVertex.Count % 4 != 0)
                    {
                        return;
                    }

                    break;
                case GeometryType.Line:
                    if (addVertex.Count % 2 != 0)
                    {
                        return;
                    }

                    break;
            }

            geometryInfo.Position.AddRange(addVertex);
            CalcNormal(addVertex, geometryInfo.GeometryType);
            foreach (var position in addVertex)
            {
                geometryInfo.Color.Add(color);
            }
        }

        #region [render]
        public virtual void Render(Scene scene)
        {
            if (!Visible)
            {
                return;
            }

            if (Shader == null)
            {
                Logger.Log(Logger.LogLevel.Error, "not set shader");
                return;
            }

            ShaderHelper.InitializeState(scene, Shader, this, TextureItem);
            Shader.BindBuffer();
            if (geometryInfo.Index.Count == 0)
            {
                DeviceContext.Instance.DrawArrays(geometryInfo.GeometryType, 0, geometryInfo.Position.Count);
            }
            else
            {
                DeviceContext.Instance.DrawElements(geometryInfo.GeometryType, geometryInfo.Index.Count, DrawElementsType.UnsignedInt, 0);
            }

            Shader.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
        #endregion

        public void SetupBuffer()
        {
            if (PositionBuffer != null)
            {
                PositionBuffer.SetData(geometryInfo.Position, EArrayType.Vec3Array);
            }

            if (geometryInfo.Normal.Count != 0)
            {
                NormalBuffer.SetData(geometryInfo.Normal, EArrayType.Vec3Array);
            }

            if (geometryInfo.Color.Count != 0)
            {
                ColorBuffer.SetData(geometryInfo.Color, EArrayType.Vec3Array);
            }

            if (geometryInfo.TexCoord.Count != 0)
            {
                TexCoordBuffer.SetData(geometryInfo.TexCoord, EArrayType.Vec2Array);
            }

            if (geometryInfo.Index.Count != 0)
            {
                if (IndexBuffer == null)
                {
                    IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
                    IndexBuffer.GenBuffer();
                }

                IndexBuffer.SetData(geometryInfo.Index, EArrayType.IntArray);
            }
        }

        private void GenBuffer()
        {
            if (geometryInfo.Position.Count != 0)
            {
                PositionBuffer = BufferFactory.Instance.CreateArrayBuffer();
                PositionBuffer.GenBuffer();
            }

            if (geometryInfo.Normal.Count != 0)
            {
                NormalBuffer = BufferFactory.Instance.CreateArrayBuffer();
                NormalBuffer.GenBuffer();
            }

            if (geometryInfo.Color.Count != 0)
            {
                ColorBuffer = BufferFactory.Instance.CreateArrayBuffer();
                ColorBuffer.GenBuffer();
            }

            if (geometryInfo.TexCoord.Count != 0)
            {
                TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer();
                TexCoordBuffer.GenBuffer();
            }

            if (geometryInfo.Index.Count != 0)
            {
                IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
                IndexBuffer.GenBuffer();
            }

            SetupBuffer();
        }
    }
}
