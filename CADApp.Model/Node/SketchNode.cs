﻿using System;
using System.Linq;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Renderer;
using OpenTK.Graphics.OpenGL;

namespace CADApp.Model.Node
{
    public class AssemblyNode : SceneNode
    {
        /// <summary>
        /// 頂点バッファ
        /// </summary>
        VertexBuffer vertexBuffer;

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        VertexBuffer controlPointBuffer;

        /// <summary>
        /// 稜線のインデックスバッファ
        /// </summary>
        VertexBuffer lineBuffer;

        /// <summary>
        /// Triangle のインデックスバッファ
        /// </summary>
        VertexBuffer triangleBuffer;

        Shader shader;

        /// <summary>
        /// スケッチの頂点情報
        /// </summary>
        public Assembly Assembly { get; private set; }

        public bool VisibleVertex { get; set; } = true;
        public bool VisibleLine { get; set; } = true;
        public bool VisibleTriangle { get; set; } = true;
        public bool VisibleControlPoint { get; set; } = true;

        public AssemblyNode(string name, Assembly assmebly, Shader _shader)
           : base(name)
        {
            shader = _shader;
            Assembly = assmebly;
            assmebly.AssemblyUpdated += OnAssemblyUpdated;
            GenerateBuffer();
        }

        private void GenerateBuffer()
        {
            vertexBuffer = new VertexBuffer();
            controlPointBuffer = new VertexBuffer();
            lineBuffer = vertexBuffer.ShallowCopy();
            lineBuffer.IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            triangleBuffer = vertexBuffer.ShallowCopy();
            triangleBuffer.IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
        }


        /// <summary>
        /// 点・線・ポリゴンのレンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(Scene scene)
        {
            if (Assembly.CurrentEdit == true)
            {
                throw new Exception();
            }

            if (VisibleVertex)
            {
                Draw(scene, PolygonType.Points, vertexBuffer);
            }

            if (VisibleControlPoint)
            {
                Draw(scene, PolygonType.Points, controlPointBuffer);
            }

            if (VisibleLine)
            {
                Draw(scene, PolygonType.Lines, lineBuffer);
            }

            if (VisibleTriangle)
            {
                Draw(scene, PolygonType.Triangles, triangleBuffer);
            }
        }

        private void Draw(Scene scene, PolygonType type, VertexBuffer buffer)
        {
            ShaderHelper.InitializeState(shader, scene, this, buffer, null);
            shader.BindBuffer();
            if (buffer.EnableIndexBuffer)
            {
                DeviceContext.Instance.DrawElements(type, buffer.Num, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                DeviceContext.Instance.DrawArrays(type, 0, buffer.Num);
            }

            shader.UnBindBuffer();
        }

        public override void Dispose()
        {
            vertexBuffer.Dispose();
            controlPointBuffer.Dispose();
            BufferFactory.Instance.RemoveByValue(lineBuffer.IndexBuffer);
            BufferFactory.Instance.RemoveByValue(triangleBuffer.IndexBuffer);
            Assembly.AssemblyUpdated -= OnAssemblyUpdated;
            base.Dispose();
        }

        private void OnAssemblyUpdated(object sender, EventArgs e)
        {
            if (Assembly.Vertex != null &&
                Assembly.Vertex.Count > 0)
            {
                vertexBuffer.SetBuffer(Assembly.Vertex.ToArray(), Enumerable.Range(0, Assembly.Vertex.Count).ToArray());
            }

            if (Assembly.ControlPoint != null &&
                Assembly.ControlPoint.Count > 0)
            {
                controlPointBuffer.SetBuffer(Assembly.ControlPoint.ToArray(), Enumerable.Range(0, Assembly.ControlPoint.Count).ToArray());
            }

            if (Assembly.LineIndex != null &&
                Assembly.LineIndex.Count > 1)
            {
                lineBuffer.SetIndexArray(Assembly.LineIndex.ToArray());
            }
            if (Assembly.TriangleIndex != null &&
                Assembly.TriangleIndex.Count > 2)
            {
                triangleBuffer.SetIndexArray(Assembly.TriangleIndex.ToArray());
            }
        }
    }
}
