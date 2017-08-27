﻿using System;
using System.Linq;
using KI.Asset;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    /// <summary>
    /// 任意形状(triangle,quad,line,patchのみ対応)
    /// </summary>
    public class RenderObject : SceneNode
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
        public RenderObject(string name, Geometry info)
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
        /// 形状
        /// </summary>
        public Geometry Geometry { get; private set; }

        /// <summary>
        /// シェーダ
        /// </summary>
        public Shader Shader { get; set; }
        
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(IScene scene)
        {
            if (Shader == null)
            {
                Logger.Log(Logger.LogLevel.Error, "not set shader");
                return;
            }

            ShaderHelper.InitializeState(scene, Shader, this, Geometry.Textures);
            Shader.BindBuffer();
            if (Geometry.Index.Count == 0)
            {
                DeviceContext.Instance.DrawArrays(Geometry.GeometryType, 0, Geometry.Vertexs.Count);
            }
            else
            {
                DeviceContext.Instance.DrawElements(Geometry.GeometryType, Geometry.Index.Count, DrawElementsType.UnsignedInt, 0);
            }

            Shader.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// 形状をセット
        /// </summary>
        /// <param name="geometry">形状情報</param>
        public void SetGeometryInfo(Geometry geometry)
        {
            geometry.GeometryUpdate -= UpdateGeometry;
            Geometry = geometry;
            Geometry.GeometryUpdate += UpdateGeometry;

            Initialize();
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            BufferFactory.Instance.RemoveByValue(PositionBuffer);
            BufferFactory.Instance.RemoveByValue(ColorBuffer);
            BufferFactory.Instance.RemoveByValue(TexCoordBuffer);
            BufferFactory.Instance.RemoveByValue(NormalBuffer);
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
            if (Geometry.Vertexs.Count != 0)
            {
                PositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                PositionBuffer.GenBuffer();
                NormalBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                NormalBuffer.GenBuffer();
                ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                ColorBuffer.GenBuffer();
                TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                TexCoordBuffer.GenBuffer();
            }

            if (Geometry.Index.Count != 0)
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
                PositionBuffer.SetData(Geometry.Vertexs.Select(p => p.Position).ToList(), EArrayType.Vec3Array);
                NormalBuffer.SetData(Geometry.Vertexs.Select(p => p.Normal).ToList(), EArrayType.Vec3Array);
                ColorBuffer.SetData(Geometry.Vertexs.Select(p => p.Color).ToList(), EArrayType.Vec3Array);
                TexCoordBuffer.SetData(Geometry.Vertexs.Select(p => p.TexCoord).ToList(), EArrayType.Vec2Array);
            }

            if (Geometry.Index.Count != 0)
            {
                if (IndexBuffer == null)
                {
                    IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
                    IndexBuffer.GenBuffer();
                }

                IndexBuffer.SetData(Geometry.Index, EArrayType.IntArray);
            }
        }

        private void UpdateGeometry(object sender, EventArgs e)
        {
            SetupBuffer();
        }
    }
}
