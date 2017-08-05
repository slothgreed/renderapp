using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    /// <summary>
    /// 任意形状(triangle,quad,line,patchのみ対応)
    /// </summary>
    public class RenderObject : Geometry
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public RenderObject(string name)
            : base(name)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="info">形状</param>
        public RenderObject(string name, GeometryInfo info)
            : base(name)
        {
            SetGeometryInfo(info);
        }

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

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
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
            if (GeometryInfo.Index.Count == 0)
            {
                DeviceContext.Instance.DrawArrays(GeometryInfo.GeometryType, 0, GeometryInfo.Position.Count);
            }
            else
            {
                DeviceContext.Instance.DrawElements(GeometryInfo.GeometryType, GeometryInfo.Index.Count, DrawElementsType.UnsignedInt, 0);
            }

            Shader.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// 形状をセット
        /// </summary>
        /// <param name="info">形状情報</param>
        public void SetGeometryInfo(GeometryInfo info)
        {
            GeometryInfo = info;
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            GenBuffer();

            string vert = ShaderCreater.Instance.GetVertexShader(this);
            string frag = ShaderCreater.Instance.GetFragShader(this);
            this.Shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
        }

        /// <summary>
        /// バッファの作成
        /// </summary>
        private void GenBuffer()
        {
            if (GeometryInfo.Position.Count != 0)
            {
                PositionBuffer = BufferFactory.Instance.CreateArrayBuffer();
                PositionBuffer.GenBuffer();
            }

            if (GeometryInfo.Normal.Count != 0)
            {
                NormalBuffer = BufferFactory.Instance.CreateArrayBuffer();
                NormalBuffer.GenBuffer();
            }

            if (GeometryInfo.Color.Count != 0)
            {
                ColorBuffer = BufferFactory.Instance.CreateArrayBuffer();
                ColorBuffer.GenBuffer();
            }

            if (GeometryInfo.TexCoord.Count != 0)
            {
                TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer();
                TexCoordBuffer.GenBuffer();
            }

            if (GeometryInfo.Index.Count != 0)
            {
                IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
                IndexBuffer.GenBuffer();
            }

            SetupBuffer();
        }

        /// <summary>
        /// バッファにデータの設定
        /// </summary>
        private void SetupBuffer()
        {
            if (PositionBuffer != null)
            {
                PositionBuffer.SetData(GeometryInfo.Position, EArrayType.Vec3Array);
            }

            if (GeometryInfo.Normal.Count != 0)
            {
                NormalBuffer.SetData(GeometryInfo.Normal, EArrayType.Vec3Array);
            }

            if (GeometryInfo.Color.Count != 0)
            {
                ColorBuffer.SetData(GeometryInfo.Color, EArrayType.Vec3Array);
            }

            if (GeometryInfo.TexCoord.Count != 0)
            {
                TexCoordBuffer.SetData(GeometryInfo.TexCoord, EArrayType.Vec2Array);
            }

            if (GeometryInfo.Index.Count != 0)
            {
                if (IndexBuffer == null)
                {
                    IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
                    IndexBuffer.GenBuffer();
                }

                IndexBuffer.SetData(GeometryInfo.Index, EArrayType.IntArray);
            }
        }
    }
}
