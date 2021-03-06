﻿using System.Collections.Generic;
using System.Linq;
using KI.Asset.Attribute;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Renderer;

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
            var polygonAttribute = new PolygonAttribute("Polygon", VertexBuffer, Polygon.Material);
            Attributes.Add(polygonAttribute);
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
        /// <param name="renderInfo">レンダリング情報</param>
        public override void RenderCore(Scene scene, RenderInfo renderInfo)
        {

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

                ShaderHelper.InitializeState(scene, this, attribute.VertexBuffer, attribute.Material);
                attribute.Render();

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
