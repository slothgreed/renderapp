﻿using KI.Gfx.Render;

namespace KI.Renderer
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
        public ShadowMap(string vertexShader, string fragShader)
            : base("ShadowMap", vertexShader, fragShader, RenderTechniqueType.Shadow, RenderType.Original)
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
        public override void Render(IScene scene)
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture);
            foreach (var asset in scene.RootNode.AllChildren())
            {
                if (asset.KIObject is RenderObject)
                {
                    var polygon = asset.KIObject as RenderObject;
                    var old = polygon.Shader;
                    polygon.Shader = ShaderItem;
                    polygon.Render(scene);
                    polygon.Shader = old;
                }
            }

            RenderTarget.UnBindRenderTarget();
        }
    }
}
