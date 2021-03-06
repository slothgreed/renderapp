﻿using KI.Gfx.Buffer;
using KI.Gfx.KITexture;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// GlayScale
    /// </summary>
    public partial class GrayScale : DefferedTechnique
    {
        private TextureBuffer _uTarget;
        public TextureBuffer uTarget
        {
            get
            {
                return _uTarget;
            }

            set
            {
                SetValue<TextureBuffer>(ref _uTarget, value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GrayScale(RenderSystem renderer, string vertexShader, string fragShader)
            : base("GrayScale", renderer, vertexShader, fragShader)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uTarget = null;
        }
    }
}
