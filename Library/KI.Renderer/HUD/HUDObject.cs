﻿using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    public class HUDObject : KIObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public HUDObject(Polygon polygon)
            : base(polygon.Name)
        {
            SetPolygon(polygon);
        }

        /// <summary>
        /// 始点座標位置
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// 表示サイズ
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// 奥行値
        /// </summary>
        int Depth;

        /// <summary>
        /// 可視不可視
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; private set; }

        /// <summary>
        /// 形状
        /// </summary>
        public Polygon Polygon
        {
            get;
            private set;
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Render(RenderInfo renderInfo)
        {
            if (Visible == true)
            {
                RenderCore(renderInfo);
            }
        }

        private void RenderCore(RenderInfo renderInfo)
        {
            if (Polygon.Material.Shader == null)
            {
                Logger.Log(Logger.LogLevel.Error, "not set shader");
                return;
            }

            if (VertexBuffer.Num == 0)
            {
                Logger.Log(Logger.LogLevel.Error, "vertexs list is 0");
                return;
            }

            ShaderHelper.InitializeHUD(VertexBuffer, Polygon.Material);
            Polygon.Material.BindToGPU();
            VertexBuffer.Render();
            Polygon.Material.UnBindToGPU();

            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// 形状をセット
        /// </summary>
        /// <param name="polygon">形状情報</param>
        public void SetPolygon(Polygon polygon)
        {
            Polygon = polygon;

            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
            }

            VertexBuffer = new VertexBuffer();

            UpdateVertexBufferObject();
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        public virtual void UpdateVertexBufferObject()
        {
            VertexBuffer.SetBuffer(Polygon.Type, Polygon.Vertexs, Polygon.Index);
        }
    }
}
