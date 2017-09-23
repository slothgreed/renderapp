using System;
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

        /// <summary>
        /// 配列バッファの作成
        /// </summary>
        /// <param name="target">バッファターゲット</param>
        /// <returns>配列バッファ</returns>
        public ArrayBuffer CreateArrayBuffer(BufferTarget target)
        {
            ArrayBuffer obj = new ArrayBuffer(target);
            obj.GenBuffer();
            AddItem(obj);
            return obj;
        }

        /// <summary>
        /// フレームバッファの作成
        /// </summary>
        /// <param name="name">バッファ名</param>
        /// <returns>フレームバッファ</returns>
        public FrameBuffer CreateFrameBuffer(string name)
        {
            FrameBuffer obj = new FrameBuffer(name);
            AddItem(obj);
            return obj;
        }

        /// <summary>
        /// レンダーバッファの作成
        /// </summary>
        /// <returns>レンダーバッファ</returns>
        public RenderBuffer CreateRenderBuffer()
        {
            RenderBuffer obj = new RenderBuffer();
            AddItem(obj);
            return obj;
        }

        /// <summary>
        /// サンプラバッファの作成
        /// </summary>
        /// <returns>サンプラバッファ</returns>
        public SamplerBuffer CreateSamplerBuffer()
        {
            SamplerBuffer obj = new SamplerBuffer();
            AddItem(obj);
            return obj;
        }

        /// <summary>
        /// テクスチャバッファの作成
        /// </summary>
        /// <param name="type">テクスチャタイプ</param>
        /// <returns>テクスチャバッファ</returns>
        public TextureBuffer CreateTextureBuffer(TextureType type)
        {
            TextureBuffer obj = new TextureBuffer(type);
            AddItem(obj);
            return obj;
        }
    }
}
