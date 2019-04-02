﻿using KI.Gfx.KITexture;

namespace KI.Asset.Technique
{
    /// <summary>
    /// IBL
    /// </summary>
    public class ImageBasedLighting : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageBasedLighting(string vertexShader, string fragShader)
            : base("IBL", vertexShader, fragShader,RenderType.Original)
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget();
            foreach (var probe in scene.RootNode.AllChildren())
            {
                if (probe.KIObject is EnvironmentProbe)
                {
                    EnvironmentProbe env = probe.KIObject as EnvironmentProbe;
                    Rectanle.Polygon.AddTexture(TextureKind.Cubemap, env.Cubemap);
                    Rectanle.Render(scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            var textures = Global.Renderer.RenderQueue.OutputTexture<GBuffer>();
            Rectanle.Polygon.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.OutputTextureType.Color]);
            Rectanle.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Normal]);
            Rectanle.Polygon.AddTexture(TextureKind.World, textures[(int)GBuffer.OutputTextureType.Posit]);
            Rectanle.Polygon.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.OutputTextureType.Light]);
        }

        private Texture _uCubeMap;
        public Texture uCubeMap
        {
            get
            {
                return _uCubeMap;
            }

            set
            {
                SetValue(ref _uCubeMap, value);
            }
        }


    }
}
