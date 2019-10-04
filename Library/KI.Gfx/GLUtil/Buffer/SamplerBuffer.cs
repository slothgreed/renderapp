using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.Buffer
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
        protected override void GenBufferCore()
        {
            DeviceID = GL.GenSampler();
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        protected override void BindBufferCore()
        {
            GL.BindSampler(ActiveTexture, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        protected override void UnBindBufferCore()
        {
            GL.BindSampler(ActiveTexture, DeviceID);
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        protected override void DisposeCore()
        {
            GL.DeleteSampler(DeviceID);
        }
    }
}
