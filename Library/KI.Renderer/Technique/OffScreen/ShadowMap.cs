﻿using KI.Gfx.Render;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// シャドウマップ
    /// </summary>
    public class ShadowMap : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vertexShader">頂点シェーダファイル</param>
        /// <param name="fragShader">フラグメントシェーダファイル</param>
        public ShadowMap(RenderSystem renderer, string vertexShader, string fragShader)
            : base("ShadowMap", renderer, vertexShader, fragShader,  RenderType.Forward)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget();
            foreach (var node in scene.RootNode.AllChildren())
            {
                if (node is PolygonNode)
                {
                    var polygon = node as PolygonNode;
                    var old = polygon.Polygon.Material.Shader;
                    polygon.Polygon.Material.Shader = Rectanle.Polygon.Material.Shader;
                    polygon.Render(scene);
                    polygon.Polygon.Material.Shader = old;
                }
            }

            RenderTarget.UnBindRenderTarget();
        }
    }
}
