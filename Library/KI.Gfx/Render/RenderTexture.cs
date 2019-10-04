using KI.Gfx.GLUtil;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.Render
{
    /// <summary>
    /// レンダリング用のテクスチャバッファ
    /// </summary>
    public class RenderTexture : TextureBuffer
    {
        public RenderTexture(string name, int width, int height, PixelFormat format)
            : base(name, GLUtil.TextureType.Texture2D, width, height, format)
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
