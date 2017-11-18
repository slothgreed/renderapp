using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    /// <summary>
    /// レンダリングパッケージ
    /// </summary>
    public class RenderPackage
    {

        public RenderPackage(Polygon polygon, PrimitiveType type)
        {
            VertexBuffer = new VertexBuffer();
            Type = type;
        }

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; }

        /// <summary>
        /// シェーダ
        /// </summary>
        public Shader Shader { get; set; }

        /// <summary>
        /// レンダリングするときの種類
        /// </summary>
        public PrimitiveType Type { get; set; }

        /// <summary>
        /// 色情報
        /// </summary>
        public List<Vector3> Color { get; set; }

        /// <summary>
        /// レンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="package">レンダリング情報</param>
        public void RenderPackageCore(IScene scene, RenderObject renderObject)
        {
            if (Shader == null)
            {
                Logger.Log(Logger.LogLevel.Error, "not set shader");
                return;
            }

            if (VertexBuffer.Num == 0)
            {
                Logger.Log(Logger.LogLevel.Error, "vertexs list is 0");
                return;
            }

            ShaderHelper.InitializeState(scene, renderObject, this, renderObject.Polygon.Textures);
            Shader.BindBuffer();
            if (VertexBuffer.IndexBuffer.ContainsKey(Type))
            {
                DeviceContext.Instance.DrawElements(Type, VertexBuffer.Num, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                DeviceContext.Instance.DrawArrays(Type, 0, VertexBuffer.Num);
            }

            Shader.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
    }

}
