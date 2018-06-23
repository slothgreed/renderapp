using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.KITexture;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.Render
{
    /// <summary>
    /// レンダリング用のテクスチャバッファ
    /// </summary>
    public class RenderTexture : Texture
    {
        public RenderTexture(string name, int width, int height)
            : base(name, GLUtil.TextureType.Texture2D, width, height)
        {

        }

        /// <summary>
        /// テクスチャアタッチメント
        /// </summary>
        public FramebufferAttachment Attachment { get; set; }

        /// <summary>
        /// ドローバッファ
        /// </summary>
        public DrawBuffersEnum DrawBuffers { get; set; }
    }
}
