using System.Collections.Generic;
using System.Linq;
using KI.Asset.Attribute;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Renderer;
using OpenTK.Graphics.OpenGL;

namespace RenderApp.Model
{
    /// <summary>
    /// 解析用のポリゴンノード
    /// </summary>
    public class AnalyzePolygonNode : PolygonNode
    {
        public AnalyzePolygonNode(string name, Polygon polygon)
            : base(name, polygon)
        {

        }

        /// <summary>
        /// アトリビュート
        /// アトリビュートのみSRTが違うことはないためAttributeNodeを作成して子ノードにすることはしない。
        /// </summary>
        public List<AttributeBase> Attributes { get; private set; } = new List<AttributeBase>();

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(Scene scene)
        {
            base.RenderCore(scene);

            foreach (var attribute in Attributes.Where(p => p.Visible))
            {
                if (attribute.Material.Shader == null)
                {
                    Logger.Log(Logger.LogLevel.Error, "not set shader");
                    return;
                }

                if (attribute.VertexBuffer.Num == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertexs list is 0");
                    return;
                }

                attribute.Binding();
                ShaderHelper.InitializeState(scene, this, attribute.VertexBuffer, attribute.Material);
                attribute.Material.Shader.BindBuffer();
                if (attribute.VertexBuffer.EnableIndexBuffer)
                {
                    DeviceContext.Instance.DrawElements(attribute.Type, attribute.VertexBuffer.Num, DrawElementsType.UnsignedInt, 0);
                }
                else
                {
                    DeviceContext.Instance.DrawArrays(attribute.Type, 0, attribute.VertexBuffer.Num);
                }

                attribute.Material.Shader.UnBindBuffer();
                attribute.UnBinding();

                Logger.GLLog(Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            foreach (var attribute in Attributes)
            {
                attribute.Dispose();
            }

            base.Dispose();
        }
    }
}
