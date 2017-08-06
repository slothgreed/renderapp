using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// サンプラバッファ
    /// </summary>
    public class SamplerBuffer : BufferObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SamplerBuffer()
        {
        }

        public int ActiveTexture { get; set; }

        /// <summary>
        /// バッファの生成
        /// </summary>
        public override void GenBufferCore()
        {
            DeviceID = GL.GenSampler();
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        public override void DisposeCore()
        {
            GL.DeleteSampler(DeviceID);
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        public override void BindBufferCore()
        {
            GL.BindSampler(ActiveTexture, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        public override void UnBindBufferCore()
        {
            GL.BindSampler(ActiveTexture, DeviceID);
        }
    }
}
