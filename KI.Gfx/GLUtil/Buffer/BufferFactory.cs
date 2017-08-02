using KI.Foundation.Core;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil.Buffer
{
    /// <summary>
    /// バッファファクトリ
    /// </summary>
    public class BufferFactory : KIFactoryBase<BufferObject>
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        public static BufferFactory Instance { get; } = new BufferFactory();

        public ArrayBuffer CreateArrayBuffer()
        {
            ArrayBuffer obj = new ArrayBuffer();
            AddItem(obj);
            return obj;
        }

        public ArrayBuffer CreateArrayBuffer(BufferTarget target)
        {
            ArrayBuffer obj = new ArrayBuffer(target);
            AddItem(obj);
            return obj;
        }

        public FrameBuffer CreateFrameBuffer(string name)
        {
            FrameBuffer obj = new FrameBuffer(name);
            AddItem(obj);
            return obj;
        }

        public RenderBuffer CreateRenderBuffer()
        {
            RenderBuffer obj = new RenderBuffer();
            AddItem(obj);
            return obj;
        }

        public SamplerBuffer CreateSamplerBuffer()
        {
            SamplerBuffer obj = new SamplerBuffer();
            AddItem(obj);
            return obj;
        }

        public TextureBuffer CreateTextureBuffer(TextureType type)
        {
            TextureBuffer obj = new TextureBuffer(type);
            AddItem(obj);
            return obj;
        }
    }
}
